using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JimKeith.BuildDB.Program;

namespace JimKeith.BuildDB;
    
    internal class MakeRefTbls
    {
        public MakeRefTbls()
        {  
        }

        public void MakeTbls()
            {
            Console.WriteLine("\nCreating the Library tables used to reference book genres, formats, conditions, status, locations, Library sections, etc...");

            Console.WriteLine($"\nUsing connection string: {Program.Dc.ConnString}");

            var conn = new SQLiteConnection(Program.Dc.ConnString);

            conn.Open();

            CheckSQLiteVersion chkStrict = new CheckSQLiteVersion();
            if (!chkStrict.IsStrict(conn))
            {
                Console.WriteLine("The SQLite version is not valid. Exiting program.");
                Environment.Exit(0);
            }
            Console.WriteLine("SQLite version is valid.");
            CreateSections(conn);
            CreateFormats(conn);
            CreateLocations(conn);
            CreateConditions(conn);
            CreateStatus(conn);
            CreateGenres(conn);

        }

        // Library sections (fiction, non fiction, history, etc.)
        public static void CreateSections(SQLiteConnection conn)
        {
            Console.WriteLine("Creating the Sections table...");

            string createTableSql = @"
                CREATE TABLE IF NOT EXISTS Sections (
                    sectionID TEXT PRIMARY KEY CHECK(length(sectionID) <= 4),
                    section   TEXT CHECK(length(section) <= 50)
                ) STRICT;";
            Console.WriteLine(createTableSql);

            using var command = new SQLiteCommand(createTableSql, conn);
            Console.WriteLine("Executing SQL to create Sections table...");
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Sections table created successfully.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error creating Sections table: {ex.Message}");
            }

        }
        // Book formats (paperback, hard cover, etc.)
        public static void CreateFormats(SQLiteConnection conn)
        {

            Console.WriteLine("Creating the Formats table...");

            string createTableSql = @"
            CREATE TABLE IF NOT EXISTS Formats (
                formatID TEXT PRIMARY KEY CHECK(length(formatID) <= 3),
                format  TEXT CHECK(length(Format) <= 50)
            ) STRICT;";
            Console.WriteLine(createTableSql);

            using var command = new SQLiteCommand(createTableSql, conn);
            Console.WriteLine("Executing SQL to create Formats table...");
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Formats table created successfully.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error creating Formats table: {ex.Message}");
            }


        }
        // Book locations (basement, study, etc...)
        // This is where the book is physically located in the Library.
        public static void CreateLocations(SQLiteConnection conn)
        {
            Console.WriteLine("Creating the Locations table...");

            string createTableSql = @"
            CREATE TABLE IF NOT EXISTS Locations (
                locationID TEXT PRIMARY KEY CHECK(length(locationID) <= 2),
                location   TEXT CHECK(length(location) <= 50)
            ) STRICT;";
            Console.WriteLine(createTableSql);

            using var command = new SQLiteCommand(createTableSql, conn);
            Console.WriteLine("Executing SQL to create Locations table...");
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Locations table created successfully.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error creating Locations table: {ex.Message}");
            }
        }
        // Condition of the book (0 - bad ... 4 - Excelent)
        public static void CreateConditions(SQLiteConnection conn)
        {
            Console.WriteLine("Creating the Conditions table...");

            string createTableSql = @"
            CREATE TABLE IF NOT EXISTS Conditions (
                conditionID INT  PRIMARY KEY,
                condition   TEXT CHECK(length(condition) <= 50)
            ) STRICT;";
            Console.WriteLine(createTableSql);

            using var command = new SQLiteCommand(createTableSql, conn);
            Console.WriteLine("Executing SQL to create Conditions table...");
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Conditions table created successfully.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error creating Conditions table: {ex.Message}");
            }

        }
        // Status of the book (Ok, deleted, out on loan, etc)
        public static void CreateStatus(SQLiteConnection conn)
        {
            Console.WriteLine("Creating the Status table...");

            string createTableSql = @"
            CREATE TABLE IF NOT EXISTS Status (
                statusID   TEXT PRIMARY KEY CHECK(length(statusID) <= 4),
                statusText TEXT CHECK(length(statusText) <= 50)
            ) STRICT;";
            Console.WriteLine(createTableSql);

            using var command = new SQLiteCommand(createTableSql, conn);
            Console.WriteLine("Executing SQL to create Status table...");
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Status table created successfully.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error creating Status table: {ex.Message}");
            }
        }

        // Genres of the book (Sci-Fi, Fantasy, etc.)
        public static void CreateGenres(SQLiteConnection conn)
        { 
            Console.WriteLine("Creating the Genres table...");

            string createTableSql = @"
            CREATE TABLE IF NOT EXISTS Genres (
                genreID   TEXT NOT NULL PRIMARY KEY CHECK(length(genreID) <= 10),
                fiction   TEXT NOT NULL CHECK(Fiction IN ('Fiction', 'NonFiction')),
                genreText TEXT
            ) STRICT;";
            Console.WriteLine(createTableSql);

            using var command = new SQLiteCommand(createTableSql, conn);
            Console.WriteLine("Executing SQL to create Genres table...");
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Genres table created successfully.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error creating Genres table: {ex.Message}");
            }

        }
    }

