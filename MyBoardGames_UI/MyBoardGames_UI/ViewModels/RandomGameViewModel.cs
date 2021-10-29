using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using MyBoardGames_UI.Models;
using System.Diagnostics;

namespace MyBoardGames_UI.ViewModels
{
    public class RandomGameViewModel : BaseViewModel
    {
        private string _minutesToPlay;
        private string _numOfPlayers;
        private string _randomGameName;
        private bool _randomGameIsVisible;
        
        public ObservableCollection<Game> MatchingGames { get; }
        public Command GenerateCommand { get; }

        public RandomGameViewModel()
        {
            MatchingGames = new ObservableCollection<Game>();
            GenerateCommand = new Command(OnGenerate, ValidateGenerate);
            this.PropertyChanged += (_, __) => GenerateCommand.ChangeCanExecute();
        }

        public bool RandomGameIsVisible
        {
            get => _randomGameIsVisible;
            set => SetProperty(ref _randomGameIsVisible, value);
        }

        public string RandomGameName
        {
            get => _randomGameName;
            set => SetProperty(ref _randomGameName, value);
        }

        public string MinutesToPlay
        {
            get => _minutesToPlay;
            set => SetProperty(ref _minutesToPlay, value);
        }

        public string NumOfPlayers
        {
            get => _numOfPlayers;
            set => SetProperty(ref _numOfPlayers, value);
        }

        public void OnAppearing()
        {
            IsBusy = false;
            RandomGameIsVisible = false;
            MinutesToPlay = string.Empty;
            NumOfPlayers = string.Empty;
            RandomGameName = string.Empty;
        }

        /// <summary>
        /// Validates the user entered the required info and the correct info
        /// </summary>
        /// <returns></returns>
        public bool ValidateGenerate()
        {
            //If MinutesToPlay and NumOfPlayers is not empty and they are both numbers return true
            if (!string.IsNullOrEmpty(MinutesToPlay) && 
                !string.IsNullOrEmpty(NumOfPlayers) &&
                int.TryParse(MinutesToPlay, out int min) &&
                int.TryParse(NumOfPlayers, out int players))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a random board game based on the entered info
        /// </summary>
        public void OnGenerate()
        {
            IsBusy = true;
            try
            {
                var allGames = this.DataStore.GetResult().GetAllGamesAsync();
                var gamesThatMatchCriteria = allGames.Result.Where(g => g.MinutesToPlay <= int.Parse(MinutesToPlay) &&
                    g.MaxNumberOfPlayers >= int.Parse(NumOfPlayers) &&
                    g.MinNumberOfPlayers <= int.Parse(NumOfPlayers)).ToList();

                if (gamesThatMatchCriteria.Count() > 0)
                {
                    Random random = new Random();
                    var randIndex = random.Next(gamesThatMatchCriteria.Count());
                    RandomGameName = gamesThatMatchCriteria[randIndex].Name;

                    //Clear the current games from the list and then add in the new list of games
                    MatchingGames.Clear();
                    foreach (var game in gamesThatMatchCriteria)
                    {
                        MatchingGames.Add(game);
                    }

                    RandomGameIsVisible = true;
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Error", "No Games Found that match that criteria", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve games that match the specified criteria with the following error message: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
