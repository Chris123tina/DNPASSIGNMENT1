using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
 
    private readonly List<Post> posts = new();
    
    
    
    public PostInMemoryRepository ()
        {
            posts.Add(new Post
            {
                Id = 1,
                Title = "AI and Humans",
                Body = "The change in humanity",
                UserId = 1
            });
           
            posts.Add(new Post
            {
                Id = 2,
                Title = "Religion in 2026",
                Body = "A closer look into how religion is dying or whatever",
                UserId = 2
            });
            
            posts.Add(new Post
            {
                Id = 3,
                Title = "This shit again ",
                Body = "Basically what the title says",
                UserId = 1
            });
            
            
            
        }
    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }
    
    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }
    
    
    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.FirstOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(post);
    }
    
    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}