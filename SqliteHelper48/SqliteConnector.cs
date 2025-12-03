using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace SqliteHelper48
{
    public partial class SqliteConnector
    {
        #region Variables
        private DateTime _startTime, _endingTime;
        SqliteCommand cmd = null;
        #endregion

        #region Properties
        public int RowsAffected { get; set; }
        public int RowsRead { get; set; }
        public string LastMessage { get; set; }

        public SqliteConnection Connection { get; set; }
        public SqliteConnection NewConnection
        {
            get
            {
                return new SqliteConnection(ConnectionString);
            }
        }
        public string ConnectionString
        {
            get { return Connection.ConnectionString; }
            set { Connection.ConnectionString = value; }
        }
        public TimeSpan ExecutionLapse
        {
            get
            {
                return _endingTime.Subtract(_startTime);
            }
        }
        public bool OnExecution { get; private set; }
        public bool Executing
        {
            get
            {
                return OnExecution;
            }
            set
            {
                if (value)
                {
                    _startTime = DateTime.Now;
                    OnExecution = true;
                }
                else
                {
                    _endingTime = DateTime.Now;
                    OnExecution = false;
                }
            }
        }
        public int TimeOut { get; set; }
        public bool Error
        {
            get
            {
                return LastMessage != "OK";
            }
        }
        public Exception? LastException { get; set; }
        public SqliteException? LastSqliteException { get; set; }
        public string Server
        {
            get
            {
                if (Connection != null)
                    return Connection.DataSource;
                return "";
            }
        }
        public string DataBase
        {
            get
            {
                if (Connection != null)
                    return Connection.Database;
                return "";
            }
        }
        #endregion

        #region Constructors
        public SqliteConnector()
        {
            TimeOut = 0;
            Connection = new SqliteConnection();

            LastMessage = "OK";
            cmd = new SqliteCommand();
        }
        public SqliteConnector(string connectionString)
        {
            TimeOut = 0;
            Connection = new SqliteConnection(connectionString);

            LastMessage = "OK";
            cmd = new SqliteCommand();
        }
        public SqliteConnector(SqliteConnection connection)
        {
            TimeOut = 0;
            Connection = (connection == null ? new SqliteConnection() : connection);

            LastMessage = "OK";
            cmd = new SqliteCommand();
        }
        #endregion

        public bool TestConnection()
        {
            try
            {
                Connection.Open();
                LastMessage = "OK";
            }
            catch (Exception ex)
            {
                LastMessage = ex.Message;
                LastException = ex;
                return false;
            }
            finally
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                    Connection.Close();
            }
            LastMessage = "OK";
            return true;
        }

        public int ExecuteNonQuery(string sql)
        {
            int result = 0;
            try
            {
                
                cmd.Connection = Connection;
                cmd.CommandText = sql;
                cmd.CommandTimeout = TimeOut;
                Executing = true;
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;

                RowsAffected = cmd.ExecuteNonQuery();
                LastMessage = "OK";
            }
            catch (SqliteException sqlex)
            {
                LastMessage = sqlex.Message;
                LastSqliteException = sqlex;
                result = -1;
            }
            catch (Exception ex)
            {
                LastMessage = ex.Message;
                LastException = ex;
                result = -1;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                }
                Executing = false;
            }
            return result;
        }
        public object ExecuteScalar(string sql)
        {
            object result = null;
            try
            {
                cmd.Connection = Connection;
                cmd.CommandText = sql;
                cmd.CommandTimeout = TimeOut;
                Executing = true;
                cmd.Connection.Open();

                cmd.CommandType = CommandType.Text;
                RowsRead = 0;
                result = cmd.ExecuteScalar();
                RowsRead++;
                LastMessage = "OK";
            }
            catch (SqliteException sqlex)
            {
                LastMessage = sqlex.Message;
                LastSqliteException = sqlex;
                result = null;
            }
            catch (Exception ex)
            {
                LastMessage = ex.Message;
                LastException = ex;
                result = null;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                }
                Executing = false;
            }
            return result;
        }
        public DataTable ExecuteTable(string sql, string tableName = "")
        {
            DataTable aux = null;
            try
            {
                cmd.Connection = Connection;
                cmd.CommandText = sql;
                cmd.CommandTimeout = TimeOut;
                Executing = true;
                cmd.Connection.Open();

                cmd.CommandType = CommandType.Text;
                RowsRead = 0;

                using (SqliteDataReader rdr = cmd.ExecuteReader())
                {
                    // Prepare table name
                    aux = new DataTable(string.IsNullOrEmpty(tableName) ? "data" : tableName);

                    // Get schema even if there are no rows (GetColumnSchema works independently)
                    var schema = rdr.GetColumnSchema();

                    // Create columns with mapped types
                    int colIndex = 0;
                    foreach (DbColumn column in schema)
                    {
                        string colName = column.ColumnName ?? ("Column" + colIndex.ToString());
                        Type colType = column.DataType ?? MapSqliteTypeNameToClr(column.DataTypeName);

                        DataColumn newColumn = new DataColumn(colName, colType ?? typeof(object));
                        if (column.AllowDBNull.HasValue)
                            newColumn.AllowDBNull = column.AllowDBNull.Value;
                        aux.Columns.Add(newColumn);
                        colIndex++;
                    }

                    // Read rows
                    while (rdr.Read())
                    {
                        var row = aux.NewRow();
                        for (int i = 0; i < aux.Columns.Count; i++)
                        {
                            if (rdr.IsDBNull(i))
                            {
                                row[i] = DBNull.Value;
                                continue;
                            }

                            object raw = rdr.GetValue(i);
                            Type targetType = aux.Columns[i].DataType;

                            // If raw already matches target type, assign directly
                            if (raw != null && targetType.IsInstanceOfType(raw))
                            {
                                row[i] = raw;
                                continue;
                            }

                            // Special handling for booleans stored as integers in SQLite
                            try
                            {
                                if (targetType == typeof(bool))
                                {
                                    // common SQLite integer boolean representation
                                    if (raw is long l) row[i] = (l != 0);
                                    else if (raw is int ii) row[i] = (ii != 0);
                                    else if (raw is byte b) row[i] = (b != 0);
                                    else row[i] = Convert.ToBoolean(raw);
                                }
                                else if (targetType == typeof(int))
                                {
                                    if (raw is long l) row[i] = Convert.ToInt32(l);
                                    else row[i] = Convert.ToInt32(raw);
                                }
                                else if (targetType == typeof(long))
                                {
                                    if (raw is int ii) row[i] = Convert.ToInt64(ii);
                                    else row[i] = Convert.ToInt64(raw);
                                }
                                else if (targetType == typeof(double))
                                {
                                    row[i] = Convert.ToDouble(raw);
                                }
                                else if (targetType == typeof(decimal))
                                {
                                    row[i] = Convert.ToDecimal(raw);
                                }
                                else if (targetType == typeof(DateTime))
                                {
                                    // SQLite may store dates as text or numeric; try conversion
                                    if (raw is string s) row[i] = DateTime.Parse(s);
                                    else row[i] = Convert.ToDateTime(raw);
                                }
                                else if (targetType == typeof(byte[]))
                                {
                                    // blobs
                                    if (raw is byte[] bytes) row[i] = bytes;
                                    else row[i] = raw;
                                }
                                else if (targetType == typeof(string))
                                {
                                    row[i] = Convert.ToString(raw);
                                }
                                else
                                {
                                    // Fallback: try ChangeType, otherwise assign raw
                                    try
                                    {
                                        row[i] = Convert.ChangeType(raw, targetType);
                                    }
                                    catch
                                    {
                                        row[i] = raw;
                                    }
                                }
                            }
                            catch
                            {
                                // On any conversion issue, fall back to raw value
                                row[i] = raw;
                            }
                        }

                        aux.Rows.Add(row);
                        RowsRead++;
                    }

                    LastMessage = "OK";
                }
            }
            catch (SqliteException sqlex)
            {
                LastMessage = sqlex.Message;
                LastSqliteException = sqlex;
                aux = null;
            }
            catch (Exception ex)
            {
                LastMessage = ex.Message;
                LastException = ex;
                aux = null;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                }
                Executing = false;
            }

            return aux;
        }
        public List<string> ExecuteColumn(string sql, bool autoTransact = false)
        {
            List<string> back = new List<string>();
            DataTable aux;
            aux = ExecuteTable(sql, "Temptable");
            if (!Error && aux != null && aux.Rows.Count > 0)
            {
                int rc = aux.Rows.Count;
                for (int i = 0; i < rc; i++)
                {
                    var value = aux.Rows[i][0];
                    back.Add(value?.ToString() ?? string.Empty);
                }
            }
            return back;
        }
        // Helper to map SQLite declared type names to CLR types
        private static Type MapSqliteTypeNameToClr(string? typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return typeof(object);

            string t = typeName.Trim().ToUpperInvariant();

            // SQLite type affinities; map common declared types to CLR counterparts
            if (t.Contains("INT")) // INTEGER, INT, TINYINT, etc.
                return typeof(long);
            if (t.Contains("CHAR") || t.Contains("CLOB") || t.Contains("TEXT") || t.Contains("VARCHAR"))
                return typeof(string);
            if (t.Contains("BLOB"))
                return typeof(byte[]);
            if (t.Contains("REAL") || t.Contains("FLOA") || t.Contains("DOUB"))
                return typeof(double);
            if (t.Contains("NUMERIC") || t.Contains("DECIMAL"))
                return typeof(decimal);

            if (t.Contains("DATE") || t.Contains("TIME"))
                return typeof(DateTime);
            if (t.Contains("BOOL"))
                return typeof(bool);

            // default fallback
            return typeof(object);
        }
    }
}