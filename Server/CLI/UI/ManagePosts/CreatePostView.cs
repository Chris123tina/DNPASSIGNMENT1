using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(
        IPostRepository postRepository,

        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }
    
    public async Task StartAsync()
    {
        Console.WriteLine();
        Console.WriteLine("===== CREATE POST =====");

        Console.Write("User ID: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        bool userExists =
            userRepository
                .GetMany()
                .Any(user => user.Id == userId);

        if (!userExists)
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        Console.Write("Title: ");
        string? title = Console.ReadLine();

        Console.Write("Body: ");
        string? body = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(title) ||
            string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Title and body cannot be empty.");
            return;
        }

        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        Post createdPost =
            await postRepository.AddAsync(post);

        Console.WriteLine(
            $"Post created with ID {createdPost.Id}.");
    }
}