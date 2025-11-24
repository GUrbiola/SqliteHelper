using System.Text;

namespace SqliteHelper
{
    public static class SqlGenerator
    {
        public static string GenerateInitializationScript(DatabaseProject project)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-- SQLite Database Initialization Script");
            sb.AppendLine($"-- Project: {project.Name}");
            sb.AppendLine($"-- Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();

            foreach (var table in project.Tables)
            {
                sb.AppendLine(GenerateCreateTableStatement(table));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string GenerateUpdateScript(DatabaseProject project)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-- SQLite Database Update Script");
            sb.AppendLine($"-- Project: {project.Name}");
            sb.AppendLine($"-- Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("-- This script drops existing tables and recreates them");
            sb.AppendLine();

            foreach (var table in project.Tables)
            {
                sb.AppendLine($"-- Drop table if exists: {table.Name}");
                sb.AppendLine($"DROP TABLE IF EXISTS [{table.Name}];");
                sb.AppendLine();

                sb.AppendLine(GenerateCreateTableStatement(table));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string GenerateCreateTableStatement(TableDefinition table)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE [{table.Name}] (");

            var columnDefinitions = new List<string>();
            var primaryKeys = new List<string>();

            foreach (var column in table.Columns)
            {
                var columnDef = new StringBuilder();
                columnDef.Append($"    [{column.Name}] {column.DataType}");

                if (column.IsPrimaryKey)
                {
                    primaryKeys.Add(column.Name);
                    if (column.IsAutoIncrement)
                    {
                        columnDef.Append(" PRIMARY KEY AUTOINCREMENT");
                    }
                }

                if (column.IsNotNull && !column.IsPrimaryKey)
                {
                    columnDef.Append(" NOT NULL");
                }

                if (!string.IsNullOrWhiteSpace(column.DefaultValue))
                {
                    if(column.DataType.ToUpper() == "TEXT" && !(column.DefaultValue.StartsWith("'") && column.DefaultValue.EndsWith("'")))
                    {
                        // Ensure text default values are quoted
                        columnDef.Append($" DEFAULT '{column.DefaultValue}'");
                    }
                    else
                    {
                        columnDef.Append($" DEFAULT {column.DefaultValue}");
                    }
                }

                columnDefinitions.Add(columnDef.ToString());
            }

            // Add composite primary key if multiple columns are marked as primary key
            if (primaryKeys.Count > 1)
            {
                columnDefinitions.Add($"    PRIMARY KEY ([{string.Join("], [", primaryKeys)}])");
            }

            sb.AppendLine(string.Join(",\n", columnDefinitions));
            sb.Append(");");

            return sb.ToString();
        }
    }
}
