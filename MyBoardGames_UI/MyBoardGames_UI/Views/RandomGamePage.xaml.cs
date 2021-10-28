﻿using MyBoardGames_UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyBoardGames_UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RandomGamePage : ContentPage
    {
        public RandomGamePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((RandomGameViewModel)BindingContext).OnAppearing();
        }
    }
}