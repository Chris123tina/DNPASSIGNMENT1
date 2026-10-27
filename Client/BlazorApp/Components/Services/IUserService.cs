using ApiContracts;

namespace Blazor.Components.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<UserDto> GetUserAsync(int id);
  
    
}