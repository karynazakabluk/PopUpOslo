using System.Data.SQLite;

public class UserRepository
{
    public void AddUser(User user)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Users (Username, Password, Role) VALUES (@u, @p, @r)";
        cmd.Parameters.AddWithValue("@u", user.Username);
        cmd.Parameters.AddWithValue("@p", user.Password);
        cmd.Parameters.AddWithValue("@r", user.Role);

        cmd.ExecuteNonQuery();
    }

    public User GetUserByUsername(string username)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Users WHERE Username = @u";
        cmd.Parameters.AddWithValue("@u", username);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
        }

        return null;
    }

    public User GetUserById(int id)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Users WHERE UserId = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
        }

        return null;
    }
}