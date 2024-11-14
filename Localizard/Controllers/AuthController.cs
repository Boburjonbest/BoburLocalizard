using Microsoft.AspNetCore.Mvc;
using Localizard.Models;
using Localizard.Views.Services;
using Microsoft.AspNetCore.Authorization;
using Localizard._context;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Localizard.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public AuthController(TokenService tokenService, ApplicationDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _tokenService.GenerateJwtToken(user);

                    
            var response = new AuthResponse(token, user.Username, user.Role);

            return Ok(new
            {
                token,
                role = user.Role
               

            });

          
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid registration request");
              
            }
          

            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == model.Username);
            if (existingUser != null)
            {
                return Conflict(new { message = "Username already exists" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Username = model.Username,
                PasswordHash = hashedPassword,
                Role = model.Role 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

           
            var token =_tokenService.GenerateJwtToken(user);
          

           

            return Ok(new
            {
                token,
                message = "User registered successfully"

            });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult>Remove(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound($"Пользователь с ID {userId} не найден");

            user.Role = "User";

            _context.Users.Remove(user);

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Пользователь с ID {userId} успешно удалён.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Ошибка при удаления пользователья: {ex.Message}");
            }

           
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(int userId, [FromBody] User updateUser)
        {
            if (userId != updateUser.Id)
            {
                return BadRequest("ID пользователя в параметре и в теле запроса не совпадают.");

            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (existingUser == null)
            {
                return NotFound($"Пользователь с ID {userId} не найден");
            }

            if (!string.IsNullOrEmpty(updateUser.Username))
            {
                existingUser.Username = updateUser.Username;
            }

        
            existingUser.Role = updateUser.Role;

            if (!string.IsNullOrEmpty(updateUser.PasswordHash))
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUser.PasswordHash);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Пользователь с ID {userId} успешно обновлён.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Ошибка при обновления пользователья: {ex.Message}");
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult>GetUsers(int currentPage = 1, int pageSize = 10)
        {
            var totalRecords = await _context.Users.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var users = await _context.Users
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                currentPage,
                pageSize,
                totalRecords,
                totalPages,
                data = users
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("personal-data")]
        public async Task<IActionResult> GetPersonalData()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var userData = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Username,
                    u.Role

                }).FirstOrDefaultAsync();

            if(userData == null)
            {
                return NotFound("Не найдено Пользователей.");
            }

            return Ok(userData);
        }
    }
}
