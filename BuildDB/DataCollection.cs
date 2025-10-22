using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WinRT; // Add this using directive at the top of the file

namespace JimKeith.BuildDB
{

    internal class DataCollection
    {
        // User table
        protected string userLoginName;
        protected string userPassword;
        protected string userName;
        protected string userRole { get; } = "Admin";
        protected string userAddress;
        protected string userCity;
        protected string userProvince;
        protected string userCountry;
        protected string userPostalCode;
        protected string userPhone;
        protected string userEmail;
        protected bool isSameAddress { get; set; }
        protected bool isSamePhone { get; set; }
        protected bool isSameEmail { get; set; }
        protected long userID { get; set; }
        protected long userAdminID { get; set; }
        //
        protected string libraryName;
        protected string libraryAddress;
        protected string libraryCity;
        protected string libraryProvince;
        protected string libraryCountry;
        protected string libraryPostalCode;
        protected string libraryPhone;
        protected string libraryEmail;
        protected long libraryID { get; set; }


        protected string dbName;
        protected string dbPath { get; set; }
        protected string connString { get; set; }

        public string UserLoginName
        {
            set
            {
                string uLoginName = "";
                Console.Write("Enter the Administrators Login ID (required): ");
                while (string.IsNullOrWhiteSpace(uLoginName))
                {
                    uLoginName = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(uLoginName))
                    {
                        if (QuitTheProgram("The Login ID is required ... "))
                        { Environment.Exit(0); }
                    }
                }
                userLoginName = uLoginName;
            }
            get
            {
                return userLoginName;
            }
        }

