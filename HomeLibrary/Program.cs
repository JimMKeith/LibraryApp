using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;

namespace HomeLibrary
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new HomeForm());
        }
    }
}

    /*
    public static class GlobalConfig
    {
        public static List<DBMetaData> Databases { get; private set; } = new();

        public static void FetchMetaData()
        {
            string metaDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                           "BaseLine", "HomeLibrary");
            string metaPath = Path.Combine(metaDir, "databases.json");

            if (File.Exists(metaPath))
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var jsonString = File.ReadAllText(metaPath);
                System.Diagnostics.Debug.WriteLine(jsonString);

                try
                {
                    var list = System.Text.Json.JsonSerializer.Deserialize<List<DBMetaData>>(jsonString, options);
                    Databases = list ?? new List<DBMetaData>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"JSON parse error: {ex.Message}");
                    Databases = new List<DBMetaData>();
                    SaveMetaData(); // recreate file if corrupted
                }
            }
            else
            {
                SaveMetaData(); // create default file if missing
            }
        }

        public static void SaveMetaData()
        {
            string metaDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                          "BaseLine", "HomeLibrary");
            string metaPath = Path.Combine(metaDir, "databases.json");

            if (!Directory.Exists(metaDir))
                Directory.CreateDirectory(metaDir);

            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };

            var jsonString = System.Text.Json.JsonSerializer.Serialize(Databases, options);
            File.WriteAllText(metaPath, jsonString);
        }

        public class DBMetaData
        {
            public string? LibName { get; set; }
            public string? DbName { get; set; }
            public string? DbFilePath { get; set; }
            public int DbVersion { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("DbCreateDt")]
            public DateTime DbCreateDt { get; set; }

            public DBMetaData()
            {
                LibName = "My Home Library";
                DbName = "HomeLibrary.db";
                DbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DbName);
                DbVersion = 1;
                DbCreateDt = DateTime.Now;
            }
        }
    }
}
    */
