using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;


    public ManagePostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }
    
    public async Task StartAsync()
    {
        Console.Write("Enter post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }

        try
        {
            Post post =
                await postRepository.GetSingleAsync(postId);

            Console.WriteLine();
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Body: {post.Body}");

            Console.WriteLine();
            Console.WriteLine("Comments:");

            IQueryable<Comment> comments =
                commentRepository
                    .GetMany()
                    .Where(comment => comment.PostId == post.Id);
            
            foreach (Comment comment in comments)
            {
                Console.WriteLine(
                    $"User {comment.UserId}: {comment.Body}");
            }
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}