/* ***************************************************
 * Catalog 
 * Cloned from the LibraryApp and modified to be a 
 * system to store and catalog a user set of files
 * 
 * BuilDB Program builds a database to support the 
 * Catalog application
 * *************************************************/

using JimKeith.BuildDB;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Windows.Media.Capture;

namespace JimKeith.BuildDB;

class Program
{
    public static DataCollection Dc = new();

    static void Main()
    {
        Console.WriteLine("Welcome to the Library Database Setup!\n");
        Console.WriteLine("This program will help you create and new database for your Library.");

        // Prompt for user information
        Dc.UserLoginName = "";  // This will trigger the user login name prompt
        Dc.UserPassword = "";   // This will trigger the password prompt and hashing
        Dc.UserName = "";       // This will trigger the user name prompt
        Dc.UserAddress = "";    // Default role is Admin
        Dc.UserCity = "";       // Default city if not provided
        Dc.UserProvince = "";   // Default state if not provided
        Dc.UserCountry = "";    // Default country if not provided
        Dc.UserPostalCode = ""; // Default postal code if not provided
        Dc.UserPhone = "";      // Default phone if not provided
        Dc.UserEmail = "";      // This will trigger the email prompt

        // Prompt for Library information
        Dc.LibraryName = "";    // This will trigger the Library name prompt
        Dc.DbName = ""; // This will trigger the Library database name prompt

        if (AskUser("Is the Library Address the same as the Administrators? (y/n): "))
        {
            Dc.IsSameAddress = true;
        }
        else
        {
            Dc.IsSameAddress = false;
            Dc.LibraryAddress = "";  // This will trigger the Library address prompt
            Dc.LibraryCity = "";     // Default libray city if not provided
            Dc.LibraryProvince = ""; // Default Library province or state if not provided
            Dc.LibraryCountry = "";  // Default Library country if not provided
        }

        if (AskUser("Is the Library Phone number the same as the Administrators? (y/n): "))
        {
            Dc.IsSamePhone = true;
        }
        else
        {
            Dc.IsSamePhone = false;
            Dc.LibraryPhone = "";   // This will trigger the Library phone prompt
        }

        if (AskUser("Is the Library Email address the same as the Administrators? (y/n): "))
        {
            Dc.IsSameEmail = true;
        }
        else
        {
            Dc.IsSameEmail = false;
            Dc.LibraryEmail = "";   // This will trigger the Libary email prompt
        }

        // Display collected information for confirmation
        Console.WriteLine("\nPlease confirm the entered information:");
        Console.WriteLine($"User Login Name is {Dc.UserLoginName}");
        Console.WriteLine($"User Name is {Dc.UserName}");
        Console.WriteLine($"User Role is {Dc.UserRole}");
        Console.WriteLine($"User Street address is {Dc.UserAddress}");
        Console.WriteLine($"User City is {Dc.UserCity}");
        Console.WriteLine($"User Province/State is {Dc.UserProvince}");
        Console.WriteLine($"User Country is {Dc.UserCountry}");
        Console.WriteLine($"User Postal Code or Zip Code is {Dc.UserPostalCode}");
        Console.WriteLine($"User Email is {Dc.UserEmail}");

        Console.WriteLine($"\nLibrary Name is {Dc.LibraryName}");
        Console.WriteLine($"Library Database Name is {Dc.DbName}");
        Console.WriteLine($"Library Address is {Dc.LibraryAddress}");
        Console.WriteLine($"Library City is {Dc.LibraryCity}");
        Console.WriteLine($"Library Province/State is {Dc.LibraryProvince}");
        Console.WriteLine($"Library Country is {Dc.LibraryCountry}");
        Console.WriteLine($"Library Postal Code or Zip Code is {Dc.LibraryPostalCode}");
        Console.WriteLine($"Library Phone is {Dc.LibraryPhone}");
        Console.WriteLine($"Library Email is {Dc.LibraryEmail}\n");
        Console.WriteLine($"Database name will be: {Dc.DbName}\n");
        Console.WriteLine($"Database file will be created at: {Dc.DbPath}\n");
        Console.WriteLine($"Database Connection string is: {Dc.ConnString} \n");

        // --------  Create and/or Update Available Database file -----------
        var mgr = new DBInfoManager();
        mgr.Load();   // loads existing JSON (if any) and appends current DB (unless duplicate)
        mgr.Save();   // writes the file (now contains current DB)

        Console.WriteLine($"Loaded database entries from {mgr.GetJsonPath()}");
        // --------------------------------------------------------------------


        if (!AskUser(@"Is this OK to proceed (y/n): "))
        {
            Console.WriteLine("Program will terminate.");    
            Environment.Exit(0);
        }

        // string connectionString = Dc.ConnString;

        Console.WriteLine("It will create the tables and populate the reference tables with initial data.\n\n");

        Console.WriteLine($"The reference tables will created\n");
        MakeRefTbls mr = new ();
        mr.MakeTbls();

        Console.WriteLine($"The Main tables will created\n");
        MakeMainTbls mm = new ();
        mm.MakeTbls();

        Console.WriteLine($"Insert initial user and Library data\n");
        InsertInitialData iid = new ();
        iid.InsertData();

        Console.WriteLine($"The reference tables will be populated\n");
        string workingDir = AppContext.BaseDirectory;
        string dataPath = Path.Combine(workingDir, "data");
        Console.WriteLine($"Working Directory: {dataPath}");

        Console.WriteLine("Populating the reference tables...\n");

        // Populate the Sections tables
        string section_file = Path.Combine(dataPath, "sectionTable.csv");
        var pSec = new PopulateSec(section_file);
        pSec.GetSectionData();
        pSec.InsertSectionData();

        // Populate the Formats table
        string format_file = Path.Combine(dataPath, "formatTable.csv");
        var pFmt = new PopulateFormats(format_file);
        pFmt.GetFormatData();
        pFmt.InsertFormatData(); 

        // Populate the Genres table
        string genres_file = Path.Combine(dataPath, "genresTable.csv");
        var pGnre = new PopulateGenres(genres_file);
        pGnre.GetGenresData();
        pGnre.InsertGenreData();

        // Populate the Status table
        string status_file = Path.Combine(dataPath, "statusTable.csv");
        var pSts = new PopulateStatus(status_file);
        pSts.GetStatusData();
        pSts.InsertStatusData();

        // Populate the Conditions table
        string conditions_file = Path.Combine(dataPath, "conditionsTable.csv");
        var pCdtn = new PopulateConditions(conditions_file);
        pCdtn.GetConditionsData();
        pCdtn.InsertConditionsData();

        // Populate the Locations table
        string locations_file = Path.Combine(dataPath, "locationsTable.csv");
        var pLoc = new PopulateLocations(locations_file);
        pLoc.GetLocationsData();
        pLoc.InsertLocationsData();
    }


    static bool AskUser(string prompt)
    {
        Console.Write(prompt);
        var response = Console.ReadLine();
        return response?.Trim().ToLower() == "y";
    }

    public static class Globals
    {
         public static bool SUPPORTS_STRICT = true;
         public static int VERSION = 1;
         public static string COMPANY_NAME = "BaseLine";
         public static string APPLICATION_NAME = "HomeLibrary";
    }


    public class CheckSQLiteVersion
    {
        public bool IsStrict(SQLiteConnection conn)
        {
            using var versionCommand = new SQLiteCommand("SELECT sqlite_version();", conn);
            string version = versionCommand.ExecuteScalar()?.ToString() ?? "Unknown";
            Console.WriteLine($"SQLite version: {version}");
            // Parse version and check for STRICT table support (>= 3.37.0)
            Version minStrictVersion = new(3, 37, 0);
            bool supportsStrict = Version.TryParse(version, out var currentVersion) && currentVersion >= minStrictVersion;
            return supportsStrict;
        }
    }
}









