using Localizard._context;
using Localizard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Localizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class Tags : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Tags (ApplicationDbContext context)
        {
            _context = context;
        }
        //Tags POST
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] Tag tags)
        {
            
            
            if (ModelState.IsValid)
            {
                _context.Tags.Add(tags);
                _context.SaveChanges();
                return Ok(tags);
            }

            return BadRequest();
        }

        //Tags GET
        [Authorize]
        [HttpGet("tags")]
        public IActionResult GetTags()
        {
           
                var tags = _context.Tags.ToList();
                return Ok(tags);
        }
           
        

       

    }
}
