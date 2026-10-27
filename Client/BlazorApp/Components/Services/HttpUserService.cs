using ApiContracts;

namespace Blazor.Components.Services;

public class HttpUserService: IUserService
{
    private readonly HttpClient client;
    
    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)

    {
        HttpResponseMessage response = await client.PostAsJsonAsync("users", request);
        response.EnsureSuccessStatusCode();
        UserDto? user =
            await response.Content.ReadFromJsonAsync<UserDto>();

        return user ?? throw new Exception(
            "The API returned no user.");
        
    }
    
    public async Task<UserDto> GetUserAsync(int id)
    {
        return await client.GetFromJsonAsync<UserDto>(
                   $"users/{id}")
               ?? throw new Exception("User not found");
    }
}