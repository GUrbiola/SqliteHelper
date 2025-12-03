using System.Text.Json;

namespace SqliteHelper
{
    public class RecentDatabases
    {
        private const int MaxRecentDatabases = 10;
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SqliteHelper",
            "recent_databases.json"
        );

        public List<RecentDatabaseInfo> Databases { get; set; } = new List<RecentDatabaseInfo>();

        public static RecentDatabases Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    var recent = JsonSerializer.Deserialize<RecentDatabases>(json);
                    if (recent != null)
                    {
                        // Filter out databases that no longer exist
                        recent.Databases = recent.Databases.Where(d => File.Exists(d.Path)).ToList();
                        return recent;
                    }
                }
            }
            catch
            {
                // If there's any error loading, just return empty list
            }

            return new RecentDatabases();
        }

        public void Save()
        {
            try
            {
                var directory = Path.GetDirectoryName(SettingsPath);
                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch
            {
                // Silently fail if we can't save
            }
        }

        public void AddDatabase(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);

            // Remove if already exists
            Databases.RemoveAll(d => d.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

            // Add to the beginning
            Databases.Insert(0, new RecentDatabaseInfo
            {
                Path = path,
                Name = fileName,
                LastOpened = DateTime.Now
            });

            // Keep only the most recent
            if (Databases.Count > MaxRecentDatabases)
            {
                Databases = Databases.Take(MaxRecentDatabases).ToList();
            }

            Save();
        }

        public void RemoveDatabase(string path)
        {
            Databases.RemoveAll(d => d.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
            Save();
        }
    }

    public class RecentDatabaseInfo
    {
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime LastOpened { get; set; }
    }
}
