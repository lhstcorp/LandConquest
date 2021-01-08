﻿using LandConquest.Entities;
using LandConquest.Models;
using System.Windows;

namespace LandConquest.DialogWIndows
{

    public partial class EstablishStateDialog : Window
    {
        Player player;
        Land land;

        LandModel landModel;
        CountryModel countryModel;

        public EstablishStateDialog(Player _player, Land _land)
        {
            InitializeComponent();
            player = _player;
            land = _land;
            Loaded += Window_Loaded;
        }

        private void EstablishState_Click(object sender, RoutedEventArgs e)
        {
            Country country = CountryModel.EstablishaState(player, land, StateColor.Color);
            country.CountryId = CountryModel.SelectLastIdOfStates();
            LandModel.UpdateLandInfo(land, country);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            landModel = new LandModel();
            countryModel = new CountryModel();
        }
    }
}
