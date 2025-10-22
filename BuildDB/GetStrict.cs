using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JimKeith.BuildDB;

public class StrictCheck
{
    public bool GetStrict(SQLiteConnection conn)
    {
        using var versionCommand = new SQLiteCommand("SELECT sqlite_version();", conn);
        string version = versionCommand.ExecuteScalar()?.ToString() ?? "Unknown";
        Console.WriteLine($"SQLite version: {version}");

        // Parse version and check for STRICT table support (>= 3.37.0)
        Version minStrictVersion = new Version(3, 37, 0);
        bool supportsStrict = Version.TryParse(version, out var currentVersion) && currentVersion >= minStrictVersion;

        return supportsStrict;
    }
}

