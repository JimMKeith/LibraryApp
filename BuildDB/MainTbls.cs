using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using static JimKeith.BuildDB.Program;


namespace JimKeith.BuildDB;

internal class MakeMainTbls
{
    private readonly string connString;
    private readonly DataCollection dc; 

    public MakeMainTbls()
    {
        connString = Program.Dc.ConnString;
        dc = Program.Dc; // Fix: Ensure 'dc' is initialized to a non-null value
    }
    public void MakeTbls()
    {
        Console.WriteLine("Creating the main tables for the Library ");
        Console.WriteLine($"Using connection string: {connString}");

        using var conn = new SQLiteConnection(connString);

        conn.Open();
        CheckSQLiteVersion chkStrict = new CheckSQLiteVersion();
        if (!chkStrict.IsStrict(conn))
        {
            Console.WriteLine("The SQLite version is not valid. Exiting program.");
            Environment.Exit(0);
        }
        Console.WriteLine("SQLite version is valid./n");
        CreateLibraries(conn);
        CreateLibrariesTrigger(conn);

        CreateUsers(conn);
        CreateUsersTrigger(conn);

        CreateBooks(conn);
        CreateBooksTrigger(conn);

        CreateReviews(conn);
        CreateBookGenre(conn);

        CreateUserReadHistory(conn);
        CreateUserReadHistoryTrigger(conn);
    }


