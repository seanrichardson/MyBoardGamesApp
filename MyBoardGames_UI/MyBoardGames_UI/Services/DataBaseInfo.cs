using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyBoardGames_UI.Services
{
    public class DataBaseInfo
    {
        public const string databaseFileName = "BoardGamesSQLite.db3";

        public const SQLite.SQLiteOpenFlags flags =
            //open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            //create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            //enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, databaseFileName);
            }
        }
    }
}
