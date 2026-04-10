using PopUpOslo.Domain.Entities;

namespace PopUpOslo.Services;

public class AuthService
{
    private readonly List<User> _users = new();
    private int _nextUserId = 1;

    public AuthService()
    {
        SeedSampleUsers();
    }

    public bool Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        bool exists = _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            return false;
        }

        var user = new User
        {
            UserId = _nextUserId++,
            Username = username.Trim(),
            PasswordHash = password.Trim(),
            Role = "User"
        };

        _users.Add(user);
        return true;
    }

    public User? Login(string username, string password)
    {
        return _users.FirstOrDefault(u =>
            u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase) &&
            u.PasswordHash == password.Trim());
    }

    public User? GetUserByUsername(string username)
    {
        return _users.FirstOrDefault(u =>
            u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    private void SeedSampleUsers()
    {
        _users.Add(new User
        {
            UserId = _nextUserId++,
            Username = "anna",
            PasswordHash = "1234",
            Role = "User"
        });

        _users.Add(new User
        {
            UserId = _nextUserId++,
            Username = "maria",
            PasswordHash = "1234",
            Role = "User"
        });
    }
}