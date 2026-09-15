using CLI.UI.ManageComments;

using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly CreateUserView createUserView;
    private readonly CreatePostView createPostView;
    private readonly CreateCommentView createCommentView;
    private readonly ListPostsView listPostsView;
    private readonly ManagePostView viewPostView;

    public CliApp(
        IUserRepository userRepository,
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        createUserView = new CreateUserView(userRepository);

        createPostView =
            new CreatePostView(postRepository, userRepository);

        createCommentView =
            new CreateCommentView(
                commentRepository,
                userRepository,
                postRepository);

        listPostsView =
            new ListPostsView(postRepository);

        viewPostView =
            new ManagePostView(postRepository, commentRepository);
    }

    public async Task StartAsync()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("===== FORUM =====");
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. Create post");
            Console.WriteLine("3. Add comment");
            Console.WriteLine("4. View posts");
            Console.WriteLine("5. View specific post");
            Console.WriteLine("0. Exit");

            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await createUserView.StartAsync();
                    break;

                case "2":
                    await createPostView.StartAsync();
                    break;

                case "3":
                    await createCommentView.StartAsync();
                    break;

                case "4":
                    listPostsView.Show();
                    break;

                case "5":
                    await viewPostView.StartAsync();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}