using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace PopUpOslo.Data
{
    public static class DatabaseInitializer
    {
        private static string ProjectRoot =>
            Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

        private static string DbPath =>
            Path.Combine(ProjectRoot, "Database", "database.db");

        private static string SchemaPath =>
            Path.Combine(ProjectRoot, "Database", "schema.sql");

        private static string SeedPath =>
            Path.Combine(ProjectRoot, "Data", "seed.sql");

        public static void Initialize(bool seed = false, bool reset = false)
        {
            Directory.CreateDirectory(Path.Combine(ProjectRoot, "Database"));

            if (reset && File.Exists(DbPath))
            {
                File.Delete(DbPath);
                Console.WriteLine("Database reset.");
            }

            bool isNew = !File.Exists(DbPath);

            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var pragma = connection.CreateCommand();
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();

            ApplySchema(connection);

            if (seed)
            {
                SeedDatabase(connection);
            }

            Console.WriteLine(isNew
                ? "Database created successfully."
                : "Database opened successfully.");
        }

        private static void ApplySchema(SqliteConnection connection)
        {
            if (!File.Exists(SchemaPath))
                throw new FileNotFoundException($"Schema not found: {SchemaPath}");

            string schemaSql = File.ReadAllText(SchemaPath);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = schemaSql;
            cmd.ExecuteNonQuery();

            Console.WriteLine("Schema applied.");
        }

        private static void SeedDatabase(SqliteConnection connection)
        {
            if (!File.Exists(SeedPath))
            {
                Console.WriteLine("Seed file not found.");
                return;
            }

            var seedSql = File.ReadAllText(SeedPath);

            // Split by semicolon safely
            var commands = seedSql
                .Split(';', StringSplitOptions.RemoveEmptyEntries);

            using var transaction = connection.BeginTransaction();

            foreach (var cmdText in commands)
            {
                var trimmed = cmdText.Trim();
                if (string.IsNullOrWhiteSpace(trimmed))
                    continue;

                using var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = trimmed;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Seed error on statement:");
                    Console.WriteLine(trimmed);
                    Console.WriteLine("Error: " + ex.Message);
                    throw;
                }
            }

            transaction.Commit();

            Console.WriteLine("Seed inserted successfully.");
        }
    }
}