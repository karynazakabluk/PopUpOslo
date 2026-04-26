using PopUpOslo.Domain.Entities;
using Microsoft.Data.Sqlite;
using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Infrastructure.Repositories;

public class UserRepository : BaseRepository
{
    // Register User
    public void AddUser(User user)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Users (Username, PasswordHash, Role)
                            VALUES (@u, @p, @r)";

        cmd.Parameters.AddWithValue("@u", user.Username);
        cmd.Parameters.AddWithValue("@p", user.PasswordHash);
        cmd.Parameters.AddWithValue("@r", user.Role);

        cmd.ExecuteNonQuery();
    }

    // Get user by username
    public User? GetUserByUsername(string username)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Users WHERE Username=@u";
        cmd.Parameters.AddWithValue("@u", username);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Role = (UserRole)reader.GetInt32(3) 
            };
        }

        return null;
    }

    // Validate login
    public User? ValidateUser(string username, string passwordHash)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Users 
                            WHERE Username=@u AND PasswordHash=@p";

        cmd.Parameters.AddWithValue("@u", username);
        cmd.Parameters.AddWithValue("@p", passwordHash);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Role = (UserRole)reader.GetInt32(3)
            };
        }

        return null;
    }

    // Get user by ID
    public User? GetUserById(int userId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Users WHERE UserId=@id";
        cmd.Parameters.AddWithValue("@id", userId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Role = (UserRole)reader.GetInt32(3)
            };
        }

        return null;
    }

    // Delete user
    public void DeleteUser(int userId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Users WHERE UserId=@id";
        cmd.Parameters.AddWithValue("@id", userId);

        cmd.ExecuteNonQuery();
    }
}