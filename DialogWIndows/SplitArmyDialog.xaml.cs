using LandConquest.Forms;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LandConquest.DialogWIndows
{
    public partial class SplitArmyDialog : Window
    {
        private ArmyInBattle armyInBattle;
        private War war;
        private int typeOfNewArmy;
        private int typeOfOldArmy;
        public SplitArmyDialog(ArmyInBattle _armyInBattle, War _war)
        {
            armyInBattle = _armyInBattle;
            war = _war;
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

            if (Convert.ToInt32(armySizeWas.Content) == 0) armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/question.jpg", UriKind.Relative));
            if (Convert.ToInt32(armySizeNow.Content) == 0) armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/question.jpg", UriKind.Relative));

            if (Convert.ToInt32(infantryCountWas.Content) == Convert.ToInt32(armySizeWas.Content))
            {
                armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
                typeOfOldArmy = 1;
            }
            else
            if (Convert.ToInt32(archersCountWas.Content) == Convert.ToInt32(armySizeWas.Content))
            {
                armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                typeOfOldArmy = 2;
            }
            else
            if (Convert.ToInt32(knightsCountWas.Content) == Convert.ToInt32(armySizeWas.Content))
            {
                armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                typeOfOldArmy = 3;
            }
            else
            if (Convert.ToInt32(siegeCountWas.Content) == Convert.ToInt32(armySizeWas.Content))
            {
                armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                typeOfOldArmy = 4;
            }
            else { armyTypeWasImg.Source = new BitmapImage(new Uri("/Pictures/question.jpg", UriKind.Relative)); typeOfOldArmy = 5; }

            armySizeNow.Content = Convert.ToInt32(infantryCountNow.Content) + Convert.ToInt32(archersCountNow.Content) + Convert.ToInt32(knightsCountNow.Content) + Convert.ToInt32(siegeCountNow.Content);

            if (Convert.ToInt32(infantryCountNow.Content) == Convert.ToInt32(armySizeNow.Content))
            {
                armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
                typeOfNewArmy = 1;
            }
            else
            {
                if (Convert.ToInt32(archersCountNow.Content) == Convert.ToInt32(armySizeNow.Content))
                {
                    armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                    typeOfNewArmy = 2;
                }

                else
                {
                    if (Convert.ToInt32(knightsCountNow.Content) == Convert.ToInt32(armySizeNow.Content))
                    {
                        armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                        typeOfNewArmy = 3;
                    }
                    else
                    {
                        if (Convert.ToInt32(siegeCountNow.Content) == Convert.ToInt32(armySizeNow.Content))
                        {
                            armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                            typeOfNewArmy = 4;
                        }
                        else
                        {
                            armyTypeNowImg.Source = new BitmapImage(new Uri("/Pictures/question.jpg", UriKind.Relative));
                            typeOfNewArmy = 5;
                        }
                    }
                }
            }
        }

        private void btnSplitArmy_Click(object sender, RoutedEventArgs e)
        {
            if ((Convert.ToInt32(armySizeNow.Content) != 0) && (Convert.ToInt32(armySizeWas.Content) != 0))
            {
                ArmyInBattle newArmyInBattle = new ArmyInBattle();
                newArmyInBattle.ArmyId = generateId();
                newArmyInBattle.ArmySizeCurrent = Convert.ToInt32(armySizeNow.Content);
                newArmyInBattle.ArmyInfantryCount = Convert.ToInt32(infantryCountNow.Content);
                newArmyInBattle.ArmyArchersCount = Convert.ToInt32(archersCountNow.Content);
                newArmyInBattle.ArmyHorsemanCount = Convert.ToInt32(knightsCountNow.Content);
                newArmyInBattle.ArmySiegegunCount = Convert.ToInt32(siegeCountNow.Content);
                newArmyInBattle.LocalLandId = armyInBattle.LocalLandId;
                newArmyInBattle.PlayerId = armyInBattle.PlayerId;
                newArmyInBattle.ArmySide = armyInBattle.ArmySide;
                newArmyInBattle.ArmyType = typeOfNewArmy;
                newArmyInBattle.CanMove = armyInBattle.CanMove;

                BattleModel.InsertArmyIntoBattleTable(newArmyInBattle, war);

                List<ArmyInBattle> playerArmies = WarWindow.GetPlayerArmies();
                playerArmies.Add(newArmyInBattle);
                WarWindow.SetPlayerArmies(playerArmies);

            }

            if (Convert.ToInt32(armySizeWas.Content) != 0 && (Convert.ToInt32(armySizeNow.Content) != 0))
            {
                armyInBattle.ArmySizeCurrent = Convert.ToInt32(armySizeWas.Content);

                armyInBattle.ArmyInfantryCount = Convert.ToInt32(infantryCountWas.Content);
                armyInBattle.ArmyArchersCount = Convert.ToInt32(archersCountWas.Content);
                armyInBattle.ArmyHorsemanCount = Convert.ToInt32(knightsCountWas.Content);
                armyInBattle.ArmySiegegunCount = Convert.ToInt32(siegeCountWas.Content);
                armyInBattle.ArmyType = typeOfOldArmy;

                BattleModel.UpdateArmyInBattle(armyInBattle);
            }

            this.Close();
        }

        public string generateId()
        {
            Thread.Sleep(15);
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
