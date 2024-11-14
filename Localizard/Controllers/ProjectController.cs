using Localizard._context;
using Localizard.Data.Migrations;
using Localizard.Models;
using Localizard.Views.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;

namespace Localizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _myEntityService;

        public ProjectController(ProjectService myEntityservice, ApplicationDbContext context)
        {
            _myEntityService = myEntityservice ?? throw new ArgumentNullException(nameof(myEntityservice));
            _context = context;
        }


        [Authorize(Roles = "user,admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Project myEntity)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                myEntity.UserId = userId;
                myEntity.CreatedAt = DateTime.Now;
                myEntity.UpdatedAt = DateTime.Now;

                _context.MyEntities.Add(myEntity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEntity), new { id = myEntity.Id }, myEntity);
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Project updatedEntity)
        {
           
            if (id != updatedEntity.Id)
            {
                return BadRequest("Project ID mismatch.");
            }

           
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is null or empty.");
            }

            var existingProject = await _context.MyEntities.FindAsync(id);
            if (existingProject == null)
            {
                return NotFound("Project not found.");
            }

            if (existingProject.UserId != userId && !User.IsInRole("admin"))
            {
                return Forbid(); 
            }

            
            existingProject.Name = updatedEntity.Name;
            existingProject.DefaultLAnguage = updatedEntity.DefaultLAnguage; 
            existingProject.AvailableLanguage = updatedEntity.AvailableLanguage ?? new List<string>();
            existingProject.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound("Project not found.");
                }
                return StatusCode(500, "Concurrency error while updating the project.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return NoContent(); 
        }

        private bool ProjectExists(int id)
        {
            return _context.MyEntities.Any(e => e.Id == id);
        }




        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _context.MyEntities.FindAsync(id);
                if (entity == null)
                {
                    Console.WriteLine($"Entity with id {id} not found.");
                    return NotFound(new { Message = $"Entity with id {id} not found." });
                }

                _context.MyEntities.Remove(entity);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"Entity with id {id} deleted successfully.", entity });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during delete: {ex.Message}");
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedResult<Project>>> GetAllEntities(int currentPage = 1, int pageSize = 10)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool isAdmin = User.IsInRole("admin");

                IQueryable<Project> query = isAdmin ? _context.MyEntities : _context.MyEntities.Where(e => e.UserId == userId);

                var totalRecords = await query.CountAsync();

                if(totalRecords == 0)
                {
                    return Ok(new PagedResult<Project>
                    {
                        CurrentPage = currentPage,
                        PageSize = pageSize,
                        TotalRecords = 0,
                        TotalPages = 0,
                        Data = new List<Project>()
                    });
                }

                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var entities = await query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new Project
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DefaultLAnguage = e.DefaultLAnguage,
                        AvailableLanguage = e.AvailableLanguage ?? new List<string>(),
                        CreatedAt = e.CreatedAt,
                        UpdatedAt = e.UpdatedAt,
                        UserId = e.UserId ?? string.Empty,
                    })
                    .ToListAsync();

                var response = new PagedResult<Project>
                {
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    Data = entities
                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetEntity(int id)
        {
            
                try
                {
                    Console.WriteLine($"Searching for entity with id: {id}");

                    var entity = await _context.MyEntities.FindAsync(id);
                    if (entity == null)
                    {
                        Console.WriteLine($"Entity with id {id} not found.");
                        return NotFound(new { Message = $"Entity with id {id} not found." });
                    }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if(entity.UserId != userId && !User.IsInRole("admin"))
                {
                    Console.WriteLine($"User with id {userId} is forbidden from accessing entity  with id {id}");
                    return Forbid();
                }

                return Ok(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
           
        }
        

    
}
