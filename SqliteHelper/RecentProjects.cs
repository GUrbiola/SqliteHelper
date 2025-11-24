using System.Text.Json;

namespace SqliteHelper
{
    public class RecentProjects
    {
        private const int MaxRecentProjects = 10;
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SqliteHelper",
            "recent_projects.json"
        );

        public List<RecentProjectInfo> Projects { get; set; } = new List<RecentProjectInfo>();

        public static RecentProjects Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    var recent = JsonSerializer.Deserialize<RecentProjects>(json);
                    if (recent != null)
                    {
                        // Filter out projects that no longer exist
                        recent.Projects = recent.Projects.Where(p => File.Exists(p.Path)).ToList();
                        return recent;
                    }
                }
            }
            catch
            {
                // If there's any error loading, just return empty list
            }

            return new RecentProjects();
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

        public void AddProject(string path, string name)
        {
            // Remove if already exists
            Projects.RemoveAll(p => p.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

            // Add to the beginning
            Projects.Insert(0, new RecentProjectInfo
            {
                Path = path,
                Name = name,
                LastOpened = DateTime.Now
            });

            // Keep only the most recent
            if (Projects.Count > MaxRecentProjects)
            {
                Projects = Projects.Take(MaxRecentProjects).ToList();
            }

            Save();
        }

        public void RemoveProject(string path)
        {
            Projects.RemoveAll(p => p.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
            Save();
        }
    }

    public class RecentProjectInfo
    {
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime LastOpened { get; set; }
    }
}
