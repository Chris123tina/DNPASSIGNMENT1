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
}