using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Text.Json;

namespace JimKeith.BuildDB
{

    public class DBInfoManager
    {
        private readonly string _configDir;
        private readonly string _jsonPath;
        private List<oldDBInfo> _dbInfoList = new ();



        public DBInfoManager()
        {
            _configDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Program.Globals.COMPANY_NAME, Program.Globals.APPLICATION_NAME);

            Directory.CreateDirectory(_configDir);
            _jsonPath = Path.Combine(_configDir, "databases.json");
        }

        // Load existing file (if any) and ensure current DB info is present.
        public List<oldDBInfo> Load()
        {
            var list = new List<oldDBInfo>();

            if (File.Exists(_jsonPath))
            {
                string jsonData = File.ReadAllText(_jsonPath);
                Console.WriteLine($"Loaded JSON data: {jsonData}");
                list = JsonSerializer.Deserialize<List<oldDBInfo>>(jsonData) ?? new();
            }

            // Build current entry from Program.Dc (make sure Program.Dc is initialized before calling this)
            var cur = new newDBInfo();
            var curOld = new oldDBInfo
            {
                LibName = cur.LibName,
                DbName = cur.DbName,
                DbFilePath = cur.DbFilePath,
                DbVersion = cur.DbVersion,
                DbCreateDt = cur.DbCreateDt
            };

            // Avoid duplicate entries by DbFilePath
            bool exists = list.Exists(x => string.Equals(x.DbFilePath, curOld.DbFilePath, StringComparison.OrdinalIgnoreCase));
            if (!exists)
                list.Add(curOld);

            _dbInfoList = list;
            return list;
        }

        public string GetJsonPath() => _jsonPath;

        // Save list of DBInfo objects to JSON
        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_dbInfoList, options);
            File.WriteAllText(_jsonPath, json);
        }

        // Optional helper if you want to add current DB without loading first
        public void AddCurrentDbIfMissing()
        {
            var cur = new newDBInfo();
            var curOld = new oldDBInfo
            {
                LibName = cur.LibName,
                DbName = cur.DbName,
                DbFilePath = cur.DbFilePath,
                DbVersion = cur.DbVersion,
                DbCreateDt = cur.DbCreateDt
            };

            if (!_dbInfoList.Exists(x => string.Equals(x.DbFilePath, curOld.DbFilePath, StringComparison.OrdinalIgnoreCase)))
                _dbInfoList.Add(curOld);
        }
    }


    public class newDBInfo
    {
        public string LibName { get; set; }
        public string DbName { get; set; }
        public string DbFilePath { get; set; }
        public int DbVersion { get; set; }
        public DateTime DbCreateDt { get; set; }

        public newDBInfo()
        {
            LibName = Program.Dc.LibraryName;
            DbName = Program.Dc.DbName;
            DbFilePath = Program.Dc.DbPath;
            DbVersion = Program.Globals.VERSION;
            DbCreateDt = DateTime.Now;
        }
    }

    public class oldDBInfo
    {
        public string LibName { get; set; } = "";
        public string DbName { get; set; } = "";
        public string DbFilePath { get; set; } = "";
        public int DbVersion { get; set; } = 0;
        public DateTime DbCreateDt { get; set; } = DateTime.MinValue;
    }
}

