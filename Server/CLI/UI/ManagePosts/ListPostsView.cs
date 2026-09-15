using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public void Show()
    {
        Console.WriteLine();
        Console.WriteLine("===== POSTS =====");

        IQueryable<Post> posts =
            postRepository.GetMany();

        foreach (Post post in posts)
        {
            Console.WriteLine(
                $"[{post.Id}] {post.Title}");
        }
    }
}