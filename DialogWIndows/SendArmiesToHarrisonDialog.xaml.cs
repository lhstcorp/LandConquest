using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для SendArmiesToHarrisonDialog.xaml
    /// </summary>
    public partial class SendArmiesToHarrisonDialog : Window
    {
        private Player player;
        Castle castle;
        public SendArmiesToHarrisonDialog(Player _player)
        {
            player = _player;
            InitializeComponent();
        }

        private void sliderInfantry_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderInfantry.IsSnapToTickEnabled = true;
        }

        private void sliderArchers_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderArchers.IsSnapToTickEnabled = true;
        }

        private void sliderKnights_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderKnights.IsSnapToTickEnabled = true;
        }

        private void sliderSiege_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderSiege.IsSnapToTickEnabled = true;
        }

        private void btnWarWindowClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void buttonCollapse_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadSliders();
            loadCastle();
            loadSlots();
        }

        private void loadSliders()
        {
            Army fullPlayerArmy = new Army();
            fullPlayerArmy = ArmyModel.GetArmyInfo(player, fullPlayerArmy);

            ArmyInBattle freeArmy = new ArmyInBattle(fullPlayerArmy);
            List<ArmyInBattle> playerArmies = new List<ArmyInBattle>();
            playerArmies = BattleModel.GetAllPlayerArmiesInfo(playerArmies, player);

            for (int i = 0; i < playerArmies.Count; i++)
            {
                freeArmy.ArmyArchersCount -= playerArmies[i].ArmyArchersCount;
                freeArmy.ArmyInfantryCount -= playerArmies[i].ArmyInfantryCount;
                freeArmy.ArmyHorsemanCount -= playerArmies[i].ArmyHorsemanCount;
                freeArmy.ArmySiegegunCount -= playerArmies[i].ArmySiegegunCount;
                freeArmy.ArmySizeCurrent -= playerArmies[i].ArmySizeCurrent;
            }

            sliderInfantry.Value = 0;
            sliderArchers.Value = 0;
            sliderKnights.Value = 0;
            sliderSiege.Value = 0;

            sliderInfantry.Maximum = freeArmy.ArmyInfantryCount;
            sliderArchers.Maximum = freeArmy.ArmyArchersCount;
            sliderKnights.Maximum = freeArmy.ArmyHorsemanCount;
            sliderSiege.Maximum = freeArmy.ArmySiegegunCount;
            // here I am;
        }

        private void loadCastle()
        {
            castle = CastleModel.GetCastleInfo(player.PlayerCurrentRegion);
        }

        private void loadSlots()
        {
            const int slotIncremental = 100;
            List<Garrison> garrisons = GarrisonModel.GetGarrisonInfo(player.PlayerCurrentRegion);

            // константные цвета для отображения загрузки слотов
            List<SolidColorBrush> loadColor = new List<SolidColorBrush>();
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 70, 210, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 115, 210, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 145, 210, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 175, 210, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 210, 190, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 210, 160, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 210, 130, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 210, 100, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 210, 70, 38))));
            loadColor.Add(new SolidColorBrush((Color.FromArgb(208, 210, 38, 38))));


            slot1.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot2.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot3.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot4.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot5.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot6.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot7.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot8.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot9.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));

            if (castle.CastleLvl < slotIncremental)
            {
                //slot1.Fill = new SolidColorBrush((Color.FromArgb(208, 70, 210, 38)));
                slot1.Fill = GarrisonModel.calculateSlotColor(garrisons, 1); // гиблая веточка :)
            }

        }
    }
}
