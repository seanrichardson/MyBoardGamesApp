using MyBoardGames_UI.ViewModels;
using MyBoardGames_UI.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyBoardGames_UI
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewGamePage), typeof(NewGamePage));
            Routing.RegisterRoute(nameof(GameDetailPage), typeof(GameDetailPage));
        }

    }
}
