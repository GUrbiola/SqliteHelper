using System;

namespace SqliteHelper48
{
    public class TableColumnInfo
    {
        public string Name { get; set; } = string.Empty;
        public Type DataType { get; set; } = typeof(string);
        public bool IsAutoIncrement { get; set; }
        public bool IsNotNull { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string? DefaultValue { get; set; }
    }
}
