using System.Text.Json;

namespace SqliteHelper
{
    public class DatabaseProject
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public List<TableDefinition> Tables { get; set; } = new List<TableDefinition>();
        public List<Relationship> Relationships { get; set; } = new List<Relationship>();
        public List<DiagramComment> Comments { get; set; } = new List<DiagramComment>();

        public void SaveToFile(string path)
        {
            LastModifiedDate = DateTime.Now;
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static DatabaseProject LoadFromFile(string path)
        {
            var json = File.ReadAllText(path);
            var project = JsonSerializer.Deserialize<DatabaseProject>(json);
            return project ?? throw new Exception("Failed to load project file.");
        }
    }

    public class TableDefinition
    {
        public string Name { get; set; } = string.Empty;
        public List<ColumnDefinition> Columns { get; set; } = new List<ColumnDefinition>();
        public Point Position { get; set; }
    }

    public class ColumnDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = "TEXT";
        public bool IsPrimaryKey { get; set; }
        public bool IsNotNull { get; set; }
        public bool IsAutoIncrement { get; set; }
        public string? DefaultValue { get; set; }
    }

    public class Relationship
    {
        public string FromTable { get; set; } = string.Empty;
        public string FromColumn { get; set; } = string.Empty;
        public string ToTable { get; set; } = string.Empty;
        public string ToColumn { get; set; } = string.Empty;
        public string RelationshipType { get; set; } = "One-to-Many";
    }

    public class DiagramComment
    {
        public string Text { get; set; } = string.Empty;
        public Point Position { get; set; }
        public Size Size { get; set; } = new Size(200, 100);
    }
}
