using MyBoardGames_UI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyBoardGames_UI.ViewModels
{
    public class NewGameViewModel : BaseViewModel
    {
        private string _name;
        private string _minutesToPlay;
        private string _minNumberOfPlayers;
        private string _maxNumberOfPlayers;

        public NewGameViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        /// <summary>
        /// The name of the new game being entered
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// The number of minutes it takes to play the game on average
        /// </summary>
        public string MinutesToPlay
        {
            get => _minutesToPlay;
            set => SetProperty(ref _minutesToPlay, value);
        }

        /// <summary>
        /// The minumum number of players that can play the game
        /// </summary>
        public string MinNumberOfPlayers
        {
            get => _minNumberOfPlayers;
            set => SetProperty(ref _minNumberOfPlayers, value);
        }

        /// <summary>
        /// The maximum number of players that can play the game
        /// </summary>
        public string MaxNumberOfPlayers
        {
            get => _maxNumberOfPlayers;
            set => SetProperty(ref _maxNumberOfPlayers, value);
        }

        /// <summary>
        /// Validates the data that was entered in by the user
        /// </summary>
        /// <returns>True if valie data was entered</returns>
        private bool ValidateSave()
        {
            if (!string.IsNullOrEmpty(Name) && 
                !string.IsNullOrEmpty(MinutesToPlay) &&
                !string.IsNullOrEmpty(MinNumberOfPlayers) &&
                !string.IsNullOrEmpty(MaxNumberOfPlayers))
            {
                if (int.TryParse(MinutesToPlay, out int minutes) &&
                    int.TryParse(MinNumberOfPlayers, out int min) &&
                    int.TryParse(MaxNumberOfPlayers, out int max))
                {
                    //Returns true since every field is filled out and MinutesToPlay and NumberOfPlayers are numeric values
                    return true;
                }
                else
                {
                    //Notify the user that the "# of Minutes to Play", "Min # of Players" and "Max # of Players" need to be a numeric value
                    return false;
                }
            }
            else
            {
                //Notify user that all the fields are required to save a new game
                return false;
            }
        }

        /// <summary>
        /// Creates a new game object and saves it to the database
        /// </summary>
        private async void OnSave()
        {
            Game newGame = new Game()
            {
                Name = Name,
                MinutesToPlay = int.Parse(MinutesToPlay),
                MinNumberOfPlayers = int.Parse(MinNumberOfPlayers),
                MaxNumberOfPlayers = int.Parse(MaxNumberOfPlayers)
            };

            var duplicateExists = await DataStore.GetResult().GetGameAsync(newGame);
            if (duplicateExists != null)
            {
                var result = await App.Current.MainPage.DisplayAlert("Warning - Possible Duplicate", "A game already exists in the database " +
                    "with the same data. Do you still want to save", "Yes", "No");

                //If the result was "No", don't save the game and don't leave the page. Just return
                if (!result)
                    return;
            }

            //Saves the newGame to the datastore
            await DataStore.GetResult().SaveGameAsync(newGame);

            //This will pop the current page off the naviagtion stack
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Pops the NewGamePage off of the navigation stack without saving the changes made
        /// </summary>
        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
