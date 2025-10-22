BuildDB Application Instructions

This application will run as a windows console program. It will create and initialize
a database to support the Library application.

See the file "BuildDB ERD.pdf" for the ER Diagram.

Select BuildDB.exe file and double click it to run the program.

The BuildDB program will prompt for some basic information
⦁	User Login Name - The account name the Admin user can use to log into the Library application and manage the applications
⦁	User Password - The password the Admin user will use to login 
⦁	User Name - The Admin users name
⦁	User address - the street address of the Admin user. 
⦁	User City - the city the Admin lives in
⦁	User Province - The province or state the Admin lives in
⦁	User country - The country the Admin lives in
⦁	User Post Code - The Post code or Zip code to identify the Admins mailing address
⦁	User Phone - The Admins phone number
⦁	User Email - The Admins email address
⦁	Library Name  - The nbame given to the Library
⦁	DB Name - Names the database file that will be created. Only the file name is required. 
    All database files are stored in a pre-determined path (or catalog stucture)
    The path is 
        C:/Users/<user>/AppData/Roaming/Libraries/
    where <user> is the windows user id.
⦁	The address of the actual Library can be the same as the Admin users.
    This is normally the case for a Home Library 
    A question will be asked "Is the Library address the same as the Users addresss?"
    If the answer is yes (y) then the User Admnin adsdress information will be copied
    to the Libraries address
⦁	The same question is asked for the Libraries phone number and again for
    the Libraries Email address.
    Answer yes (y) will copy the appropiate User Admin info to the Libraries info.
⦁	Answer no (n) to these question and you will be given an opportunity to enter
    relevent data for the Library address, phone and email.
⦁	Note most of the information collected is optional.
    Simply leave the response blank. 
    If the information is required it will continue to prompt you until it is satisfied.

Note: Personal information, Name, address, phone etc. is not shared. 
It identifies the Library and the users.The personal information is not validated 
and therefor does not have to be anything but decriptive. For example if you do not
have an email address you could enter "No Email", the database would be happy with that.

After all information is collected you are prompted to proceed or quit.
If you choose to proceed the database tables will be built. 
Some of the tables will be populated with supporting data such as
allowable book formats – Hard Cover, Paperback, or supported book genres, etc.

No books are entered at this time. You will enter books using the Library application.

Two users accounts will be created, an Administrators account and a Guest account.
The Guest account has no password and is used to allow users to browse the database.
No password is requied to access via the Guest account. 
Guest are not allowed any updates. They cannot add new books or make book reviews.

Administrator account is created with the user name and password you entered.
An Administrator has total access to all functions. They can add books, write reviews,
change the various supporting data elements such as book formats, Genres etc.

A person may create an account using the Library application. They will become a "Member".
A Member can and write reviews. They cannot change the supporting data elements or add books.

Passwords are stored in the database in an encrypted form. The encryption is one way.
If you forget or loose your password there is no recourse. The password in its encrypted
form is not possible to reconstruct. Passwords cannot be recoverd from their encrypted
form.

At this time there is no method available to change a password, (a future project).
The database must be given back to the developer so a new password can be encrypted and 
installed in the database. The database and new password would then be given back to you. 

Alternativly you can use BuildDB.exe to create a new database and then reload your books.
Library members and book reviews would all be lost in this case.

The database is a single SQLite file suffixed with “.db”. In a windows system
the path to database file is 

    C:/Users/<user>/AppData/Roaming/Libraries/<name.db>

    where <user> is the windows user id.
    and   <name.db> is the name of the database file. 
        Name is the name you assigned when prompted for a database name by the
        BuildDB.exe program. The BuilDB.exe program prints the full path to 
        the database when it runs.

SQLite does not support a DROP DATABASE command.
To get rid of the database simply delete the SQLite database file.
Note the BuildDB.exe program will only create a uniqly named database.
If a database file exists you must either select a new name or delete 
the existing one before you can successfuly run BuildDB.exe using that Name again.

There is a set of “reference” tables in the database. These tables define the
various attributes a book is allowed to take on when it is entered. The existing
attributes fall into the catagories
    • Sections – (4 char code, 50 char description)
        This represents the various section in a Library such as where the fiction 
        books are kept together, or where the Reference books, dictionaries, encyclopedias
        etc. are kept. 
    • Locations – (2 char code, 50 char description)
        Identifies where in the Library the books are, 2 floor, basement, west wing, etc.
    • Status - (4 char code, 50 char description)
        The current status of the book, on loan, Sold, Deleted, Ok (available to read)
    • Condition – (integer code, 50 character description)
        The integer range from 0 to 4 with descriptions from bad to excelent.
        The condition refers to the physical condition of the book.
    • Format - (3 char code, 50 char description)
        The format of the document, hardback, paperback, pamphlet, etc.
    •  Genres – (10 character code, Fiction or NonFiction column, Genre text)
        Describes the various genres a book may fall into.
        Includes a column containg a code identifing the specific genre,
        a column to identify the genre as fiction or non fiction,
        and a column for the full description of the genre.
        Books can be assigned multiple genres.

The various reference tables, Section, Location, Status, etc., are populated when
the database is created by the BuildDB.exe program.
A set of .csv files (comma separated values) contain the information to populate these
files. The files are available under the directory “data”. The "data" directory 
will be in the same deployment folder BuildDB.exe is in. 

They "data" .csv files can be edited before you run the BuildDB.exe program to take on
values that will be relevant to your Library.

- Follow the format in the supplied in the .csv files.
- Do not introduce any stray commas. The commas define where the fields start and stop.
- Do not exceed the character length stated in the list above reference table descriptions.
  Incorrect formatting may result in database tables that fail to populate properly.

If the tables do not all load properly fix any errors in the .csv files. 
Delete the newly created database file and rerun the BuildDB.exe program.

Once the database has been created you will be able to install the Library application,
login as an Administrator, and manage the reference tables through the Library application.

Alternate database files can be created with the BuildDB.exe program. This will allow
you to manage multiple libraries using Library application.

To backup the database simple copy the database file to backup location.
To restore a database copy the backup file back to the database original location.