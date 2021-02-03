using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LandConquest.Forms
{

    public partial class StorageWindow : Window
    {
        private Player player;
        private PlayerStorage storage;
        private PlayerEquipment equipment;
        public StorageWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
            Loaded += StorageWindow_Loaded;
        }

        private void StorageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            storage = new PlayerStorage();
            equipment = new PlayerEquipment();

            storage = StorageModel.GetPlayerStorage(player, storage);
            equipment = EquipmentModel.GetPlayerEquipment(player, equipment);

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
            if ((Convert.ToInt32(labelSwordAmount1.Content) * (Convert.ToInt32(SwordsToCraft.Text)) <= storage.PlayerCopper) && (Convert.ToInt32(labelSwordAmount2.Content) * (Convert.ToInt32(SwordsToCraft.Text)) <= storage.PlayerIron))
            {
                storage.PlayerCopper -= (Convert.ToInt32(labelSwordAmount1.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
                storage.PlayerIron -= (Convert.ToInt32(labelSwordAmount2.Content) * (Convert.ToInt32(SwordsToCraft.Text)));

                equipment.PlayerSword += Convert.ToInt32(SwordsToCraft.Text);
                StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelSwordAmount.Content = Convert.ToInt32(labelSwordAmount.Content) + Convert.ToInt32(SwordsToCraft.Text);
                labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) - (Convert.ToInt32(labelSwordAmount1.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - (Convert.ToInt32(labelSwordAmount2.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void craftArmor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Convert.ToInt32(labelArmorAmount1.Content) * (Convert.ToInt32(ArmorToCraft.Text)) <= storage.PlayerLeather) && (Convert.ToInt32(labelArmorAmount2.Content) * (Convert.ToInt32(ArmorToCraft.Text)) <= storage.PlayerIron))
            {
                storage.PlayerLeather -= (Convert.ToInt32(labelArmorAmount1.Content) * (Convert.ToInt32(ArmorToCraft.Text)));
                storage.PlayerIron -= (Convert.ToInt32(labelArmorAmount2.Content) * (Convert.ToInt32(ArmorToCraft.Text)));

                equipment.PlayerArmor += Convert.ToInt32(ArmorToCraft.Text);
                StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelArmorAmount.Content = Convert.ToInt32(labelArmorAmount.Content) + Convert.ToInt32(ArmorToCraft.Text);
                labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) - (Convert.ToInt32(labelArmorAmount1.Content) * (Convert.ToInt32(ArmorToCraft.Text)));
                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - (Convert.ToInt32(labelArmorAmount2.Content) * (Convert.ToInt32(ArmorToCraft.Text)));
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void craftHarness_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Convert.ToInt32(labelHarnessAmount1.Content) * (Convert.ToInt32(HarnessToCraft.Text)) <= storage.PlayerLeather) && (Convert.ToInt32(labelHarnessAmount2.Content) * (Convert.ToInt32(HarnessToCraft.Text)) <= storage.PlayerCopper))
            {
                storage.PlayerLeather -= (Convert.ToInt32(labelHarnessAmount1.Content) * (Convert.ToInt32(HarnessToCraft.Text)));
                storage.PlayerCopper -= (Convert.ToInt32(labelHarnessAmount2.Content) * (Convert.ToInt32(HarnessToCraft.Text)));

                equipment.PlayerHarness += Convert.ToInt32(HarnessToCraft.Text);
                StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelHarnessAmount.Content = Convert.ToInt32(labelHarnessAmount.Content) + Convert.ToInt32(HarnessToCraft.Text);
                labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) - (Convert.ToInt32(labelHarnessAmount1.Content) * (Convert.ToInt32(HarnessToCraft.Text)));
                labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) - (Convert.ToInt32(labelHarnessAmount2.Content) * (Convert.ToInt32(HarnessToCraft.Text)));
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void craftSpear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Convert.ToInt32(labelSpearAmount1.Content) * (Convert.ToInt32(SpearToCraft.Text)) <= storage.PlayerWood) && (Convert.ToInt32(labelSpearAmount2.Content) * (Convert.ToInt32(SpearToCraft.Text)) <= storage.PlayerIron))
            {
                storage.PlayerWood -= (Convert.ToInt32(labelSpearAmount1.Content) * (Convert.ToInt32(SpearToCraft.Text)));
                storage.PlayerIron -= (Convert.ToInt32(labelSpearAmount2.Content) * (Convert.ToInt32(SpearToCraft.Text)));

                equipment.PlayerSpear += Convert.ToInt32(SpearToCraft.Text);
                StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelSpearAmount.Content = Convert.ToInt32(labelSpearAmount.Content) + Convert.ToInt32(SpearToCraft.Text);
                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - (Convert.ToInt32(labelSpearAmount1.Content) * (Convert.ToInt32(SpearToCraft.Text)));
                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - (Convert.ToInt32(labelSpearAmount2.Content) * (Convert.ToInt32(SpearToCraft.Text)));
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void craftBow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Convert.ToInt32(labelBowAmount1.Content) * (Convert.ToInt32(BowToCraft.Text)) <= storage.PlayerWood) && (Convert.ToInt32(labelBowAmount2.Content) * (Convert.ToInt32(BowToCraft.Text)) <= storage.PlayerLeather))
            {
                storage.PlayerWood -= (Convert.ToInt32(labelBowAmount1.Content) * (Convert.ToInt32(BowToCraft.Text)));
                storage.PlayerLeather -= (Convert.ToInt32(labelBowAmount2.Content) * (Convert.ToInt32(BowToCraft.Text)));

                equipment.PlayerBow += Convert.ToInt32(BowToCraft.Text);
                StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelBowAmount.Content = Convert.ToInt32(labelBowAmount.Content) + Convert.ToInt32(BowToCraft.Text);
                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - (Convert.ToInt32(labelBowAmount1.Content) * (Convert.ToInt32(BowToCraft.Text)));
                labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) - (Convert.ToInt32(labelBowAmount2.Content) * (Convert.ToInt32(BowToCraft.Text)));
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void craftGear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Convert.ToInt32(labelGearAmount1.Content) * (Convert.ToInt32(GearToCraft.Text)) <= storage.PlayerWood) && (Convert.ToInt32(labelGearAmount2.Content) * (Convert.ToInt32(GearToCraft.Text)) <= storage.PlayerIron))
            {
                storage.PlayerWood -= (Convert.ToInt32(labelSpearAmount1.Content) * (Convert.ToInt32(GearToCraft.Text)));
                storage.PlayerIron -= (Convert.ToInt32(labelSpearAmount2.Content) * (Convert.ToInt32(GearToCraft.Text)));

                equipment.PlayerGear += Convert.ToInt32(GearToCraft.Text);
                StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelGearAmount.Content = Convert.ToInt32(labelGearAmount.Content) + Convert.ToInt32(GearToCraft.Text);
                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - (Convert.ToInt32(labelGearAmount1.Content) * (Convert.ToInt32(GearToCraft.Text)));
                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - (Convert.ToInt32(labelGearAmount2.Content) * (Convert.ToInt32(GearToCraft.Text)));
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void ArmorToCraft_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

