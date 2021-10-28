using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBoardGames_UI.Models
{
    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinutesToPlay { get; set; }
        public int MinNumberOfPlayers { get; set; }
        public int MaxNumberOfPlayers { get; set; }
        public GenreTypes Genre { get; set; }
    }
}
