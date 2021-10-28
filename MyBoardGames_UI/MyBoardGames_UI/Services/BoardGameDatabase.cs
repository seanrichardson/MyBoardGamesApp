using MyBoardGames_UI.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardGames_UI.Services
{
    public class BoardGameDatabase : IBoardGameDatabase<Game>
    {
        static SQLiteAsyncConnection Database;

        //This instantiates the database lazily
        public static readonly AsyncLazy<BoardGameDatabase> Instance = new AsyncLazy<BoardGameDatabase>(async () =>
        {
            var instance = new BoardGameDatabase();
            CreateTableResult result = await Database.CreateTableAsync<Game>();
            return instance;

        });

        public BoardGameDatabase()
        {
            Database = new SQLiteAsyncConnection(DataBaseInfo.DatabasePath, DataBaseInfo.flags);
        }

        ////This instatiates the database using the Singleton pattern
        //static BoardGameDatabase instance = null;

        ////Makes the constructor private
        //private BoardGameDatabase()
        //{
        //    //Database = new SQLiteAsyncConnection(DataBaseInfo.DatabasePath, DataBaseInfo.flags);
        //}

        //public static BoardGameDatabase Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new BoardGameDatabase();
        //        }

        //        return instance;
        //    }
        //}

        public Task<List<Game>> GetAllGamesAsync()
        {
            return Database.Table<Game>().ToListAsync();
        }

        //public Task<List<Game>> GetAllGamesNotAsync()
        //{
        //    //SQL queries are also possible
        //    return Database.QueryAsync<Game>("SELECT * FROM [Game] WHERE [Done] = 0");
        //}

        public Task<Game> GetGameAsync(int id)
        {
            return Database.Table<Game>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<Game> GetGameAsync(Game game)
        {
            return Database.Table<Game>().Where(i => i.Name.Equals(game.Name) &&
                i.MinutesToPlay == game.MinutesToPlay &&
                i.MinNumberOfPlayers == game.MinNumberOfPlayers &&
                i.MaxNumberOfPlayers == game.MaxNumberOfPlayers).FirstOrDefaultAsync();
        }

        public Task<int> SaveGameAsync(Game game)
        {
            if (game.Id != 0)
            {
                //Updates the item in the database
                return Database.UpdateAsync(game);
            }
            else
            {
                //Inserts a new item into the database
                return Database.InsertAsync(game);
            }
        }

        public Task<int> DeleteGameAsync(int id)
        {
            return Database.DeleteAsync(GetGameAsync(id).Result);
        }
    }
}
