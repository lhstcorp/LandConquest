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

    public partial class StorageWindow : Window
    {
        SqlConnection connection;
        MainWindow window;
        Player player;
        User user;
        StorageModel model;
        PlayerStorage storage;
        PlayerEquipment equipment;
        EquipmentModel equipmentModel;
        public StorageWindow(MainWindow _window,SqlConnection _connection, Player _player, User _user)
        {
            InitializeComponent();
            window = _window; 
            connection = _connection;
            player = _player;
            user = _user;
            Loaded += StorageWindow_Loaded;
        }

        private void StorageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            storage = new PlayerStorage();
            model = new StorageModel();
            equipment = new PlayerEquipment();
            equipmentModel = new EquipmentModel();

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

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void craftSword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Convert.ToInt32(labelSwordAmount1.Content)*(Convert.ToInt32(SwordsToCraft.Text)) <= storage.PlayerCopper)&&(Convert.ToInt32(labelSwordAmount2.Content)* (Convert.ToInt32(SwordsToCraft.Text)) <= storage.PlayerIron))
            {
                storage.PlayerCopper -= (Convert.ToInt32(labelSwordAmount1.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
                storage.PlayerIron -= (Convert.ToInt32(labelSwordAmount2.Content) * (Convert.ToInt32(SwordsToCraft.Text)));

                equipment.PlayerSword += Convert.ToInt32(SwordsToCraft.Text);
                model.UpdateStorage(connection, player, storage);
                equipmentModel.UpdateEquipment(connection, player, equipment);

                labelSwordAmount.Content = Convert.ToInt32(labelSwordAmount.Content) + Convert.ToInt32(SwordsToCraft.Text);
                labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) - (Convert.ToInt32(labelSwordAmount1.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
                labelIronAmount.Content= Convert.ToInt32(labelIronAmount.Content)- (Convert.ToInt32(labelSwordAmount2.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }
    }
}
