using PopUpOslo.Domain.Entities;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class AuthService
{
    private readonly UserRepository _userRepository = new();

    public bool Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        username = username.Trim();
        password = password.Trim();

        User? existingUser = _userRepository.GetUserByUsername(username);

        if (existingUser != null)
        {
            return false;
        }

        var user = new User
        {
            Username = username,
            PasswordHash = password,
            UserId = _nextUserId++,
            Username = username.Trim(),
            PasswordHash = password.Trim(),
            Role = "User"
        };

        _userRepository.AddUser(user);
        return true;
    }

    public User? Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }
        return _users.FirstOrDefault(u =>
            u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase) &&
            u.PasswordHash == password.Trim());
    }

        username = username.Trim();
        password = password.Trim();

        return _userRepository.ValidateUser(username, password);
    }

    public User? GetUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        return _userRepository.GetUserByUsername(username.Trim());
    }
}