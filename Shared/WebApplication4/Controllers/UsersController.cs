using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication4.Controller;


[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;
    
    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser(
        [FromBody] CreateUserDto request)
    {
        User user = new User
        {
            UserName = request.UserName,
            Password = request.Password
        };

        User created = await userRepository.AddAsync(user);

        UserDto dto = new UserDto
        {
            Id = created.Id,
            UserName = created.UserName
        };

        return Created($"/Users/{dto.Id}", dto);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers()
    {
        IEnumerable<UserDto> users = userRepository
            .GetMany()
            .Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName
            });

        return Ok(users);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            User user = await userRepository.GetSingleAsync(id);

            UserDto dto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return Ok(dto);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers(
        [FromQuery] string? username)
    {
        IQueryable<User> users = userRepository.GetMany();

        if (!string.IsNullOrWhiteSpace(username))
        {
            users = users.Where(user =>
                user.UserName.Contains(
                    username,
                    StringComparison.OrdinalIgnoreCase));
        }

        IEnumerable<UserDto> result = users.Select(user =>
            new UserDto
            {
                Id = user.Id,
                UserName = user.UserName
            });

        return Ok(result);
    }
}