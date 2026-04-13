using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace PopUpOslo.Data
{
    public static class DatabaseInitializer
    {
        private static string dbPath = Path.Combine("Database", "database.db");
        private static string schemaPath = Path.Combine("Database", "schema.sql");

        public static void Initialize(bool seed = false)
        {
            Directory.CreateDirectory("Database");

            bool isNew = !File.Exists(dbPath);

            var connectionString = $"Data Source={dbPath}";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Enable foreign keys
                var pragma = connection.CreateCommand();
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();

                if (!File.Exists(schemaPath))
                {
                    Console.WriteLine("schema.sql not found!");
                    return;
                }

                string sql = File.ReadAllText(schemaPath);

                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
                if (seed)
                {
                    SeedDatabase(connection);
                }
                Console.WriteLine(isNew
                    ? "Database created and initialized."
                    : "Database already exists. Schema ensured.");
                
            }
            
            
        }
        
        private static void SeedDatabase(SqliteConnection connection)
        {
            var seedSql = File.ReadAllText("Data/seed.sql");

            using var command = connection.CreateCommand();
            command.CommandText = seedSql;
            command.ExecuteNonQuery();
        }
    }
}