using Localizard._context;
using Localizard.Data.Migrations;
using Localizard.Models;
using Localizard.Views.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Localizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class ProyController : ControllerBase
    {

        private readonly  ApplicationDbContext _context;
        
        public ProyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("GetProjects")]
        public async Task<IActionResult> GetProject([FromQuery] string projectName)
        {
            var myEntity = await _context.MyEntities
                 .Where(e => e.Name == projectName)
                 .FirstOrDefaultAsync();

            if(myEntity == null)
            {
                return NotFound("No project found the specified projectName.");
            }

            var obyektPerevods = await _context.ObyektPerevods
                .Where(p => p.ParentId == myEntity.Id)
                .Include(p => p.Translations)
                .ToListAsync();

            if(obyektPerevods == null || !obyektPerevods.Any())
            {
                return NotFound("No Obyektperevod found associated with the specified  projectName.");
            }

            var result = obyektPerevods.Select(p => new
            {
                id = p.Id,
                namekeys = p.Namekeys,
                parent = p.ParentId,
                tags = p.Tags,
                translations = p.Translations.Select(t => new
                {
                    t.Key,
                    t.Language,
                    t.Text
                }).ToList()
            }).ToList();
            

            return Ok(result);



        }
    }
}
