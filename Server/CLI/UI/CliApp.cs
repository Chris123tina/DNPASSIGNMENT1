using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
  private readonly  IUserRepository userRepository;
  private readonly IPostRepository postRepository;
  private readonly ICommentRepository commentRepository;


  public CliApp(
    IUserRepository userRepository,
    IPostRepository postRepository,
    ICommentRepository commentRepository)

  {
    this.userRepository = userRepository;
    this.postRepository = postRepository;
    this.commentRepository = commentRepository;
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
          await CreateUserAsync();
          break;

        case "2":
          await CreatePostAsync();
          break;

        case "3":
          await AddCommentAsync();
          break;

        case "4":
          ViewPosts();
          break;

        case "5":
          await ViewPostAsync();
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
  
  private async Task CreateUserAsync()
  {
    Console.Write("Username: ");
    string? username = Console.ReadLine();

    Console.Write("Password: ");
    string? password = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(username) ||
        string.IsNullOrWhiteSpace(password))
    {
      Console.WriteLine("Username and password cannot be empty.");
      return;
    }

    bool usernameExists =
      userRepository.GetMany()
        .Any(user => user.UserName == username);

    if (usernameExists)
    {
      Console.WriteLine("Username already exists.");
      return;
    }

    User user = new User
    {
      UserName = username,
      Password = password
    };

    User createdUser = await userRepository.AddAsync(user);

    Console.WriteLine(
      $"User created successfully with ID {createdUser.Id}.");
  }
  private async Task CreatePostAsync()
  {
    Console.Write("User ID: ");

    if (!int.TryParse(Console.ReadLine(), out int userId))
    {
      Console.WriteLine("Invalid user ID.");
      return;
    }

    bool userExists =
      userRepository.GetMany()
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

    Post createdPost = await postRepository.AddAsync(post);

    Console.WriteLine(
      $"Post created successfully with ID {createdPost.Id}.");
  }
  
  private async Task AddCommentAsync()
  {
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
      userRepository.GetMany()
        .Any(user => user.Id == userId);

    if (!userExists)
    {
      Console.WriteLine("User does not exist.");
      return;
    }

    bool postExists =
      postRepository.GetMany()
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
      $"Comment created successfully with ID {createdComment.Id}.");
  }
  
  private void ViewPosts()
  {
    IQueryable<Post> posts = postRepository.GetMany();

    Console.WriteLine();
    Console.WriteLine("===== POSTS =====");

    foreach (Post post in posts)
    {
      Console.WriteLine($"[{post.Id}] {post.Title}");
    }
  }
  
  private async Task ViewPostAsync()
  {
    Console.Write("Enter post ID: ");

    if (!int.TryParse(Console.ReadLine(), out int postId))
    {
      Console.WriteLine("Invalid post ID.");
      return;
    }

    try
    {
      Post post = await postRepository.GetSingleAsync(postId);

      Console.WriteLine();
      Console.WriteLine("===== POST =====");
      Console.WriteLine($"ID: {post.Id}");
      Console.WriteLine($"Title: {post.Title}");
      Console.WriteLine($"Body: {post.Body}");
      Console.WriteLine($"Written by User ID: {post.UserId}");

      IQueryable<Comment> comments =
        commentRepository.GetMany()
          .Where(comment => comment.postId == post.Id);

      Console.WriteLine();
      Console.WriteLine("===== COMMENTS =====");

      if (!comments.Any())
      {
        Console.WriteLine("No comments.");
        return;
      }

      foreach (Comment comment in comments)
      {
        Console.WriteLine(
          $"User {comment.userId}: {comment.body}");
      }
    }
    catch (InvalidOperationException e)
    {
      Console.WriteLine(e.Message);
    }
  }
}