using LandConquest.Entities;
using LandConquest.Models;
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

namespace LandConquest.Forms
{

    public partial class RecruitWindow : Window
    {
        SqlConnection connection;
        Player player;
        PlayerStorage storage;
        PlayerEquipment equipment;
        EquipmentModel equipmentModel;
        Peasants peasants;
        ArmyModel armyModel;
        Army army = new Army();

        PeasantModel peasantModel;

        PlayerModel playerModel;

        public RecruitWindow(SqlConnection _connection, Player _player, PlayerStorage _storage, PlayerEquipment _equipment)
        {
            InitializeComponent();
            connection = _connection;
            player = _player;
            storage = _storage;
            equipment = _equipment;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            playerModel = new PlayerModel();
            peasantModel = new PeasantModel();
            equipmentModel = new EquipmentModel();
            peasants = new Peasants();
            armyModel = new ArmyModel();
            peasants = peasantModel.GetPeasantsInfo(player, connection, peasants);

            army = armyModel.GetArmyInfo(connection, player, army);

            TotalRecruitPeasants.Content = peasants.PeasantsCount.ToString();
            TotalRecruitArchers.Content = army.ArmyArchersCount.ToString();
            TotalRecruitInfantry.Content = army.ArmyInfantryCount.ToString();
            TotalRecruitKnights.Content = army.ArmyHorsemanCount.ToString();
            TotalRecruitSiege.Content = army.ArmySiegegunCount.ToString();

            AvailableRecruitPeasants.Content = (peasants.PeasantsMax - peasants.PeasantsCount).ToString();
        }

        private void HirePeasants_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney > Convert.ToInt32(PeasantsAmount.Text)) && (Convert.ToInt32(PeasantsAmount.Text)<Convert.ToInt32(AvailableRecruitPeasants.Content)))
            {
                peasants.PeasantsCount += Convert.ToInt32(PeasantsAmount.Text);

                peasants = peasantModel.UpdatePeasantsInfo(peasants, connection);
                TotalRecruitPeasants.Content = peasants.PeasantsCount.ToString();
                AvailableRecruitPeasants.Content = (peasants.PeasantsMax - peasants.PeasantsCount).ToString();
                player.PlayerMoney -= Convert.ToInt32(PeasantsAmount.Text);
                player = playerModel.UpdatePlayerMoney(player, connection);
                MessageBox.Show("OK");
                PeasantsAmount.Text = "0";
            }
            else
            {
                MessageBox.Show("Error");
                PeasantsAmount.Text = "0";
            }
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HireArchers_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney >= Convert.ToInt32(ArchersAmount.Text)*5) && (equipment.PlayerBow >= Convert.ToInt32(ArchersAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(ArchersAmount.Text))))
            {
                army.ArmyArchersCount += Convert.ToInt32(ArchersAmount.Text);
                army.ArmySizeCurrent += Convert.ToInt32(ArchersAmount.Text);
                player.PlayerMoney -= Convert.ToInt32(ArchersAmount.Text)*5;
                player = playerModel.UpdatePlayerMoney(player, connection);
                equipment.PlayerBow -= Convert.ToInt32(ArchersAmount.Text);
                equipment.PlayerArmor -= Convert.ToInt32(ArchersAmount.Text);
                equipmentModel.UpdateEquipment(connection, player, equipment);
                TotalRecruitArchers.Content = army.ArmyArchersCount.ToString();

                army.ArmyType = armyModel.ReturnTypeOfArmy(army);
                army = armyModel.UpdateArmy(connection, army);

                MessageBox.Show("OK");
                ArchersAmount.Text = "0";
            }
            else
            {
                MessageBox.Show("Error");
                ArchersAmount.Text = "0";
            }
        }

        private void HireInfantry_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney >= Convert.ToInt32(InfantryAmount.Text) * 5) && (equipment.PlayerSword >= Convert.ToInt32(InfantryAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(InfantryAmount.Text))))
            {
                army.ArmyInfantryCount += Convert.ToInt32(InfantryAmount.Text);
                army.ArmySizeCurrent += Convert.ToInt32(InfantryAmount.Text);
                player.PlayerMoney -= Convert.ToInt32(InfantryAmount.Text) * 5;
                player = playerModel.UpdatePlayerMoney(player, connection);
                equipment.PlayerSword -= Convert.ToInt32(InfantryAmount.Text);
                equipment.PlayerArmor -= Convert.ToInt32(InfantryAmount.Text);
                equipmentModel.UpdateEquipment(connection, player, equipment);
                TotalRecruitInfantry.Content = army.ArmyInfantryCount.ToString();

                army.ArmyType = armyModel.ReturnTypeOfArmy(army);
                army = armyModel.UpdateArmy(connection, army);

                MessageBox.Show("OK");
                InfantryAmount.Text = "0";
            }
            else
            {
                MessageBox.Show("Error");
                InfantryAmount.Text = "0";
            }
        }

        private void HireKnights_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney >= Convert.ToInt32(KnightsAmount.Text) * 25) && (equipment.PlayerSpear >= Convert.ToInt32(KnightsAmount.Text) && (equipment.PlayerArmor >= Convert.ToInt32(KnightsAmount.Text)) && (equipment.PlayerHarness >= Convert.ToInt32(KnightsAmount.Text))))
            {
                army.ArmyHorsemanCount += Convert.ToInt32(KnightsAmount.Text);
                army.ArmySizeCurrent += Convert.ToInt32(KnightsAmount.Text);
                player.PlayerMoney -= Convert.ToInt32(KnightsAmount.Text) * 25;
                player = playerModel.UpdatePlayerMoney(player, connection);
                equipment.PlayerSpear -= Convert.ToInt32(KnightsAmount.Text);
                equipment.PlayerArmor -= Convert.ToInt32(KnightsAmount.Text);
                equipment.PlayerHarness -= Convert.ToInt32(KnightsAmount.Text);
                equipmentModel.UpdateEquipment(connection, player, equipment);
                TotalRecruitKnights.Content = army.ArmyHorsemanCount.ToString();

                army.ArmyType = armyModel.ReturnTypeOfArmy(army);
                army = armyModel.UpdateArmy(connection, army);

                MessageBox.Show("OK");
                KnightsAmount.Text = "0";
            }
            else
            {
                MessageBox.Show("Error");
                KnightsAmount.Text = "0";
            }
        }

        private void HireSiege_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney >= Convert.ToInt32(SiegeAmount.Text) * 500) && (equipment.PlayerGear >= Convert.ToInt32(SiegeAmount.Text)*5))
            {
                army.ArmySiegegunCount += Convert.ToInt32(SiegeAmount.Text);
                army.ArmySizeCurrent += Convert.ToInt32(SiegeAmount.Text);
                player.PlayerMoney -= Convert.ToInt32(SiegeAmount.Text) * 500;
                player = playerModel.UpdatePlayerMoney(player, connection);
                equipment.PlayerGear -= Convert.ToInt32(SiegeAmount.Text)*5;
                equipmentModel.UpdateEquipment(connection, player, equipment);
                TotalRecruitSiege.Content = army.ArmySiegegunCount.ToString();

                army.ArmyType = armyModel.ReturnTypeOfArmy(army);
                army = armyModel.UpdateArmy(connection, army);

                MessageBox.Show("OK");
                SiegeAmount.Text = "0";
            }
            else
            {
                MessageBox.Show("Error");
                SiegeAmount.Text = "0";
            }
        }
    }
}
