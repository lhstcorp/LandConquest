using LandConquest.Entities;
using LandConquest.Forms;
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

namespace LandConquest.DialogWIndows
{
    
    public partial class CreateListingDialog : Window
    {
        SqlConnection connection;
        Player player;
        StorageModel model;
        PlayerStorage storage;
        PlayerEquipment equipment;
        EquipmentModel equipmentModel;
        Peasants peasants;
        ArmyModel armyModel;
        PeasantModel peasantModel;
        Army army;

        public CreateListingDialog(SqlConnection _connection, Player _player)
        {
            InitializeComponent();
            connection = _connection;
            player = _player;
            Loaded += Window_Loaded;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            storage = new PlayerStorage();
            model = new StorageModel();
            equipment = new PlayerEquipment();
            equipmentModel = new EquipmentModel();

            peasants = new Peasants();
            armyModel = new ArmyModel();
            peasantModel = new PeasantModel();

            army = new Army();

            peasants = peasantModel.GetPeasantsInfo(player, connection, peasants);

            army = armyModel.GetArmyInfo(connection, player, army);

            labelPeasantAmount.Content = peasants.PeasantsCount.ToString();
            labelBowmanAmount.Content = army.ArmyArchersCount.ToString();
            labelInfantryAmount.Content = army.ArmyInfantryCount.ToString();
            labelKnightAmount.Content = army.ArmyHorsemanCount.ToString();
            labelSiegeMachinesAmount.Content = army.ArmySiegegunCount.ToString();

            storage = model.GetPlayerStorage(player, connection, storage);
            equipment = equipmentModel.GetPlayerEquipment(player, connection, equipment);

            labelWoodAmount.Content = storage.PlayerWood.ToString();
            labelStoneAmount.Content = storage.PlayerStone.ToString();
            labelFoodAmount.Content = storage.PlayerFood.ToString();
            labelGemsAmount.Content = storage.PlayerGems.ToString();
            labelCopperAmount.Content = storage.PlayerCopper.ToString();
            labelGoldAmount.Content = storage.PlayerGoldOre.ToString();
            labelIronAmount.Content = storage.PlayerIron.ToString();
            labelLeatherAmount.Content = storage.PlayerLeather.ToString();

            labelHarnessAmount.Content = equipment.PlayerHarness.ToString();
            labelGearAmount.Content = equipment.PlayerGear.ToString();
            labelSpearAmount.Content = equipment.PlayerSpear.ToString();
            labelBowAmount.Content = equipment.PlayerBow.ToString();
            labelArmorAmount.Content = equipment.PlayerArmor.ToString();
            labelSwordAmount.Content = equipment.PlayerSword.ToString();

        }
    }
}
