using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace JimKeith.BuildDB;

    public class PopulateGenres
    {
        private readonly string genresFile;

        public List<GenresRecord> Records { get; private set; } = [];

        public PopulateGenres(string dataFile)
        {
            genresFile = dataFile;
        }

        public void GetGenresData()
        {
            if (!File.Exists(genresFile))
            {
                Console.WriteLine($"File not found: {genresFile}");
                return;
            }
            else
            {
                Console.WriteLine($"Loading data from file: {genresFile}");
            }

            try
            {
                foreach (var line in File.ReadLines(genresFile))
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        Records.Add(new GenresRecord
                        {
                            Fiction = parts[0].Trim(),
                            GenreID = parts[1].Trim(),
                            GenreText = parts[2].Trim()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
                    
        public void InsertGenreData()
        {
        try
            {
                using var connection = new SQLiteConnection(Program.Dc.ConnString);
                connection.Open();

                foreach (var record in Records)
                {
                    Console.WriteLine($"Inserting record: Fiction={record.Fiction}, GenreID={record.GenreID}, GenreText={record.GenreText}");
                    using var command = new SQLiteCommand("INSERT INTO Genres (GenreID, Fiction, GenreText) VALUES (@GenreID, @Fiction, @GenreText)", connection);
                    command.Parameters.AddWithValue("@GenreID", record.GenreID);
                    command.Parameters.AddWithValue("@Fiction", record.Fiction);
                    command.Parameters.AddWithValue("@GenreText", record.GenreText);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
        }
    }

    public class GenresRecord
    {
        public required string Fiction { get; set; }
        public required string GenreID { get; set; }
        public required string GenreText { get; set; }
    }


