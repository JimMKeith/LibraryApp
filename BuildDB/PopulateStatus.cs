using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace JimKeith.BuildDB;

public class PopulateStatus
{
    private readonly string statusFile;

    public List<StatusRecord> Records { get; private set; } = [];

    public PopulateStatus(string dataFile)
    {
        statusFile = dataFile;
    }

    public void GetStatusData()
    {
        if (!File.Exists(statusFile))
        {
            Console.WriteLine($"File not found: {statusFile}");
            return;
        }
        else
        {
            Console.WriteLine($"Loading data from file: {statusFile}");
        }

        try
        {
            foreach (var line in File.ReadLines(statusFile))
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    Records.Add(new StatusRecord
                    {
                        statusID = parts[0].Trim(),
                        statusText = parts[1].Trim()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file {ex.Message}");
        }
    }

    public void InsertStatusData()
    { 
        try
        {
            using var connection = new SQLiteConnection(Program.Dc.ConnString);
            connection.Open();

            foreach (var record in Records)
            {
                Console.WriteLine($"Inserting record: statusID={record.statusID}, statusID{record.statusText}");
                using var command = new SQLiteCommand("INSERT INTO Status (statusID, statusText) VALUES (@statusID, @statusText)", connection);
                command.Parameters.AddWithValue("@statusID", record.statusID);
                command.Parameters.AddWithValue("@statusText", record.statusText);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }
}

public class StatusRecord
{
    public required string statusID { get; set; }
    public required string statusText { get; set; }
}



