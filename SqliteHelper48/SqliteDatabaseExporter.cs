using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace SqliteHelper48
{
    public class SqliteDatabaseExporter
    {
        public static bool CreateDatabaseWithSchema(DatabaseProject project, string databasePath, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Delete existing file if it exists
                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                }

                // Create connection string
                var connectionString = $"Data Source={databasePath};";

                // Create and open connection (this creates the database file)
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Generate SQL script using existing SqlGenerator
                    string createScript = SqlGenerator.GenerateInitializationScript(project);

                    // Split script into individual statements and execute each one
                    var statements = createScript.Split(new[] { ";\r\n", ";\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var statement in statements)
                    {
                        var trimmedStatement = statement.Trim();
                        if (string.IsNullOrEmpty(trimmedStatement))
                            continue;

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = trimmedStatement;
                            command.ExecuteNonQuery();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error creating database: {ex.Message}";

                // Clean up failed database file
                if (File.Exists(databasePath))
                {
                    try { File.Delete(databasePath); } catch { }
                }

                return false;
            }
        }

        private static string GenerateSampleInsert(TableDefinition table)
        {
            if (table.Columns == null || table.Columns.Count == 0)
                return string.Empty;

            var columns = new List<string>();
            var values = new List<string>();

            foreach (var column in table.Columns)
            {
                // Skip auto-increment columns
                if (column.IsAutoIncrement)
                    continue;

                columns.Add(column.Name);

                // Generate sample value based on data type
                string sampleValue = GenerateSampleValue(column);
                values.Add(sampleValue);
            }

            if (columns.Count == 0)
                return string.Empty;

            return $"INSERT INTO {table.Name} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)});";
        }

        private static string GenerateSampleValue(ColumnDefinition column)
        {
            // If there's a default value, use it
            if (!string.IsNullOrEmpty(column.DefaultValue))
            {
                return column.DefaultValue;
            }

            // Generate based on data type
            switch (column.DataType.ToUpper())
            {
                case "INTEGER":
                    return "1";
                case "REAL":
                    return "1.0";
                case "TEXT":
                    return $"'Sample {column.Name}'";
                case "BLOB":
                    return "X'00'";
                case "NUMERIC":
                    return "1";
                default:
                    return "NULL";
            }
        }
    }
}
