
using ApiContracts;

namespace BlazorApp.HttpServices;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient _client;

    public HttpCommentService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<CommentDto>>
        GetCommentsForPostAsync(int postId)
    {
        return await _client
                   .GetFromJsonAsync<List<CommentDto>>(
                       $"comments?postId={postId}")
               ?? new List<CommentDto>();
    }

    public async Task<CommentDto> AddCommentAsync(
        CreateCommentDto request)
    {
        HttpResponseMessage response =
            await _client.PostAsJsonAsync(
                "comments", request);

        response.EnsureSuccessStatusCode();

        return await response.Content
                   .ReadFromJsonAsync<CommentDto>()
               ?? throw new Exception("No comment returned");
    }
}