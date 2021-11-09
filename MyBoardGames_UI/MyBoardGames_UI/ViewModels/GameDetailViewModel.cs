using MyBoardGames_UI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyBoardGames_UI.ViewModels
{
    [QueryProperty(nameof(GameId), nameof(GameId))]
    public class GameDetailViewModel : BaseViewModel
    {
        private int _gameId;
        private string _name;
        private string _minToPlay;
        private string _minNumPlayers;
        private string _maxNumPlayers;

        public GameDetailViewModel()
        {
            CancelCommand = new Command(OnCancel);
            UpdateCommand = new Command(OnUpdate, ValidateUpdate);
            DeleteCommand = new Command(OnDelete);
            this.PropertyChanged += (_, __) => UpdateCommand.ChangeCanExecute();
        }

        public Command CancelCommand { get; }
        public Command UpdateCommand { get; }
        public Command DeleteCommand { get; }

        public bool IsChange { get; set; } = false;

        public int Id { get; set; }

        public int GameId
        {
            get => _gameId;
            set
            {
                _gameId = value;
                LoadGameId(value);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (!string.IsNullOrEmpty(_name) && !_name.Equals(value))
                    IsChange = true;
                SetProperty(ref _name, value);
            }
        }

        public string MinToPlay
        {
            get => _minToPlay;
            set
            {
                if (!string.IsNullOrEmpty(_minToPlay) && !_minToPlay.Equals(value))
                    IsChange = true;
                SetProperty(ref _minToPlay, value);
            }
        }

        public string MinNumPlayers
        {
            get => _minNumPlayers;
            set
            {
                if (!string.IsNullOrEmpty(_minNumPlayers) && !_minNumPlayers.Equals(value))
                    IsChange = true;
                SetProperty(ref _minNumPlayers, value);
            }
        }

        public string MaxNumPlayers
        {
            get => _maxNumPlayers;
            set
            {
                if (!string.IsNullOrEmpty(_maxNumPlayers) && !_maxNumPlayers.Equals(value))
                    IsChange = true;
                SetProperty(ref _maxNumPlayers, value);
            }
        }

        /// <summary>
        /// Deletes the Game with that id from the Database
        /// </summary>
        /// <param name="obj"></param>
        private async void OnDelete(object obj)
        {
            await this.DataStore.GetResult().DeleteGameAsync(GameId);
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateUpdate(object arg)
        {
            //Check to see if there has been a change to the page's data
            //Make sure all of the fields have data
            //Make sure the MinToPlay, MinNumPlayers, and the MaxNumPlayers all have numeric values
            if (IsChange &&
                !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(MinToPlay) &&
                !string.IsNullOrEmpty(MinNumPlayers) &&
                !string.IsNullOrEmpty(MaxNumPlayers) &&
                int.TryParse(MinToPlay, out int minutes) &&
                int.TryParse(MinNumPlayers, out int minP) &&
                int.TryParse(MaxNumPlayers, out int maxP))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves the updated information to the database
        /// </summary>
        /// <param name="obj"></param>
        private async void OnUpdate(object obj)
        {
            Game updatedGame = new Game()
            {
                Id = this.GameId,
                Name = this.Name,
                MinutesToPlay = int.Parse(this.MinToPlay),
                MinNumberOfPlayers = int.Parse(this.MinNumPlayers),
                MaxNumberOfPlayers = int.Parse(this.MaxNumPlayers)
            };

            var duplicateExists = await DataStore.GetResult().CheckForDuplicateAsync(updatedGame.Name);
            if (duplicateExists.Count > 1 || duplicateExists[0].Id != updatedGame.Id)
            {
                var result = await App.Current.MainPage.DisplayAlert("Warning - Possible Duplicate", "A game already exists with " +
                    "that name. Do you still want to save?", "Yes", "No");

                //If the result was "No", don't save the game and don't leave the page. Just return
                if (!result)
                    return;
            }

            await this.DataStore.GetResult().SaveGameAsync(updatedGame);
            IsChange = false;
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Removes the current page from the naviagation stack
        /// </summary>
        /// <param name="obj"></param>
        private async void OnCancel(object obj)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void LoadGameId(int value)
        {
            try
            {
                var game = await DataStore.GetResult().GetGameAsync(value);
                Id = game.Id;
                Name = game.Name;
                MinToPlay = game.MinutesToPlay.ToString();
                MinNumPlayers = game.MinNumberOfPlayers.ToString();
                MaxNumPlayers = game.MaxNumberOfPlayers.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to Load Item: {ex.Message}");
            }
        }
    }
}