        // When setting the password, hash it using BCrypt
        public string UserPassword
        {
            set
            {
                string pw = "";
                Console.Write("Enter Administrators password (required): ");
                while (string.IsNullOrWhiteSpace(pw))
                {
                    pw = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(pw))
                    {
                        Console.WriteLine("Password is required");
                        if (QuitTheProgram("Password is requied "))
                        { Environment.Exit(0); }
                    }
                }
                // Hash the password using BCrypt
                userPassword = BCrypt.Net.BCrypt.HashPassword(pw);
            }
            get
            {
                return userPassword;
            }
        }

        public string UserName
        {
            set
            {
                string uName = "";
                Console.Write("Enter the name of the person who will be Administrating the new Library Branch (required): ");
                while (string.IsNullOrWhiteSpace(uName))
                {
                    uName = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(uName))
                    {
                        uName = "";
                    }
                    userName = uName;
                }
            }
            get
            {
                return userName;
            }
        }

        public string UserRole
        {
            get
            {
                return userRole;
            }
        }

        public string UserAddress
        {
            set
            {
                string uAddress = "";
                Console.Write("Enter the Administrators street Address (optional): ");
                uAddress = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(uAddress))
                {
                    uAddress = "";  // default address if blank
                }
                userAddress = uAddress;
            }
            get
            {
                return userAddress;
            }
        }

        public string UserCity
        {
            set
            {
                string uCity = "";
                Console.Write("Enter the Administrators City (optional): ");
                uCity = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(uCity))
                {
                    uCity = "";  // default city if blank
                }
                userCity = uCity;
            }
            get
            {
                return userCity;
            }
        }

        public string UserProvince
        {
            set
            {
                string uProvince = "";
                Console.Write("Enter the Administrators Province or State (optional): ");
                uProvince = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(uProvince))
                {
                    uProvince = "";  // default province or state if blank
                }
                userProvince = uProvince;
            }
            get
            {
                return userProvince;
            }
        }

        public string UserCountry
        {
            set
            {
                string uCountry = "";
                Console.Write("Enter the Administrators Country (optional): ");
                uCountry = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(uCountry))
                {
                    uCountry = "";  // default country if blank
                }
                userCountry = uCountry;
            }
            get
            {
                return userCountry;
            }
        }


        public string UserPostalCode
        {
            set
            {
                string uPostalCode = "";
                Console.Write("Enter the Administrators Postal Code or Zip Code (optional): ");
                uPostalCode = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(uPostalCode))
                {
                    uPostalCode = "";  // default city if blank
                }
                userPostalCode = uPostalCode;
            }
            get
            {
                return userPostalCode;
            }
        }


        public string UserPhone
        {
            set
            {
                string uPhone = "";
                Console.Write("Enter the Administrators Phone number (optional): ");
                uPhone = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(uPhone))
                {
                    uPhone = "";  // default phone if blank
                }
                userPhone = uPhone;
            }
            get
            {
                return userPhone;
            }
        }


        public string UserEmail
        {
            set
            {
                string uEmail = "";
                Console.Write("Enter a Administrators Email address (required): ");
                while (string.IsNullOrWhiteSpace(uEmail))
                {
                    uEmail = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(uEmail))
                    {
                        uEmail = "";
                    }
                    userEmail = uEmail;
                }
            }
            get
            {
                return userEmail;
            }
        }


        // Administrators User is created when the database is created
        // save the Admin userID
        public long UserAdminID
        {
            get
            {
                return userAdminID;
            }
            set
            {
                userAdminID = value;
            }
        }

        // The Library record is created when the database is created
        // save the libraryID
        public long LibraryID
        {
            get
            {
                return libraryID;
            }
            set
            {
                libraryID = value;
            }
        }


        public string LibraryName
        {
            set
            {
                string lName = "";
                Console.Write("Enter a name for your Library (required): ");
                while (string.IsNullOrWhiteSpace(lName))
                {
                    lName = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(lName))
                    {
                        lName = "";
                    }
                    libraryName = lName;
                }
            }
            get
            {
                return libraryName;
            }
        }

        // Set the name of the database to support the Library
        // Validate the name and
        // set the database path (dbPath) and connetction string (connString)
        public string DbName
        {
            set
            {
                string libDBName = "";
                while (string.IsNullOrWhiteSpace(libDBName))
                {
                    Console.Write("Enter a name for database that will store the books, Name (required): ");
                    libDBName = Console.ReadLine() ?? "";
                    libDBName = (libDBName ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(libDBName))
                    {
                        if (QuitTheProgram("The Library database Name is required ... "))
                        { Environment.Exit(0); }
                    }
                    else
                    { //  Validate the name 
                        // Ensure the database name has a .db suffix
                        if (!libDBName.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
                        {
                            libDBName += ".db";
                        }

                        string curentFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string fullPathToDatabase = Path.Combine(curentFolder, Program.Globals.COMPANY_NAME, Program.Globals.APPLICATION_NAME, libDBName);
                        Console.WriteLine($"The database will be created at: {fullPathToDatabase}");
                        if (File.Exists(fullPathToDatabase))
                        {
                            Console.WriteLine($"The database file {fullPathToDatabase} already exists. Try another name or quit ");
                            libDBName = "";
                            if (QuitTheProgram("A unique Library database Name is required ... "))
                            {
                                Environment.Exit(0);
                            }
                            dbName = libDBName;
                        }

                        if (!string.IsNullOrWhiteSpace(libDBName))
                        {
                            // initialize global variables to define the database path and the connection string
                            // Create a directory for the database if it one doesn't exist already
                            try { 
                                Directory.CreateDirectory(Path.GetDirectoryName(fullPathToDatabase));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error creating directory for database: {ex.Message}");
                                if (QuitTheProgram("Unable to create directory for database. Do you want to quit? "))
                                { Environment.Exit(0); }
                            }
                            dbName = libDBName;
                            dbPath = fullPathToDatabase;
                            connString = $"Data Source= {dbPath} ";
                        }  // end initialize
                    } // end validate the name
                } // end while libDBName is blank
            }  // end set
            get
            {
                return dbName;
            }  // end get
        }

        public string DbPath
        {
            get
            {
                return dbPath;
            }

            set
            {
                dbPath = value;
            }   
        }

        public string ConnString
        {
            get
            {
                return connString;
            }
            set
            {
                connString = value;
            }
        }

        public string LibraryAddress
        {
            set
            {
                string lAddress = "";
                Console.Write("Enter a Library street address (optional): ");
                lAddress = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(lAddress))
                {
                    lAddress = "";  // default to blank
                }
                libraryAddress = lAddress;
            }
            get
            {
                return libraryAddress;
            }
        }

        public string LibraryCity
        {
            set
            {
                string lCity = "";
                Console.Write("Enter the Library\'s city (optional): ");
                lCity = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(lCity))
                {
                    lCity = "";  // default to blank
                }
                libraryCity = lCity;
            }
            get
            {
                return libraryCity;
            }
        }


        public string LibraryProvince
        {
            set
            {
                string lProvince = "";
                Console.Write("Enter Libraries Province or State (optional): ");
                lProvince = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(lProvince))
                {
                    lProvince = "";  // default to blank
                }
                libraryProvince = lProvince;
            }
            get
            {
                return libraryProvince;
            }
        }

        public string LibraryCountry
        {
            set
            {
                string lCountry = "";
                Console.Write("Enter Libraries Country (optional): ");
                lCountry = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(lCountry))
                {
                    lCountry = "";  // default to blank
                }
                libraryCountry = lCountry;
            }
            get
            {
                return libraryCountry;
            }
        }


        public string LibraryPostalCode
        {
            set
            {
                string lPostalCode = "";
                Console.Write("Enter Libraries Post Code or Zip Code (optional): ");
                lPostalCode = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(lPostalCode))
                {
                    lPostalCode = "";  // default to blank
                }
                libraryPostalCode = lPostalCode;
            }
            get
            {
                return libraryPostalCode;
            }
        }

        public string LibraryPhone
        {
            set
            {
                string lPhone = "";
                Console.Write("Enter the Library Phone number (optional): ");
                
                lPhone = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(lPhone))
                {
                    lPhone = ""; // default to blank
                }
                libraryPhone = lPhone;
            }
            get
            {
                return libraryPhone;
            }
        }
        public string LibraryEmail
        {
            set
            {
                string lEmail = "";
                Console.Write("Enter the Library\'s Email address: ");
                {
                    lEmail = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(lEmail))
                    {
                        lEmail = ""; // default to blank
                    }
                    libraryEmail = lEmail;
                }
            }
            get
            {
                return libraryEmail;
            }
        }

        public bool IsSameAddress
        {
            set
            {
                isSameAddress = value;

                if (isSameAddress)
                {
                    libraryAddress = userAddress;
                    libraryCity = userCity;
                    libraryProvince = userProvince;
                    libraryCountry = userCountry;
                    libraryPostalCode = userPostalCode;
                }
            }
            get
            {
                return isSameAddress;
            }
        }

        public bool IsSamePhone
        {
            set
            {
                isSamePhone = value;

                if (isSamePhone)
                {
                    libraryPhone = userPhone;
                }
            }
            get
            {
                return isSamePhone;
            }
        }

        public bool IsSameEmail
        {
            set
            {
                isSameEmail = value;

                if (isSameEmail)
                {
                    libraryEmail = userEmail;
                }
            }
            get
            {
                return isSameEmail;
            }
        }


        private static bool QuitTheProgram(string prompt)
        {
            string ans = "";
            bool quitProg = true;

            while (string.IsNullOrWhiteSpace(ans))
            {
                Console.WriteLine(prompt);
                Console.WriteLine("Do you want to quit the program? (y/n): ");
                ans = Console.ReadLine();
                ans = ans?.Trim().ToLower();
                if (ans == "y")
                {
                    quitProg = true;
                }
                else if (ans == "n")
                {
                    quitProg = false;
                }
                else
                {   
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'... ");
                    ans = "";
                }
            }
            return quitProg;  // quit if true else continue
        }
    }
}
