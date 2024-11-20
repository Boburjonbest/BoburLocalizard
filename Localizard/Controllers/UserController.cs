using AutoMapper;
using Localizard.DAL;
using Localizard.DAL.Repositories;
using Localizard.Domain.Entites;
using Localizard.Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Localizard.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    
    public UserController(IUserRepo userRepo, IMapper mapper )
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }
    
    
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _userRepo.GetAllUsers();
        var mappedUsers = _mapper.Map<List<GetUsersView>>(users);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(mappedUsers);
    }
    
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        if (!_userRepo.UserExists(id))
            return NotFound();

        var user = await _userRepo.GetByIdAsync(id);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(user);
    }
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateUser([FromBody] UserView userCreate)
    {
        if (userCreate == null)
            return BadRequest(ModelState);

        var user = _userRepo.GetAllUsers()
            .Where(p => p.Username.Trim().ToUpper() == userCreate.Username.TrimEnd().ToUpper())
            .FirstOrDefault();

        if (user != null)
        {
            ModelState.AddModelError("", "Project already exist!");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userMap = _mapper.Map<User>(userCreate);

        if (!_userRepo.CreateUser(userMap))
        {
            ModelState.AddModelError("", "Something went wrong! while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut]
    public IActionResult UpdateUser(int userId, [FromBody] User updateUser)
    {
        if (updateUser == null)
            return BadRequest(ModelState);

        if (userId != updateUser.Id)
            return BadRequest(ModelState);

        if (!_userRepo.UserExists(userId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();
        if (!_userRepo.UpdateUser(updateUser))
        {
            ModelState.AddModelError("", "Something went wrong while updating the user");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        if (!_userRepo.UserExists(userId))
            return NotFound();

        var userDelete = await _userRepo.GetByIdAsync(userId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_userRepo.DeleteUser(userDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting translation");
        }

        return NoContent();
    }
}