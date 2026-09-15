using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public CreateCommentView(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }
    
    public async Task StartAsync()
    {
        Console.WriteLine();
        Console.WriteLine("===== ADD COMMENT =====");

        Console.Write("User ID: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        Console.Write("Post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID.");
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

        bool postExists =
            postRepository
                .GetMany()
                .Any(post => post.Id == postId);

        if (!postExists)
        {
            Console.WriteLine("Post does not exist.");
            return;
        }

        Console.Write("Comment: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty.");
            return;
        }
        Comment comment = new Comment
        {
            body = body,
            userId = userId,
            postId = postId
        };

        Comment createdComment =
            await commentRepository.AddAsync(comment);

        Console.WriteLine(
            $"Comment created with ID {createdComment.Id}.");
    }
}