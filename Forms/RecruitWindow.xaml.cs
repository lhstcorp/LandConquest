using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LandConquest.Forms
{
    public partial class RecruitWindow : Window
    {
        private Player player;
        private PlayerEquipment equipment;
        private Peasants peasants;
        private Army army;

        public RecruitWindow(Player _player, PlayerEquipment _equipment)
        {
            InitializeComponent();
            player = _player;
            equipment = _equipment;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            army = new Army();
            peasants = new Peasants();

            peasants = PeasantModel.GetPeasantsInfo(player, peasants);
            army = ArmyModel.GetArmyInfo(player, army);

            TotalRecruitPeasants.Content = peasants.PeasantsCount.ToString();
            TotalRecruitArchers.Content = army.ArmyArchersCount.ToString();
            TotalRecruitInfantry.Content = army.ArmyInfantryCount.ToString();
            TotalRecruitKnights.Content = army.ArmyHorsemanCount.ToString();
            TotalRecruitSiege.Content = army.ArmySiegegunCount.ToString();

            AvailableRecruitPeasants.Content = (peasants.PeasantsMax - peasants.PeasantsCount).ToString();
        }

        private void HirePeasants_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney > Convert.ToInt32(PeasantsAmount.Text)) && (Convert.ToInt32(PeasantsAmount.Text) < Convert.ToInt32(AvailableRecruitPeasants.Content)))
            {
                peasants.PeasantsCount += Convert.ToInt32(PeasantsAmount.Text);

                peasants = PeasantModel.UpdatePeasantsInfo(peasants);
                TotalRecruitPeasants.Content = peasants.PeasantsCount.ToString();
                AvailableRecruitPeasants.Content = (peasants.PeasantsMax - peasants.PeasantsCount).ToString();
                player.PlayerMoney -= Convert.ToInt32(PeasantsAmount.Text);
                player = PlayerModel.UpdatePlayerMoney(player);
                PeasantsAmount.Text = "0";
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error");
                PeasantsAmount.Text = "0";
            }
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HireArchers_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney >= Convert.ToInt32(ArchersAmount.Text) * 5) && (equipment.PlayerBow >= Convert.ToInt32(ArchersAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(ArchersAmount.Text))))
            {
                army.ArmyArchersCount += Convert.ToInt32(ArchersAmount.Text);
                army.ArmySizeCurrent += Convert.ToInt32(ArchersAmount.Text);
                player.PlayerMoney -= Convert.ToInt32(ArchersAmount.Text) * 5;
                player = PlayerModel.UpdatePlayerMoney(player);
                equipment.PlayerBow -= Convert.ToInt32(ArchersAmount.Text);
                equipment.PlayerArmor -= Convert.ToInt32(ArchersAmount.Text);
                EquipmentModel.UpdateEquipment(player, equipment);
                TotalRecruitArchers.Content = army.ArmyArchersCount.ToString();

                army.ArmyType = ArmyModel.ReturnTypeOfArmy(army);
                army = ArmyModel.UpdateArmy(army);

                ArchersAmount.Text = "0";
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error");
                ArchersAmount.Text = "0";
            }
        }

        private void HireInfantry_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney >= Convert.ToInt32(InfantryAmount.Text) * 5) && (equipment.PlayerSpear >= Convert.ToInt32(InfantryAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(InfantryAmount.Text))))
            {
                army.ArmyInfantryCount += Convert.ToInt32(InfantryAmount.Text);
                army.ArmySizeCurrent += Convert.ToInt32(InfantryAmount.Text);
                player.PlayerMoney -= Convert.ToInt32(InfantryAmount.Text) * 5;
                player = PlayerModel.UpdatePlayerMoney(player);
                equipment.PlayerSpear -= Convert.ToInt32(InfantryAmount.Text);
                equipment.PlayerArmor -= Convert.ToInt32(InfantryAmount.Text);
                EquipmentModel.UpdateEquipment(player, equipment);
                TotalRecruitInfantry.Content = army.ArmyInfantryCount.ToString();

                army.ArmyType = ArmyModel.ReturnTypeOfArmy(army);
                army = ArmyModel.UpdateArmy(army);

                InfantryAmount.Text = "0";
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error");
                InfantryAmount.Text = "0";
            }
        }

        private void HireKnights_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney >= Convert.ToInt32(KnightsAmount.Text) * 25) && (equipment.PlayerSword >= Convert.ToInt32(KnightsAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(KnightsAmount.Text)) && (equipment.PlayerHarness >= Convert.ToInt32(KnightsAmount.Text))))
            {
                army.ArmyHorsemanCount += Convert.ToInt32(KnightsAmount.Text);
                army.ArmySizeCurrent += Convert.ToInt32(KnightsAmount.Text);
                player.PlayerMoney -= Convert.ToInt32(KnightsAmount.Text) * 25;
                player = PlayerModel.UpdatePlayerMoney(player);
                equipment.PlayerSword -= Convert.ToInt32(KnightsAmount.Text);
                equipment.PlayerArmor -= Convert.ToInt32(KnightsAmount.Text);
                equipment.PlayerHarness -= Convert.ToInt32(KnightsAmount.Text);
                EquipmentModel.UpdateEquipment(player, equipment);
                TotalRecruitKnights.Content = army.ArmyHorsemanCount.ToString();

                army.ArmyType = ArmyModel.ReturnTypeOfArmy(army);
                army = ArmyModel.UpdateArmy(army);

                KnightsAmount.Text = "0";
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error");
                KnightsAmount.Text = "0";
            }
        }

        private void HireSiege_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney >= Convert.ToInt32(SiegeAmount.Text) * 500) && (equipment.PlayerGear >= Convert.ToInt32(SiegeAmount.Text) * 5))
            {
                army.ArmySiegegunCount += Convert.ToInt32(SiegeAmount.Text);
                army.ArmySizeCurrent += Convert.ToInt32(SiegeAmount.Text);
                player.PlayerMoney -= Convert.ToInt32(SiegeAmount.Text) * 500;
                player = PlayerModel.UpdatePlayerMoney(player);
                equipment.PlayerGear -= Convert.ToInt32(SiegeAmount.Text) * 5;
                EquipmentModel.UpdateEquipment(player, equipment);
                TotalRecruitSiege.Content = army.ArmySiegegunCount.ToString();

                army.ArmyType = ArmyModel.ReturnTypeOfArmy(army);
                army = ArmyModel.UpdateArmy(army);

                SiegeAmount.Text = "0";
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error");
                SiegeAmount.Text = "0";
            }
        }

        private void PeasantsAmount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 1 && i <= 9999;
        }
    }
}
