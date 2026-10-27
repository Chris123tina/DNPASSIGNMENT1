
using ApiContracts;
using Blazor.Components.Services;


public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<List<PostDto>> GetPostsAsync()
    {
        return await client.GetFromJsonAsync<List<PostDto>>(
            "posts") ?? new();
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        return await client.GetFromJsonAsync<PostDto>(
                   $"posts/{id}")
               ?? throw new Exception("Post not found");
    }

    public async Task<PostDto> CreatePostAsync(
        CreatePostDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("posts", request);

        response.EnsureSuccessStatusCode();

        return await response.Content
                   .ReadFromJsonAsync<PostDto>()
               ?? throw new Exception("No post returned");
    }
}