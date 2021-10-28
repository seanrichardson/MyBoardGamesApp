using MyBoardGames_UI.Models;
using MyBoardGames_UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyBoardGames_UI.ViewModels
{
    public class AllBoardGamesViewModel : BaseViewModel
    {
        private Game _selectedGame;

        public ObservableCollection<Game> Games { get; }
        public Command LoadGamesCommand { get; }
        public Command<Game> ItemTapped { get; }
        public Command AddGameCommand { get; }

        public AllBoardGamesViewModel()
        {
            Games = new ObservableCollection<Game>();
            LoadGamesCommand = new Command(async () => await ExecuteLoadGamesCommand());
            ItemTapped = new Command<Game>(OnGameSelected);
            AddGameCommand = new Command(OnAddGame);
        }

        public Game SelectedGame
        {
            get => _selectedGame;
            set
            {
                SetProperty(ref _selectedGame, value);
                OnGameSelected(value);
            }
        }

        private async void OnAddGame(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewGamePage));
        }

        async void OnGameSelected(Game game)
        {
            if (game == null)
                return;

            //Pushes the GameDetailPage onto the naviagation stack
            await Shell.Current.GoToAsync($"{nameof(GameDetailPage)}?{nameof(GameDetailViewModel.GameId)}={game.Id}");
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedGame = null;
        }

        private async Task ExecuteLoadGamesCommand()
        {
            IsBusy = true;

            try
            {
                Games.Clear();
                var games = await DataStore.GetResult().GetAllGamesAsync();
                foreach(var game in games)
                {
                    Games.Add(game);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
