using Localizard._context;
using Localizard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Localizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LanguageController(ApplicationDbContext context)
        {
            _context = context;
        }

     
        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> GetLanguage()
        {
            
                var languages = await _context.languages.ToListAsync();
                return Ok(languages);
            
           
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Language>>GetLanguageById(int id)
        {
            var language = await _context.languages.FirstOrDefaultAsync(l => l.Id == id);
            if(language == null)
            {
                return NotFound("Language not found");
            }
            return Ok(language);
        }
      

        [Authorize(Roles = "user,admin")]
        [HttpPost]
        public async Task<ActionResult<Language>> CreateLanguage([FromBody] Language language1)
        {
            if (language1 == null || string.IsNullOrEmpty(language1.Name) ||
                language1.PluralForms == null || string.IsNullOrEmpty(language1.LanguageCode))
            {
                return BadRequest("Invalid language data.");
            }

            try
            {
                await _context.languages.AddAsync(language1);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateLanguage), new { id = language1.Id }, language1);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
            }

            
        }
        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLanguage(int id, [FromBody] Language language)
        {
            if (language == null || id != language.Id || string.IsNullOrEmpty(language.Name) ||
                language.PluralForms == null || string.IsNullOrEmpty(language.LanguageCode))
            {
                return BadRequest("Invalid language data or ID mismatch.");
            }

    
            var existingLanguage = await _context.languages
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (existingLanguage == null)
            {
                return NotFound("Language not found.");
            }

            _context.languages.Remove(existingLanguage);
            await _context.SaveChangesAsync();

          
            await _context.languages.AddAsync(language);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "A concurrency error occurred while updating the language.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        private async Task<bool> LanguageExists(int id)
        {
            return await _context.languages.AnyAsync(e => e.Id == id);
        }






        [Authorize(Roles = "user,admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguage( int id)
        {
            var language = await _context.languages
                .FirstOrDefaultAsync(l => l.Id == id);
            if(language == null)
            {
                return NotFound();
            }

            _context.languages.Remove(language);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
