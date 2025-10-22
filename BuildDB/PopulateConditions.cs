using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace JimKeith.BuildDB;

public class PopulateConditions
{
    private readonly string conditionsFile;

    public List<ConditionsRecord> Records { get; private set; } = [];
    public PopulateConditions(string dataFile) => conditionsFile = dataFile;


    public void GetConditionsData()
    {
        Console.WriteLine($"Looking for file at: {conditionsFile}");

        if (!File.Exists(conditionsFile))
        {
            Console.WriteLine($"File not found: {conditionsFile}");
            return;
        }
        else
        {
            Console.WriteLine($"Loading data from file: {conditionsFile}");
        }

        try
        {
            foreach (var line in File.ReadLines(conditionsFile))
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    Records.Add(new ConditionsRecord()
                    {
                        conditionID = parts[0].Trim(),
                        condition   = parts[1].Trim()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file {ex.Message}");
        }
    }



    public void InsertConditionsData()
    {
        try
        {
            using var connection = new SQLiteConnection(Program.Dc.ConnString);
            connection.Open();

            foreach (var record in Records)
            {
                Console.WriteLine($"Inserting record: conditionID={record.conditionID}, condition={record.condition}");

                // Fix: Parse conditionID to int
                if (!int.TryParse(record.conditionID, out int cID))
                {
                    Console.WriteLine($"Invalid conditionID: {record.conditionID}");
                    continue;
                }
                using var command = new SQLiteCommand("INSERT INTO Conditions (conditionID, condition) VALUES (@conditionID, @condition)", connection);
                command.Parameters.AddWithValue("@conditionID", cID);
                command.Parameters.AddWithValue("@condition", record.condition);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }
}

public class ConditionsRecord
{
    public required string conditionID { get; set; }
    public required string condition { get; set; }
}


