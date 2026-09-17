using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    private async Task<List<Post>> LoadPostsAsync()
    {
        string json = await File.ReadAllTextAsync(filePath);

        return JsonSerializer.Deserialize<List<Post>>(json)
               ?? new List<Post>();
    }

    private async Task SavePostsAsync(List<Post> posts)
    {
        string json = JsonSerializer.Serialize(
            posts,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        await File.WriteAllTextAsync(filePath, json);
    }

    
    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();

        post.Id = posts.Count == 0
            ? 1
            : posts.Max(p => p.Id) + 1;

        posts.Add(post);

        await SavePostsAsync(posts);

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();

        int index = posts.FindIndex(p => p.Id == post.Id);

        if (index == -1)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found.");
        }

        posts[index] = post;

        await SavePostsAsync(posts);
    }

    
    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await LoadPostsAsync();

        Post? postToRemove =
            posts.SingleOrDefault(p => p.Id == id);

        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found.");
        }

        posts.Remove(postToRemove);

        await SavePostsAsync(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await LoadPostsAsync();

        Post? post =
            posts.SingleOrDefault(p => p.Id == id);

        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found.");
        }

        return post;
    }
    
    public IQueryable<Post> GetMany()
    {
        string json =
            File.ReadAllTextAsync(filePath).Result;

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(json)
            ?? new List<Post>();

        return posts.AsQueryable();
    }
}