    // =====================================================
    //    Create Libraries Table
    public static void CreateLibraries(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the Libraries table...");
        string createTableSql = @"
                CREATE TABLE IF NOT EXISTS Libraries (
                    libraryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    libraryName TEXT NOT NULL,
                    address TEXT,
                    city TEXT,
                    province TEXT,
                    country TEXT,
                    postalCode TEXT,    
                    phoneNumber TEXT,
                    email TEXT,
                    dataBaseName TEXT NOT NULL UNIQUE,
                    version INTEGER NOT NULL,
                    createdAt TEXT DEFAULT CURRENT_TIMESTAMP,
                    updatedAt TEXT
                ) STRICT;";
        Console.WriteLine(createTableSql);
        using var command = new SQLiteCommand(createTableSql, conn);
        Console.WriteLine("Executing SQL to create Libraries table...");
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Libraries table created successfully.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Libraries table: {ex.Message}");
        }
    }

    // =====================================================
    //    Create Libraries Trigger 
    public static void CreateLibrariesTrigger(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the trigger for Libraries table...");
        string createTriggerSql = @"
                CREATE TRIGGER IF NOT EXISTS trg_UpdateLibraryTimestamp
                AFTER UPDATE ON Libraries
                FOR EACH ROW
                WHEN (OLD.libraryName != NEW.libraryName OR
                      OLD.address != NEW.address OR
                      OLD.city != NEW.city OR
                      OLD.province != NEW.province OR
                      OLD.country != NEW.country OR
                      OLD.postalCode != NEW.postalCode OR
                      OLD.phoneNumber != NEW.phoneNumber OR
                      OLD.email != NEW.email OR
                      OLD.version != NEW.version OR  
                      OLD.dataBaseName != NEW.dataBaseName)
                BEGIN
                    UPDATE Libraries SET UpdatedAt = CURRENT_TIMESTAMP WHERE libraryID = OLD.libraryID;
                END;";
        Console.WriteLine(createTriggerSql);
        using var command = new SQLiteCommand(createTriggerSql, conn);
        Console.WriteLine("Executing SQL to create trigger on Libraries table update...");
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Trigger on Libraries table update created successfully.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Trigger on Libraries table update: {ex.Message}");
        }
    }

    // =====================================================
    //    Create Users Table 
    public static void CreateUsers(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the Users table...");
        string createTableSql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    userID INTEGER PRIMARY KEY AUTOINCREMENT,
                    loginName TEXT NOT NULL UNIQUE,
                    userName TEXT NOT NULL,
                    address TEXT,
                    city TEXT,
                    province TEXT,
                    country TEXT,
                    postalCode TEXT, 
                    phoneNumber TEXT,
                    email TEXT NOT NULL UNIQUE,
                    userRole TEXT DEFAULT 'Member' CHECK(userRole IN ('Admin', 'Member', 'Guest')),
                    libraryID INTEGER, 
                    createdAt TEXT DEFAULT CURRENT_TIMESTAMP,
                    lastLogin TEXT,
                    isActive INTEGER DEFAULT 1 CHECK(isActive IN (0, 1)), 
                    passwordHash TEXT NOT NULL,
                    FOREIGN KEY (libraryID) REFERENCES Libraries(libraryID)
                ) STRICT;";
        Console.WriteLine(createTableSql);
        using var command = new SQLiteCommand(createTableSql, conn);
        Console.WriteLine("Executing SQL to create Users table...");
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Users table created successfully.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Users table: {ex.Message}");
        }
    }

    // =====================================================
    //    Create Users Trigger 
    public static void CreateUsersTrigger(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the trigger for Users table...");
        string createTriggerSql = @"
                CREATE TRIGGER IF NOT EXISTS trg_UpdateUsersTimestamp
                AFTER UPDATE ON Users
                FOR EACH ROW
                WHEN (OLD.loginName != NEW.loginName OR
                      OLD.userName != NEW.userName OR
                      OLD.address != NEW.address OR
                      OLD.city != NEW.city OR
                      OLD.province != NEW.province OR
                      OLD.country != NEW.country OR
                      OLD.postalCode != NEW.postalCode OR
                      OLD.phoneNumber != NEW.phoneNumber OR
                      OLD.email != NEW.email OR
                      OLD.userRole != NEW.userRole OR
                      OLD.libraryID != NEW.libraryID OR
                      OLD.isActive != NEW.isActive OR
                      OLD.passwordHash != NEW.passwordHash)
                BEGIN
                    UPDATE Users SET UpdatedAt = CURRENT_TIMESTAMP WHERE usersID = OLD.usersID;
                END;";
        Console.WriteLine(createTriggerSql);
        using var command = new SQLiteCommand(createTriggerSql, conn);
        Console.WriteLine("Executing SQL to create trigger on Libraries table update...");
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Trigger on Users table update created successfully.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Trigger on Users table update: {ex.Message}");
        }
    }

    // =====================================================
    //    Create Books Table 
    public static void CreateBooks(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the Books table...");
        string createTableSql = @"
                CREATE TABLE IF NOT EXISTS Books (
                    bookID INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    author TEXT NOT NULL,
                    publishedYear INTEGER,
                    edition TEXT,
                    condition INT NOT NULL DEFAULT 2,
                    genre     TEXT NOT NULL CHECK(length(genre) <= 10),
                    format    TEXT NOT NULL CHECK(length(format) <= 3),
                    location  TEXT NOT NULL CHECK(length(location) <= 3),
                    section   TEXT NOT NULL CHECK(length(section) <= 3),
                    status    TEXT NOT NULL DEFAULT 'OK' CHECK(length(status) <= 4),
                    createdAt TEXT DEFAULT CURRENT_TIMESTAMP,
                    updatedAt TEXT,
                    FOREIGN KEY (genre) REFERENCES BookGenre(genreCode)
                ) STRICT;";

        Console.WriteLine(createTableSql);
        using var tblCommand = new SQLiteCommand(createTableSql, conn);
        Console.WriteLine("Executing SQL to create Books table...");
        try
        {
            tblCommand.ExecuteNonQuery();
            Console.WriteLine("Books table created successfully.\n");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Books table: {ex.Message}");
        }
    }


    // =====================================================
    //    Create Books Trigger
    public static void CreateBooksTrigger(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the trigger for Books table...");
        string createTriggerSql = @"
            CREATE TRIGGER IF NOT EXISTS trg_UpdateBookTimestamp
            AFTER UPDATE ON Books
            FOR EACH ROW
            WHEN (OLD.title != NEW.title OR
                  OLD.author != NEW.author OR
                  OLD.publishedYear != NEW.publishedYear OR
                  OLD.edition != NEW.edition OR
                  OLD.condition != NEW.condition OR
                  OLD.genre != NEW.genre OR
                  OLD.format != NEW.format OR
                  OLD.location != NEW.location OR
                  OLD.section != NEW.section OR
                  OLD.status != NEW.status)
             BEGIN
                  UPDATE Books SET UpdatedAt = CURRENT_TIMESTAMP WHERE bookID = OLD.bookID;
             END;";

        Console.WriteLine(createTriggerSql);
        using var trigCommand = new SQLiteCommand(createTriggerSql, conn);
        Console.WriteLine("Executing SQL to create trigger to update UpdatedAt field...");
        try
        {
            trigCommand.ExecuteNonQuery();
            Console.WriteLine("Trigger on Books table update created successfully.\n");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Trigger on Books table update: {ex.Message}");
        }
    }

    // =====================================================
    //    Create Book Reviews Table
    public static void CreateReviews(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the Reviews table...");
        string createTableSql = @"
                CREATE TABLE IF NOT EXISTS Reviews (
                    reviewID INTEGER PRIMARY KEY AUTOINCREMENT,
                    bookID INTEGER NOT NULL,
                    userID INTEGER NOT NULL,
                    rating INTEGER CHECK(Rating >= 1 AND Rating <= 5),
                    comment TEXT,
                    createdAt TEXT DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (bookID) REFERENCES Books(bookID),
                    FOREIGN KEY (userID) REFERENCES Users(userID)
                ) STRICT;";
        Console.WriteLine(createTableSql);
        using var command = new SQLiteCommand(createTableSql, conn);
        Console.WriteLine("Executing SQL to create Reviews table...");
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Reviews table created successfully.\n");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Reviews table: {ex.Message}");
        }
    }


    // =====================================================
    //    Create BookGenre Table
    public static void CreateBookGenre(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the BookGenre table...");
        string createTableSql = @"
                CREATE TABLE IF NOT EXISTS BookGenre (
                    genreID  TEXT NOT NULL CHECK(length(genreID) <= 10),
                    bookID Integer NOT NULL,
                    PRIMARY KEY (genreID, bookID),
                    FOREIGN KEY (genreID) REFERENCES Genres(genreID),
                    FOREIGN KEY (bookID) REFERENCES Books(bookID)
                ) STRICT;";

        Console.WriteLine(createTableSql);
        using var command = new SQLiteCommand(createTableSql, conn);
        Console.WriteLine("Executing SQL to create BookGenre table...");
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("BookGenre table created successfully.\n");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating BookGenre table: {ex.Message}");
        }
    }


    // =====================================================
    //    Create UserReadHistory Table
    public static void CreateUserReadHistory(SQLiteConnection conn)
    {
        Console.WriteLine("Creating the UserReadHistory table...");
        string createTableSql = @"
                CREATE TABLE IF NOT EXISTS UserReadHistory (
                    userID INTEGER NOT NULL,
                    bookID INTEGER NOT NULL,
                    isRead INTEGER CHECK(isRead IN (0, 1)) DEFAULT 0,
                    lastRead TEXT DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY (userID, bookID),
                    FOREIGN KEY (userID) REFERENCES Users(userID),
                    FOREIGN KEY (bookID) REFERENCES Books(bookID)
                ) STRICT;";

        string createTrigOnReadHisSql = @"
                CREATE TRIGGER IF NOT EXISTS trg_UpdateUserReadHistory
                AFTER UPDATE ON UserReadHistory
                FOR EACH ROW
                WHEN (OLD.isRead != NEW.isRead)
                BEGIN
                    UPDATE UserReadHistory SET lastRead = CURRENT_TIMESTAMP WHERE userID = OLD.userID AND bookID = OLD.bookID;
                END;";

        Console.WriteLine(createTableSql);
        Console.WriteLine(createTrigOnReadHisSql);
        using var command = new SQLiteCommand(createTableSql, conn);
        Console.WriteLine("Executing SQL to create UserReadHistory table...");
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("UserReadHistory table created successfully.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating UserReadHistory table: {ex.Message}");
        }

        using var trigCommand = new SQLiteCommand(createTrigOnReadHisSql, conn);
        Console.WriteLine("Executing SQL to create trigger to update LastRead field...");
        try
        {
            trigCommand.ExecuteNonQuery();
            Console.WriteLine("Trigger on UserReadHistory table update created successfully.\n");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Trigger on UserReadHistory table update: {ex.Message}");
        }
    }

    // =====================================================
    //    Create UserReadHistory Trigger
    public static void CreateUserReadHistoryTrigger(SQLiteConnection conn)
    { string createTrigOnReadHisSql = @"
            CREATE TRIGGER IF NOT EXISTS trg_UpdateUserReadHistory
            AFTER UPDATE ON UserReadHistory
            FOR EACH ROW
            WHEN (OLD.isRead != NEW.isRead)
            BEGIN
                UPDATE UserReadHistory SET lastRead = CURRENT_TIMESTAMP WHERE userID = OLD.userID AND bookID = OLD.bookID;
            END;";

        Console.WriteLine(createTrigOnReadHisSql);
        using var trigCommand = new SQLiteCommand(createTrigOnReadHisSql, conn);
        Console.WriteLine("Executing SQL to create trigger to update LastRead field...");
        try
        {
            trigCommand.ExecuteNonQuery();
            Console.WriteLine("Trigger on UserReadHistory table update created successfully.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error creating Trigger on UserReadHistory table update: {ex.Message}");
        }
    }


   
}// <-- MakeMainTbls class
