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
        List<Garrison> garrisons;
        Rectangle selectedSlot;
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
            loadGarrisonInfo();
            selectedSlot = allSlots;
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
            garrisons = GarrisonModel.GetGarrisonInfo(player.PlayerCurrentRegion);

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
            // <--

            // блочим все слоты серым цветом на случай если слот ещё недоступен.
            slot1.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot2.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot3.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot4.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot5.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot6.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot7.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot8.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            slot9.Fill = new SolidColorBrush((Color.FromArgb(208, 51, 54, 51)));
            // <--

            // смотрим уровень замка и в зависимости от уровня красим слоты в цвет загрузки
            if (castle.CastleLvl < slotIncremental)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
            }
            else if (castle.CastleLvl < slotIncremental*2)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
                slot2.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 2, castle)];
            }
            else if (castle.CastleLvl < slotIncremental * 3)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
                slot2.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 2, castle)];
                slot3.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 3, castle)];
            }
            else if (castle.CastleLvl < slotIncremental * 4)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
                slot2.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 2, castle)];
                slot3.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 3, castle)];
                slot4.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 4, castle)];
            }
            else if (castle.CastleLvl < slotIncremental * 5)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
                slot2.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 2, castle)];
                slot3.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 3, castle)];
                slot4.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 4, castle)];
                slot5.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 5, castle)];
            }
            else if (castle.CastleLvl < slotIncremental * 6)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
                slot2.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 2, castle)];
                slot3.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 3, castle)];
                slot4.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 4, castle)];
                slot5.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 5, castle)];
                slot6.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 6, castle)];
            }
            else if (castle.CastleLvl < slotIncremental * 7)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
                slot2.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 2, castle)];
                slot3.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 3, castle)];
                slot4.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 4, castle)];
                slot5.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 5, castle)];
                slot6.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 6, castle)];
                slot7.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 7, castle)];
            }
            else if (castle.CastleLvl < slotIncremental * 8)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
                slot2.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 2, castle)];
                slot3.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 3, castle)];
                slot4.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 4, castle)];
                slot5.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 5, castle)];
                slot6.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 6, castle)];
                slot7.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 7, castle)];
                slot8.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 8, castle)];
            }
            else if (castle.CastleLvl >= slotIncremental * 9)
            {
                slot1.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 1, castle)];
                slot2.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 2, castle)];
                slot3.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 3, castle)];
                slot4.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 4, castle)];
                slot5.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 5, castle)];
                slot6.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 6, castle)];
                slot7.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 7, castle)];
                slot8.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 8, castle)];
                slot9.Fill = loadColor[GarrisonModel.calculateSlotColor(garrisons, 9, castle)];
            }
            // <--
        }

        private void slot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedSlot = (Rectangle)sender;

            this.loadSlots();
            // всем ректанглам красим границу в дефолтный чёрно-коричневый
            this.unmarkRectangles();
            // <--

            // выделяем активный ректангл жёлтым
            Rectangle rectangle = (Rectangle)sender;
            rectangle.Stroke = new SolidColorBrush((Color.FromRgb(199, 176, 8)));
            // <--

            Garrison garrison = new Garrison();
            for (int i = 0; i < garrisons.Count; i++)
            {
                if (garrisons[i].SlotId == Convert.ToInt32((sender as Rectangle).Name.Replace("slot", "")))
                {
                    garrison.ArmySizeCurrent += garrisons[i].ArmySizeCurrent;
                    garrison.ArmyInfantryCount += garrisons[i].ArmyInfantryCount;
                    garrison.ArmyArchersCount += garrisons[i].ArmyArchersCount;
                    garrison.ArmyHorsemanCount += garrisons[i].ArmyHorsemanCount;
                    garrison.ArmySiegegunCount += garrisons[i].ArmySiegegunCount;
                }
            }

            CurrentHarrisonCount.Content = garrison.ArmySizeCurrent;
            InfantryHarrisonCount.Content = garrison.ArmyInfantryCount;
            ArchersHarrisonCount.Content = garrison.ArmyArchersCount;
            KnightsHarrisonCount.Content = garrison.ArmyHorsemanCount;
            SiegeHarrisonCount.Content = garrison.ArmySiegegunCount;
            MaxHarrisonCount.Content = castle.returnMaxTroopsInSlot();
        }

        private void allSlots_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedSlot = (Rectangle)sender;

            this.loadGarrisonInfo();
        }

        private void unmarkRectangles()
        {
            // всем ректанглам красим границу в дефолтный чёрно-коричневый
            slot1.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            slot2.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            slot3.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            slot4.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            slot5.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            slot6.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            slot7.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            slot8.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            slot9.Stroke    = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            allSlots.Stroke = new SolidColorBrush((Color.FromRgb(49, 24, 24)));
            // <--
        }

        private void loadGarrisonInfo()
        {
            allSlots.Stroke = new SolidColorBrush((Color.FromRgb(199, 176, 8)));

            Garrison garrison = new Garrison();
            for (int i = 0; i < garrisons.Count; i++)
            {
                garrison.ArmySizeCurrent += garrisons[i].ArmySizeCurrent;
                garrison.ArmyInfantryCount += garrisons[i].ArmyInfantryCount;
                garrison.ArmyArchersCount += garrisons[i].ArmyArchersCount;
                garrison.ArmyHorsemanCount += garrisons[i].ArmyHorsemanCount;
                garrison.ArmySiegegunCount += garrisons[i].ArmySiegegunCount;
            }

            CurrentHarrisonCount.Content = garrison.ArmySizeCurrent;
            InfantryHarrisonCount.Content = garrison.ArmyInfantryCount;
            ArchersHarrisonCount.Content = garrison.ArmyArchersCount;
            KnightsHarrisonCount.Content = garrison.ArmyHorsemanCount;
            SiegeHarrisonCount.Content = garrison.ArmySiegegunCount;
            MaxHarrisonCount.Content = castle.returnMaxTroops();
        }

        private void btnSendArmy_Click(object sender, RoutedEventArgs e)
        {
            garrisons = GarrisonModel.GetGarrisonInfo(player.PlayerCurrentRegion);

            Garrison garrison = new Garrison();
            for (int i = 0; i < garrisons.Count; i++)
            {
                garrison.ArmySizeCurrent += garrisons[i].ArmySizeCurrent;
                garrison.ArmyInfantryCount += garrisons[i].ArmyInfantryCount;
                garrison.ArmyArchersCount += garrisons[i].ArmyArchersCount;
                garrison.ArmyHorsemanCount += garrisons[i].ArmyHorsemanCount;
                garrison.ArmySiegegunCount += garrisons[i].ArmySiegegunCount;
            }

            castle = CastleModel.GetCastleInfo(player.PlayerCurrentRegion);

            if (selectedSlot.Name != "allSlots" && selectedSlot.Name != "")
            {
                if (garrison.ArmySizeCurrent + sliderInfantry.Value + sliderArchers.Value + sliderKnights.Value + sliderSiege.Value <= castle.returnMaxTroopsInSlot())
                {
                    Garrison garrisonNew = new Garrison();

                    garrisonNew.LandId = player.PlayerCurrentRegion;
                    garrisonNew.PlayerId = player.PlayerId;
                    garrisonNew.ArmyInfantryCount = Convert.ToInt32(sliderInfantry.Value);
                    garrisonNew.ArmyArchersCount = Convert.ToInt32(sliderArchers.Value);
                    garrisonNew.ArmyHorsemanCount = Convert.ToInt32(sliderKnights.Value);
                    garrisonNew.ArmySiegegunCount = Convert.ToInt32(sliderSiege.Value);
                    garrisonNew.ArmySizeCurrent = garrisonNew.ArmyInfantryCount + garrisonNew.ArmyArchersCount + garrisonNew.ArmyHorsemanCount + garrisonNew.ArmySiegegunCount;
                    garrisonNew.ArmyId = UserModel.GenerateId();
                    garrisonNew.SlotId = Convert.ToInt32(selectedSlot.Name.Replace("slot", ""));

                    if (garrisonNew.ArmySizeCurrent > 0)
                    {
                        GarrisonModel.InsertGarrison(garrisonNew);
                    }

                    this.slot_MouseDown(selectedSlot, null);
                }
            }
            else
            {
                MessageBox.Show("Please, choose castle slot.");
            }
            
        }
    }
}
