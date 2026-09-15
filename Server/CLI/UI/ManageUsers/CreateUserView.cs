using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine();
        Console.WriteLine("===== CREATE USER =====");

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
            userRepository
                .GetMany()
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

        User createdUser =
            await userRepository.AddAsync(user);

        Console.WriteLine(
            $"User created with ID {createdUser.Id}.");
    }
}
