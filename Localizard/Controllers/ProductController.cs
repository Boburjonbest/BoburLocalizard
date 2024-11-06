using Localizard._context;
using Localizard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Localizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostObyektPerevod([FromBody] ObyektPerevod obyektPerevod)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.ObyektPerevods.Add(obyektPerevod);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = obyektPerevod.Id }, obyektPerevod);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           
                 var result = await _context.ObyektPerevods
                                        .Include(p => p.Translations)
                                        .FirstOrDefaultAsync(p => p.Id == id);
                 if (result == null) return NotFound();
                   return Ok(result);
           
           
        }

        [Authorize(Roles ="user,admin")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search, [FromQuery] int? parentId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            IQueryable<ObyektPerevod> query = _context.ObyektPerevods.Include(p => p.Translations);
              

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Namekeys.Contains(search));
            }

            if (parentId.HasValue)
            {
                query = query.Where(p => p.ParentId == parentId.Value);
            }

            var obyektResults = await query.ToListAsync();

            if (!obyektResults.Any())
            {
                return Ok(new List<object>());
            }

            var response = obyektResults.Select(p => new

            {
                ProjectID = p.Id,
                ProjectName = p.Namekeys,
                ProductDesc = p.Description,
                ProductTags = p.Tags,
            
                Parent = p.ParentId,
                Translation = p.Translations.Select(t => new { t.Key, t.Language, t.Text }).ToList()
            });
            return Ok(response);
            
           
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ObyektPerevod obyektPerevod) 
        {
            if(id != obyektPerevod.Id)
            {
                return BadRequest(new { message = "Такого Id не совпадает" });
            }

            var existingPerevod = await _context.ObyektPerevods
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(existingPerevod == null)
            {
                return NotFound(new { message = "не найдено обьект" });
            }

            existingPerevod.Namekeys = obyektPerevod.Namekeys;
            existingPerevod.Description = obyektPerevod.Description;
            existingPerevod.Tags = obyektPerevod.Tags;
            existingPerevod.ParentId = obyektPerevod.ParentId;
            

            _context.ObyektTranslations.RemoveRange(existingPerevod.Translations);
            foreach(var translation in obyektPerevod.Translations)
            {
                translation.ObyektPerevodId = existingPerevod.Id;
            }
            _context.ObyektTranslations.AddRange(obyektPerevod.Translations);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "во время изменеия ошибка возникает" });
            }

            return Ok(new { message = "Успешно обновлен" });


        }

        [Authorize]
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var exiting = await _context.ObyektPerevods.FindAsync(id);
            if (exiting == null) return NotFound();

            _context.ObyektPerevods.Remove(exiting);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
    
}
