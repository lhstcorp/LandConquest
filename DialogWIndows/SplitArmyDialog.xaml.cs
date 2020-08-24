using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.DialogWIndows
{
    /// <summary>
    /// Логика взаимодействия для SplitArmyDialog.xaml
    /// </summary>
    public partial class SplitArmyDialog : Window
    {
        ArmyInBattle armyInBattle;
        SqlConnection connection;
        public SplitArmyDialog(SqlConnection _connection, ArmyInBattle _armyInBattle)
        {
            connection = _connection;
            armyInBattle = _armyInBattle;
            InitializeComponent();

            sliderInfantryWas.IsSnapToTickEnabled = true;
            sliderInfantryNow.IsSnapToTickEnabled = true;
            sliderArchersNow.IsSnapToTickEnabled = true;
            sliderArchersWas.IsSnapToTickEnabled = true;
            sliderKnightsNow.IsSnapToTickEnabled = true;
            sliderKnightsWas.IsSnapToTickEnabled = true;
            sliderSiegeNow.IsSnapToTickEnabled = true;
            sliderSiegeWas.IsSnapToTickEnabled = true;

            sliderInfantryWas.Maximum = armyInBattle.ArmyInfantryCount;
            sliderInfantryWas.Value = armyInBattle.ArmyInfantryCount;
            sliderInfantryNow.Maximum = armyInBattle.ArmyInfantryCount;
            sliderInfantryNow.Value = 0;

            sliderArchersWas.Maximum = armyInBattle.ArmyArchersCount;
            sliderArchersWas.Value = armyInBattle.ArmyArchersCount;
            sliderArchersNow.Maximum = armyInBattle.ArmyArchersCount;
            sliderArchersNow.Value = 0;

            sliderKnightsWas.Maximum = armyInBattle.ArmyHorsemanCount;
            sliderKnightsWas.Value = armyInBattle.ArmyHorsemanCount;
            sliderKnightsNow.Maximum = armyInBattle.ArmyHorsemanCount;
            sliderKnightsNow.Value = 0;

            sliderSiegeWas.Maximum = armyInBattle.ArmySiegegunCount;
            sliderSiegeWas.Value = armyInBattle.ArmySiegegunCount;
            sliderSiegeNow.Maximum = armyInBattle.ArmySiegegunCount;
            sliderSiegeNow.Value = 0;

            switch (armyInBattle.ArmyType)
            {
                case 1:
                    {
                        armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
                        break;
                    }
                case 2:
                    {
                        armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                        break;
                    }
                case 3:
                    {
                        armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                        break;
                    }
                case 4:
                    {
                        armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                        break;
                    }
                case 5:
                    {
                        armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/peasants_total.png", UriKind.Relative));
                        break;
                    }
            }

            armySizeWas.Content = armyInBattle.ArmySizeCurrent;
        }

        private void sliderInfantryWas_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderInfantryNow.Value = sliderInfantryWas.Maximum - sliderInfantryWas.Value;
        }

        private void sliderInfantryNow_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderInfantryWas.Value = sliderInfantryNow.Maximum - sliderInfantryNow.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void sliderArchersWas_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderArchersNow.Value = sliderArchersWas.Maximum - sliderArchersWas.Value;
        }

        private void sliderArchersNow_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderArchersWas.Value = sliderArchersNow.Maximum - sliderArchersNow.Value;
        }

        private void sliderKnightsWas_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderKnightsNow.Value = sliderKnightsWas.Maximum - sliderKnightsWas.Value;
        }

        private void sliderKnightsNow_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderKnightsWas.Value = sliderKnightsNow.Maximum - sliderKnightsNow.Value;
        }

        private void sliderSiegeWas_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderSiegeNow.Value = sliderSiegeWas.Maximum - sliderSiegeWas.Value;
        }

        private void sliderSiegeNow_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderSiegeWas.Value = sliderSiegeNow.Maximum - sliderSiegeNow.Value;
        }

        private void CheckTypeAndReturnCount(object sender, MouseButtonEventArgs e)
        {
            armySizeWas.Content = Convert.ToInt32(infantryCountWas.Content) + Convert.ToInt32(archersCountWas.Content) + Convert.ToInt32(knightsCountWas.Content) + Convert.ToInt32(siegeCountWas.Content);

            if (infantryCountWas.Content == armySizeWas.Content)
            {
                armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
            }
            else
            if (archersCountWas.Content == armySizeWas.Content)
            {
                armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
            }
            else
            if (knightsCountWas.Content == armySizeWas.Content)
            {
                armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
            }
            else
            if (siegeCountWas.Content == armySizeWas.Content)
            {
                armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
            }
            else armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/question.jpg", UriKind.Relative));

            armySizeNow.Content = Convert.ToInt32(infantryCountNow.Content) + Convert.ToInt32(archersCountNow.Content) + Convert.ToInt32(knightsCountNow.Content) + Convert.ToInt32(siegeCountNow.Content);

            if (infantryCountNow.Content == armySizeNow.Content)
            {
                armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
            }
            else
            {
                if (archersCountNow.Content == armySizeNow.Content)
                {
                    armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                }
                
                else {
                    if (knightsCountNow.Content == armySizeNow.Content)
                    {
                        armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                    }
                    else
                    {
                        if (siegeCountNow.Content == armySizeNow.Content)
                        {
                            armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                        }
                        else {
                            armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/question.jpg", UriKind.Relative));
                        }
                    }
                }
            }
        }
    }
}
