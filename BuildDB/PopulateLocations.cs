using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace JimKeith.BuildDB;

public class PopulateLocations
{
    private readonly string locationsFile;

    public List<LocationsRecord> Records { get; private set; } = [];

    public PopulateLocations(string dataFile)
    {
        locationsFile = dataFile;
    }

    public void GetLocationsData()
    {
        if (!File.Exists(locationsFile))
        {
            Console.WriteLine($"File not found: {locationsFile}");
            return;
        }
        else
        {
            Console.WriteLine($"Loading data from file: {locationsFile}");
        }

        try
        {
            foreach (var line in File.ReadLines(locationsFile))
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    Records.Add(new LocationsRecord
                    {
                        LocationID = parts[0].Trim(),
                        Location = parts[1].Trim()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file{ex.Message}");
        }
    }

    public void InsertLocationsData()
    {
        try
        {
            using var connection = new SQLiteConnection(Program.Dc.ConnString);
            connection.Open();

            foreach (var record in Records)
            {
                Console.WriteLine($"Inserting record: LocationID={record.LocationID}, Location={record.Location}");
                using var command = new SQLiteCommand("INSERT INTO Locations (LocationID, Location) VALUES (@LocationID, @Location)", connection);
                command.Parameters.AddWithValue("@LocationID", record.LocationID);
                command.Parameters.AddWithValue("@Location", record.Location);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }
}

public class LocationsRecord
{
    public required string LocationID { get; set; }
    public required string Location { get; set; }
}



