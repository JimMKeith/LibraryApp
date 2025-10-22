using JimKeith.BuildDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JimKeith.BuildDB.Program;

namespace JimKeith
{
    internal class InsertInitialData
    {
        private readonly string connString;
        public InsertInitialData()
        {
            connString = Dc.ConnString;
        }

        public void InsertData()
        {
            Console.WriteLine("Insert Initial Data ");
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

            InsertLibrary(conn);
            InsertUser(conn);
        }
        // =====================================================
        //    Insert Users into the Users Table 
        public void InsertUser(SQLiteConnection conn)
        {
            string insertUserSql = @"
                INSERT OR IGNORE INTO Users (loginName, userName, address, city, province, country, postalCode, phoneNumber, email, userRole, libraryID, isActive, passwordHash)
                VALUES (@loginName, @uName, @uAddress, @uCity, @uProvince, @uCountry, @uPostalCode, @uPhone, @uEmail, @uRole, @libID, @active, @passHash);";
            Console.WriteLine($"{insertUserSql}");

            using var command = new SQLiteCommand(insertUserSql, conn);
            // insert the Admin User
            command.Parameters.AddWithValue("@loginName", Dc.UserLoginName);
            command.Parameters.AddWithValue("@uName", Dc.UserName);
            command.Parameters.AddWithValue("@uAddress", Dc.UserAddress);
            command.Parameters.AddWithValue("@uCity", Dc.UserCity);
            command.Parameters.AddWithValue("@uProvince", Dc.UserProvince);
            command.Parameters.AddWithValue("@uCountry", Dc.UserCountry);
            command.Parameters.AddWithValue("@uPostalCode", Dc.UserPostalCode);
            command.Parameters.AddWithValue("@uPhone", Dc.UserPhone);
            command.Parameters.AddWithValue("@uEmail", Dc.UserEmail);
            command.Parameters.AddWithValue("@uRole", Dc.UserRole);
            command.Parameters.AddWithValue("@libID", Dc.LibraryID);
            command.Parameters.AddWithValue("@active", 1);
            command.Parameters.AddWithValue("@passHash", Dc.UserPassword);
 
            Console.WriteLine($"Inserting Admin user: {Dc.UserName}, Role: {Dc.UserRole}, Email: {Dc.UserEmail}");

            Dc.UserAdminID = -1; // Default value if insertion fails or user exists
            try
            {
                Console.WriteLine("Inserting a Admin user into Users table...");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Rows affected: {rowsAffected}");
                if (rowsAffected > 0)
                {
                    Dc.UserAdminID = conn.LastInsertRowId;  // Save Admin users ID
                    Console.WriteLine("Admin user inserted successfully.");
                }
                else
                {
                    Console.WriteLine("Admin user already exists. No new record inserted.");
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error inserting Admin User: {ex.Message}");
            }

            // insert the Guest User
            command.Parameters.AddWithValue("@loginName", "Guest");
            command.Parameters.AddWithValue("@uName", "Guest");
            command.Parameters.AddWithValue("@uAddress", "");
            command.Parameters.AddWithValue("@uCity", "");
            command.Parameters.AddWithValue("@uProvince", "");
            command.Parameters.AddWithValue("@uCountry", "");
            command.Parameters.AddWithValue("@uPostalCode", "");
            command.Parameters.AddWithValue("@uPhone", "");
            command.Parameters.AddWithValue("@uEmail", "");
            command.Parameters.AddWithValue("@uRole", "Guest");
            command.Parameters.AddWithValue("@libID", Dc.LibraryID);
            command.Parameters.AddWithValue("@active", 1);
            command.Parameters.AddWithValue("@passHash", "");

            Console.WriteLine($"Inserting Guest user: Guest, Guest, Email: (none)");

            try
            {
                Console.WriteLine("Inserting a Guest user into Users table...");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Rows affected: {rowsAffected}");
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Guest user inserted successfully.");
                }
                else
                {
                    Console.WriteLine("Guest user user already exists. No new record inserted.");
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error inserting Guest User: {ex.Message}");
            }
        }


        public void InsertLibrary(SQLiteConnection conn)
        {
            Console.WriteLine("Inserting a Library into Libraries table...");
            Console.WriteLine($"{Dc.LibraryName}");

            string insertLibSql = @"
        INSERT OR IGNORE INTO Libraries (libraryName, address, city, province, country, postalCode, phoneNumber, email, dataBaseName, version)
        VALUES (@libName, @libAddress, @libCity, @libProvince,  @libCountry, @libPostalCode, @LibPhone, @libEmail, @dataBaseName, @version);";

            Console.WriteLine($"{insertLibSql}\n");

            using var command = new SQLiteCommand(insertLibSql, conn);
            command.Parameters.AddWithValue("@libName",  Dc.LibraryName);
            command.Parameters.AddWithValue("@libAddress", Dc.LibraryAddress);
            command.Parameters.AddWithValue("@libCity", Dc.LibraryCity);
            command.Parameters.AddWithValue("@libProvince", Dc.LibraryProvince);
            command.Parameters.AddWithValue("@libCountry", Dc.LibraryCountry);
            command.Parameters.AddWithValue("@libPostalCode", Dc.LibraryPostalCode);
            command.Parameters.AddWithValue("@LibPhone", Dc.LibraryPhone);
            command.Parameters.AddWithValue("@libEmail", Dc.LibraryEmail);
            command.Parameters.AddWithValue("@dataBaseName", Dc.DbName);
            command.Parameters.AddWithValue("@version", Globals.VERSION);

            Dc.LibraryID = -1; // Default value if insertion fails or Library exists
            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Dc.LibraryID = conn.LastInsertRowId;
                    Console.WriteLine("Library inserted successfully.\n");
                }
                else
                {
                    // No rows affected means the library already exists due to INSERT OR IGNORE
                    Console.WriteLine("Library already exists. No new record inserted.\n");
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error inserting Library: {ex.Message}");
            }
        }

    }
}