using ApiContracts;

namespace Blazor.Components.Services;

public interface IPostService
{
    Task<List<PostDto>> GetPostsAsync();
    Task<PostDto> GetPostAsync(int id);
    Task<PostDto> CreatePostAsync(CreatePostDto request);
}