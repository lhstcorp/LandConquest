using LandConquest.DialogWIndows;
using LandConquest.Logic;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using LandConquestDB.Forces;

namespace LandConquest.Forms
{
    public partial class RecruitWindow : Window
    {
        private Player player;
        private PlayerEquipment equipment;
        private Peasants peasants;
        private Army army;
        //private ForcesEnum forcesEnum;
       

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
            //forcesEnum = new ForcesEnum();
            

            peasants = PeasantModel.GetPeasantsInfo(player, peasants);
            army = ArmyModel.GetArmyInfo(player, army);
            //forcesEnum = ForcesEnum.Archers.

            TotalRecruitPeasants.Content = peasants.PeasantsCount.ToString();
            TotalRecruitArchers.Content = army.ArmyArchersCount.ToString();
            TotalRecruitInfantry.Content = army.ArmyInfantryCount.ToString();
            TotalRecruitKnights.Content = army.ArmyHorsemanCount.ToString();
            TotalRecruitSiege.Content = army.ArmySiegegunCount.ToString();

            archerConsumption.Content = (int)ConsumptionLogic.Consumption.Archers;
            infanrtyConsumption.Content = (int)ConsumptionLogic.Consumption.Infantry;
            knightsConsumption.Content = (int)ConsumptionLogic.Consumption.Knights;
            siegeConsumption.Content = (int)ConsumptionLogic.Consumption.Siege;

            AvailableRecruitPeasants.Content = (peasants.PeasantsMax - peasants.PeasantsCount).ToString();


            healthArchersLabel.Content = ForcesEnum.Archers.Health.ToString();
            forceArchersLabel.Content = ForcesEnum.Archers.Damage.ToString();
            defenceArchersLabel.Content = ForcesEnum.Archers.Defence.ToString();
            rangeArchersLabel.Content = ForcesEnum.Archers.Range.ToString();
            movementArchersLabel.Content = ForcesEnum.Archers.Movement.ToString();

            healthInfantryLabel.Content = ForcesEnum.Infantry.Health.ToString();
            forceInfantryLabel.Content = ForcesEnum.Infantry.Damage.ToString();
            defenceInfantryLabel.Content = ForcesEnum.Infantry.Defence.ToString();
            rangeInfantryLabel.Content = ForcesEnum.Infantry.Range.ToString();
            movementInfantryLabel.Content = ForcesEnum.Infantry.Movement.ToString();

            healthKnightsLabel.Content = ForcesEnum.Knights.Health.ToString();
            forceKnightsLabel.Content = ForcesEnum.Knights.Damage.ToString();
            defenceKnightsLabel.Content = ForcesEnum.Knights.Defence.ToString();
            rangeKnightsLabel.Content = ForcesEnum.Knights.Range.ToString();
            movementKnightsLabel.Content = ForcesEnum.Knights.Movement.ToString();

            healthSiegeLabel.Content = ForcesEnum.Siege.Health.ToString();
            forceSiegeLabel.Content = ForcesEnum.Siege.Damage.ToString();
            defenceSiegeLabel.Content = ForcesEnum.Siege.Defence.ToString();
            rangeSiegeLabel.Content = ForcesEnum.Siege.Range.ToString();
            movementSiegeLabel.Content = ForcesEnum.Siege.Movement.ToString();


        }

        private void HirePeasants_Click(object sender, RoutedEventArgs e)
        {
            if (PeasantsAmount.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((player.PlayerMoney > Convert.ToInt32(PeasantsAmount.Text)) && (Convert.ToInt32(PeasantsAmount.Text) < Convert.ToInt32(AvailableRecruitPeasants.Content)))
            {
                peasants.PeasantsCount += Convert.ToInt32(PeasantsAmount.Text);

                peasants = PeasantModel.UpdatePeasantsInfo(peasants);
                TotalRecruitPeasants.Content = peasants.PeasantsCount.ToString();
                AvailableRecruitPeasants.Content = (peasants.PeasantsMax - peasants.PeasantsCount).ToString();
                player.PlayerMoney -= Convert.ToInt32(PeasantsAmount.Text);
                player = PlayerModel.UpdatePlayerMoney(player);

            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");

            }
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HireArchers_Click(object sender, RoutedEventArgs e)
        {
            if (ArchersAmount.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((player.PlayerMoney >= Convert.ToInt32(ArchersAmount.Text) * 5) && (equipment.PlayerBow >= Convert.ToInt32(ArchersAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(ArchersAmount.Text))))
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


            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");

            }
        }

        private void HireInfantry_Click(object sender, RoutedEventArgs e)
        {
            if (InfantryAmount.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((player.PlayerMoney >= Convert.ToInt32(InfantryAmount.Text) * 5) && (equipment.PlayerSpear >= Convert.ToInt32(InfantryAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(InfantryAmount.Text))))
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


            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");

            }
        }

        private void HireKnights_Click(object sender, RoutedEventArgs e)
        {
            if (KnightsAmount.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((player.PlayerMoney >= Convert.ToInt32(KnightsAmount.Text) * 25) && (equipment.PlayerSword >= Convert.ToInt32(KnightsAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(KnightsAmount.Text)) && (equipment.PlayerHarness >= Convert.ToInt32(KnightsAmount.Text))))
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


            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");

            }
        }

        private void HireSiege_Click(object sender, RoutedEventArgs e)
        {
            if (SiegeAmount.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((player.PlayerMoney >= Convert.ToInt32(SiegeAmount.Text) * 500) && (equipment.PlayerGear >= Convert.ToInt32(SiegeAmount.Text) * 5))
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


            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");

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
