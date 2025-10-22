using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using JimKeith.BuildDB;

namespace JimKeith.BuildDB;
            
public class PopulateFormats
{
    private readonly string formatFile;
    public List<FormatRecord> Records { get; private set; } = [];

    //  constuctor
    public PopulateFormats(string dataFile)
    {
            formatFile = dataFile;
    }

    public void GetFormatData()
    {

        if (!File.Exists(formatFile))
        {
            Console.WriteLine($"File not found: {formatFile}");
            return;
        }
        else
        {
            Console.WriteLine($"Loading data from file: {formatFile}");
        }

        try
        {
            foreach (var line in File.ReadLines(formatFile))
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    Records.Add(new FormatRecord
                    {
                        FormatID = parts[0].Trim(),
                        Format = parts[1].Trim()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading {ex.Message}");
        }
    }

    public void InsertFormatData()
    {
        var dc = new DataCollection();
        try
        {
            using var connection = new SQLiteConnection(Program.Dc.ConnString);
            connection.Open();

            foreach (var record in Records)
            {
                Console.WriteLine($"Inserting record: SectionID={record.FormatID}, Section={record.Format}");
                using var command = new SQLiteCommand("INSERT INTO Formats (FormatID, Format) VALUES (@FormatID, @Format)", connection);
                command.Parameters.AddWithValue("@FormatID", record.FormatID);
                command.Parameters.AddWithValue("@Format", record.Format);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }

    public class FormatRecord
    {
        public required string FormatID { get; set; }
        public required string Format { get; set; }
    }
}

