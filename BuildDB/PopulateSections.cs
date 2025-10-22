using JimKeith.BuildDB;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Windows.Media.Control;

namespace JimKeith.BuildDB
{
    public class PopulateSec
    {
        private readonly string sectionFile;

        public List<SectionRecord> Records { get; private set; } = new();

        public PopulateSec(string dataFile)
        {
            sectionFile = dataFile;
        }

        public void GetSectionData()
        {
            if (!File.Exists(sectionFile))
            {
                Console.WriteLine($"File not found: {sectionFile}");
                return;
            }
            else
            {
                Console.WriteLine($"Loading data from file: {sectionFile}");
            }

            try
            {
                foreach (var line in File.ReadLines(sectionFile))
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        Records.Add(new SectionRecord
                        {
                            SectionID = parts[0].Trim(),
                            Section = parts[1].Trim()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }

        public void InsertSectionData()
        {
             try
            {
                using var connection = new SQLiteConnection(Program.Dc.ConnString);
                Console.WriteLine($"\n\nDatabase connection string: {Program.Dc.ConnString}\n\n");
                connection.Open();

                foreach (var record in Records)
                {
                    Console.WriteLine($"Inserting record: SectionID={record.SectionID}, Section={record.Section}");
                    using var command = new SQLiteCommand(
                        "INSERT INTO Sections (SectionID, Section) VALUES (@SectionID, @Section)",
                        connection);
                    command.Parameters.AddWithValue("@SectionID", record.SectionID);
                    command.Parameters.AddWithValue("@Section", record.Section);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
        }
    }

    public class SectionRecord
    {
        public required string SectionID { get; set; }
        public required string Section { get; set; }
    }
}
