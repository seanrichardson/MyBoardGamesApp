using MyBoardGames_UI.Models;
using MyBoardGames_UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyBoardGames_UI.Views
{
    public partial class NewGamePage : ContentPage
    {
        public Game Game { get; set; }
        public NewGamePage()
        {
            InitializeComponent();
        }
    }
}