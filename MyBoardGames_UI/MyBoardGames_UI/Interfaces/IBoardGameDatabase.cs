using MyBoardGames_UI.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardGames_UI.Services
{
    public interface IBoardGameDatabase<T>
    {
        Task<List<T>> GetAllGamesAsync();
        Task<T> GetGameAsync(int id);
        Task<T> GetGameAsync(Game game);
        Task<int> SaveGameAsync(T game);
        Task<int> DeleteGameAsync(int id);
        Task<int> DropGameTableAsync();
        Task<int> DeleteAllGamesAsync();
        Task<int> AddAllGamesFromCSVAsync(List<Game> games);
    }
}
