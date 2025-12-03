using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace SqliteHelper48
{
    public class DatabaseManager : IDisposable
    {
        private SqliteConnection? connection;
        public string? DatabasePath { get; private set; }

        public bool OpenDatabase(string path, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                CloseDatabase();

                if (!File.Exists(path))
                {
                    errorMessage = $"Database file not found: {path}";
                    return false;
                }

                DatabasePath = path;
                var connectionString = $"Data Source={path};Mode=ReadWrite";
                connection = new SqliteConnection(connectionString);
                connection.Open();

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error opening database: {ex.Message}";
                connection = null;
                DatabasePath = null;
                return false;
            }
        }

        public void CloseDatabase()
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
                connection = null;
            }
            DatabasePath = null;
        }

        public List<string> GetTableNames()
        {
            var tables = new List<string>();

            if (connection == null || connection.State != ConnectionState.Open)
                return tables;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' ORDER BY name";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
            }
            catch
            {
                // Return empty list on error
            }

            return tables;
        }

        public List<string> GetViewNames()
        {
            var views = new List<string>();

            if (connection == null || connection.State != ConnectionState.Open)
                return views;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='view' ORDER BY name";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    views.Add(reader.GetString(0));
                }
            }
            catch
            {
                // Return empty list on error
            }

            return views;
        }

        public List<string> GetIndexNames()
        {
            var indexes = new List<string>();

            if (connection == null || connection.State != ConnectionState.Open)
                return indexes;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='index' AND name NOT LIKE 'sqlite_%' ORDER BY name";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    indexes.Add(reader.GetString(0));
                }
            }
            catch
            {
                // Return empty list on error
            }

            return indexes;
        }

        public List<TableColumnInfo> GetTableColumnInfo(string tableName)
        {
            var columns = new List<TableColumnInfo>();

            if (connection == null || connection.State != ConnectionState.Open)
                return columns;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"PRAGMA table_info('{tableName}')";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var columnName = reader.GetString(1);
                    var sqlType = reader.GetString(2).ToUpper();
                    var notNull = reader.GetInt32(3) == 1;
                    var defaultValue = reader.IsDBNull(4) ? null : reader.GetValue(4).ToString();
                    var isPrimaryKey = reader.GetInt32(5) == 1;

                    // Determine .NET type from SQLite type
                    Type netType = sqlType switch
                    {
                        string s when s.Contains("INT") => typeof(long),
                        string s when s.Contains("REAL") || s.Contains("DOUBLE") || s.Contains("FLOAT") => typeof(double),
                        string s when s.Contains("BLOB") => typeof(byte[]),
                        string s when s.Contains("BOOL") => typeof(bool),
                        _ => typeof(string)
                    };

                    // Check if it's autoincrement
                    bool isAutoIncrement = isPrimaryKey && sqlType.Contains("INT");

                    columns.Add(new TableColumnInfo
                    {
                        Name = columnName,
                        DataType = netType,
                        IsAutoIncrement = isAutoIncrement,
                        IsNotNull = notNull,
                        IsPrimaryKey = isPrimaryKey,
                        DefaultValue = defaultValue
                    });
                }
            }
            catch
            {
                // Return empty list on error
            }

            return columns;
        }

        public string GetTableSchema(string tableName)
        {
            if (connection == null || connection.State != ConnectionState.Open)
                return string.Empty;

            try
            {
                var schema = new System.Text.StringBuilder();
                schema.AppendLine($"Table: {tableName}");
                schema.AppendLine(new string('-', 60));

                using var command = connection.CreateCommand();
                command.CommandText = $"PRAGMA table_info('{tableName}')";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var columnName = reader.GetString(1);
                    var dataType = reader.GetString(2);
                    var notNull = reader.GetInt32(3) == 1;
                    var defaultValue = reader.IsDBNull(4) ? null : reader.GetValue(4).ToString();
                    var isPrimaryKey = reader.GetInt32(5) == 1;

                    schema.Append($"  {columnName} {dataType}");

                    if (isPrimaryKey)
                        schema.Append(" PRIMARY KEY");

                    if (notNull)
                        schema.Append(" NOT NULL");

                    if (!string.IsNullOrEmpty(defaultValue))
                        schema.Append($" DEFAULT {defaultValue}");

                    schema.AppendLine();
                }

                return schema.ToString();
            }
            catch (Exception ex)
            {
                return $"Error getting schema: {ex.Message}";
            }
        }

        public string GetViewSchema(string viewName)
        {
            if (connection == null || connection.State != ConnectionState.Open)
                return string.Empty;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT sql FROM sqlite_master WHERE type='view' AND name='{viewName}'";

                var sql = command.ExecuteScalar()?.ToString();
                if (!string.IsNullOrEmpty(sql))
                {
                    return $"View: {viewName}\n{new string('-', 60)}\n{sql}";
                }

                return $"View: {viewName}\n{new string('-', 60)}\nNo definition found.";
            }
            catch (Exception ex)
            {
                return $"Error getting view schema: {ex.Message}";
            }
        }

        public string GetIndexSchema(string indexName)
        {
            if (connection == null || connection.State != ConnectionState.Open)
                return string.Empty;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT sql FROM sqlite_master WHERE type='index' AND name='{indexName}'";

                var sql = command.ExecuteScalar()?.ToString();
                if (!string.IsNullOrEmpty(sql))
                {
                    return $"Index: {indexName}\n{new string('-', 60)}\n{sql}";
                }

                return $"Index: {indexName}\n{new string('-', 60)}\nAuto-generated index.";
            }
            catch (Exception ex)
            {
                return $"Error getting index schema: {ex.Message}";
            }
        }

        public DataTable? GetTableData(string tableName)
        {
            if (connection == null || connection.State != ConnectionState.Open)
                return null;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {tableName}";

                var dataTable = new DataTable(tableName);

                using var reader = command.ExecuteReader();

                // Create columns
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }

                // Add rows
                while (reader.Read())
                {
                    var row = dataTable.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                    }
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            catch
            {
                return null;
            }
        }

        public bool ExecuteNonQuery(string sql, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (connection == null || connection.State != ConnectionState.Open)
            {
                errorMessage = "Database is not open.";
                return false;
            }

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error executing query: {ex.Message}";
                return false;
            }
        }

        public DataSet ExecuteQuery(string sql, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (connection == null || connection.State != ConnectionState.Open)
            {
                errorMessage = "Database is not open.";
                return null;
            }

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = sql;

                using var reader = command.ExecuteReader();

                var dataSet = new DataSet();
                int resultIndex = 0;

                // Handle each result set (SQLite typically returns a single result set,
                // but we support multiple via NextResult())
                do
                {
                    resultIndex++;
                    var table = new DataTable($"Result{resultIndex}");

                    // If there are no columns (e.g., non-select), skip adding table
                    if (reader.FieldCount == 0)
                    {
                        continue;
                    }

                    // Create columns
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Type columnType;
                        try
                        {
                            columnType = reader.GetFieldType(i) ?? typeof(object);
                        }
                        catch
                        {
                            columnType = typeof(object);
                        }

                        table.Columns.Add(reader.GetName(i), columnType);
                    }

                    // Add rows
                    while (reader.Read())
                    {
                        var row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                        }
                        table.Rows.Add(row);
                    }

                    dataSet.Tables.Add(table);
                } while (reader.NextResult());

                return dataSet;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error executing query: {ex.Message}";
                return null;
            }
        }


        public SqliteConnection? GetConnection()
        {
            return connection;
        }

        public string GetTableSql(string tableName)
        {
            if (connection == null || connection.State != ConnectionState.Open)
                return string.Empty;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT sql FROM sqlite_master WHERE type='table' AND name='{tableName}'";

                var sql = command.ExecuteScalar()?.ToString();
                return sql ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetViewSql(string viewName)
        {
            if (connection == null || connection.State != ConnectionState.Open)
                return string.Empty;

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT sql FROM sqlite_master WHERE type='view' AND name='{viewName}'";

                var sql = command.ExecuteScalar()?.ToString();
                return sql ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void Dispose()
        {
            CloseDatabase();
        }
    }
}
