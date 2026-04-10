using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;
using Microsoft.Data.Sqlite;

namespace PopUpOslo.Infrastructure.Repositories;

public class UserRepository : BaseRepository
{
    //  Register User
    public void AddUser(User user)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Users (Username, PasswordHash)
                            VALUES (@u, @p)";

        cmd.Parameters.AddWithValue("@u", user.Username);
        cmd.Parameters.AddWithValue("@p", user.PasswordHash);

        cmd.ExecuteNonQuery();
    }

    //  Get user by username (for login)
    public User GetUserByUsername(string username)
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
                PasswordHash = reader.GetString(2)
            };
        }
        else
        {
            throw new Exception("User not found");
        }

    }

    // Validate login
    public User ValidateUser(string username, string passwordHash)
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
                PasswordHash = reader.GetString(2)
            };
        }
        else
        {
            throw new Exception("Invalid username or password");
        }
    }

    //  Get user by ID
    public User GetUserById(int userId)
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
                PasswordHash = reader.GetString(2)
            };
        }
        else
        {
            throw new Exception("User not found");
        }
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