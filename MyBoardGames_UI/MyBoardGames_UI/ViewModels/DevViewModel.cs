using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.IO;
using System.Reflection;
using CsvHelper;
using System.Globalization;
using MyBoardGames_UI.Models;
using Android.Widget;

namespace MyBoardGames_UI.ViewModels
{
    public class DevViewModel : BaseViewModel
    {
        public Command AddAllGamesCommand { get; }
        public Command DeleteAllGamesCommand { get; }
        public Command DeleteGameTableCommand { get; }

        public DevViewModel()
        {
            AddAllGamesCommand = new Command(async () => await ExecuteAddAllGamesCommand());
            DeleteAllGamesCommand = new Command(async () => await ExecuteDeleteAllGamesCommand());
            DeleteGameTableCommand = new Command(async () => await ExecuteDeleteGameTableCommand());
        }

        private async Task ExecuteDeleteGameTableCommand()
        {
            await DataStore.GetResult().DropGameTableAsync();

            if (Device.RuntimePlatform == Device.Android)
                Toast.MakeText(Android.App.Application.Context, "Game Table Dropped", ToastLength.Short);
        }

        /// <summary>
        /// Deletes all of the games from the database
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteDeleteAllGamesCommand()
        {
            await DataStore.GetResult().DeleteAllGamesAsync();

            if (Device.RuntimePlatform == Device.Android)
                Toast.MakeText(Android.App.Application.Context, "All Games Deleted", ToastLength.Short).Show();
        }

        private async Task ExecuteAddAllGamesCommand()
        {
            var games = new List<Game>();

            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("MyBoardGames_UI.AllMyGames.csv"))
            using (StreamReader reader = new StreamReader(stream))
            {
                if (reader != null)
                {
                    using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                    {
                        //Read and discard the first row which should be the column header of the csv
                        csv.Read();

                        while (csv.Read())
                        {
                            games.Add(new Game
                            {
                                Name = csv.GetField<string>(0),
                                MinutesToPlay = csv.GetField<int>(1),
                                MinNumberOfPlayers = csv.GetField<int>(2),
                                MaxNumberOfPlayers = csv.GetField<int>(3)
                            });
                        }
                    }
                }
            }

            await DataStore.GetResult().AddAllGamesFromCSVAsync(games);

            if (Device.RuntimePlatform == Device.Android)
                Toast.MakeText(Android.App.Application.Context, "All Games Added", ToastLength.Short).Show();
        }
    }
}
