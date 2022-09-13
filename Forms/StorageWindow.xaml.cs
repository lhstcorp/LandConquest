using LandConquest.DialogWIndows;
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
            
            equipment = new PlayerEquipment();

            //storage = StorageModel.GetPlayerStorage(player);
            equipment = EquipmentModel.GetPlayerEquipment(player, equipment);

            labelWoodAmount.Content = storage.Wood.ToString();
            labelStoneAmount.Content = storage.Stone.ToString();
            labelFoodAmount.Content = storage.Food.ToString();
            labelGemsAmount.Content = storage.Gems.ToString();
            labelCopperAmount.Content = storage.Copper.ToString();
            labelGoldAmount.Content = storage.GoldOre.ToString();
            labelIronAmount.Content = storage.Iron.ToString();
            labelLeatherAmount.Content = storage.Leather.ToString();

            labelHarnessAmount.Content = equipment.PlayerHarness.ToString();
            labelGearAmount.Content = equipment.PlayerGear.ToString();
            labelSpearAmount.Content = equipment.PlayerSword.ToString();
            labelBowAmount.Content = equipment.PlayerBow.ToString();
            labelArmorAmount.Content = equipment.PlayerArmor.ToString();
            labelSwordAmount.Content = equipment.PlayerSpear.ToString();


        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void craftSword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SwordsToCraft.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((Convert.ToInt32(labelSwordAmount1.Content) * (Convert.ToInt32(SwordsToCraft.Text)) <= storage.Copper) && (Convert.ToInt32(labelSwordAmount2.Content) * (Convert.ToInt32(SwordsToCraft.Text)) <= storage.Iron))
            {
                storage.Copper -= (Convert.ToInt32(labelSwordAmount1.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
                storage.Iron -= (Convert.ToInt32(labelSwordAmount2.Content) * (Convert.ToInt32(SwordsToCraft.Text)));

                equipment.PlayerSpear += Convert.ToInt32(SwordsToCraft.Text);
                //StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelSwordAmount.Content = Convert.ToInt32(labelSwordAmount.Content) + Convert.ToInt32(SwordsToCraft.Text);
                labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) - (Convert.ToInt32(labelSwordAmount1.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - (Convert.ToInt32(labelSwordAmount2.Content) * (Convert.ToInt32(SwordsToCraft.Text)));
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
            }
        }

        private void craftArmor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ArmorToCraft.TextDecorations = null;
            if (ArmorToCraft.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((Convert.ToInt32(labelArmorAmount1.Content) * (Convert.ToInt32(ArmorToCraft.Text)) <= storage.Leather) && (Convert.ToInt32(labelArmorAmount2.Content) * (Convert.ToInt32(ArmorToCraft.Text)) <= storage.Iron))
            {
                storage.Leather -= (Convert.ToInt32(labelArmorAmount1.Content) * (Convert.ToInt32(ArmorToCraft.Text)));
                storage.Iron -= (Convert.ToInt32(labelArmorAmount2.Content) * (Convert.ToInt32(ArmorToCraft.Text)));

                equipment.PlayerArmor += Convert.ToInt32(ArmorToCraft.Text);
                //StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelArmorAmount.Content = Convert.ToInt32(labelArmorAmount.Content) + Convert.ToInt32(ArmorToCraft.Text);
                labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) - (Convert.ToInt32(labelArmorAmount1.Content) * (Convert.ToInt32(ArmorToCraft.Text)));
                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - (Convert.ToInt32(labelArmorAmount2.Content) * (Convert.ToInt32(ArmorToCraft.Text)));
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
            }
        }

        private void craftHarness_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (HarnessToCraft.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((Convert.ToInt32(labelHarnessAmount1.Content) * (Convert.ToInt32(HarnessToCraft.Text)) <= storage.Leather) && (Convert.ToInt32(labelHarnessAmount2.Content) * (Convert.ToInt32(HarnessToCraft.Text)) <= storage.Copper))
            {
                storage.Leather -= (Convert.ToInt32(labelHarnessAmount1.Content) * (Convert.ToInt32(HarnessToCraft.Text)));
                storage.Copper -= (Convert.ToInt32(labelHarnessAmount2.Content) * (Convert.ToInt32(HarnessToCraft.Text)));

                equipment.PlayerHarness += Convert.ToInt32(HarnessToCraft.Text);
                //StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelHarnessAmount.Content = Convert.ToInt32(labelHarnessAmount.Content) + Convert.ToInt32(HarnessToCraft.Text);
                labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) - (Convert.ToInt32(labelHarnessAmount1.Content) * (Convert.ToInt32(HarnessToCraft.Text)));
                labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) - (Convert.ToInt32(labelHarnessAmount2.Content) * (Convert.ToInt32(HarnessToCraft.Text)));
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
            }
        }

        private void craftSpear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SpearToCraft.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((Convert.ToInt32(labelSpearAmount1.Content) * (Convert.ToInt32(SpearToCraft.Text)) <= storage.Wood) && (Convert.ToInt32(labelSpearAmount2.Content) * (Convert.ToInt32(SpearToCraft.Text)) <= storage.Iron))
            {
                storage.Wood -= (Convert.ToInt32(labelSpearAmount1.Content) * (Convert.ToInt32(SpearToCraft.Text)));
                storage.Iron -= (Convert.ToInt32(labelSpearAmount2.Content) * (Convert.ToInt32(SpearToCraft.Text)));

                equipment.PlayerSword += Convert.ToInt32(SpearToCraft.Text);
                //StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelSpearAmount.Content = Convert.ToInt32(labelSpearAmount.Content) + Convert.ToInt32(SpearToCraft.Text);
                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - (Convert.ToInt32(labelSpearAmount1.Content) * (Convert.ToInt32(SpearToCraft.Text)));
                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - (Convert.ToInt32(labelSpearAmount2.Content) * (Convert.ToInt32(SpearToCraft.Text)));
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
            }
        }

        private void craftBow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (BowToCraft.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((Convert.ToInt32(labelBowAmount1.Content) * (Convert.ToInt32(BowToCraft.Text)) <= storage.Wood) && (Convert.ToInt32(labelBowAmount2.Content) * (Convert.ToInt32(BowToCraft.Text)) <= storage.Leather))
            {
                storage.Wood -= (Convert.ToInt32(labelBowAmount1.Content) * (Convert.ToInt32(BowToCraft.Text)));
                storage.Leather -= (Convert.ToInt32(labelBowAmount2.Content) * (Convert.ToInt32(BowToCraft.Text)));

                equipment.PlayerBow += Convert.ToInt32(BowToCraft.Text);
                //StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelBowAmount.Content = Convert.ToInt32(labelBowAmount.Content) + Convert.ToInt32(BowToCraft.Text);
                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - (Convert.ToInt32(labelBowAmount1.Content) * (Convert.ToInt32(BowToCraft.Text)));
                labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) - (Convert.ToInt32(labelBowAmount2.Content) * (Convert.ToInt32(BowToCraft.Text)));
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
            }
        }

        private void craftGear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GearToCraft.Text == "")
            {
                WarningDialogWindow.CallWarningDialogNoResult("No data!");
            }
            else if ((Convert.ToInt32(labelGearAmount1.Content) * (Convert.ToInt32(GearToCraft.Text)) <= storage.Wood) && (Convert.ToInt32(labelGearAmount2.Content) * (Convert.ToInt32(GearToCraft.Text)) <= storage.Iron))
            {
                storage.Wood -= (Convert.ToInt32(labelSpearAmount1.Content) * (Convert.ToInt32(GearToCraft.Text)));
                storage.Iron -= (Convert.ToInt32(labelSpearAmount2.Content) * (Convert.ToInt32(GearToCraft.Text)));

                equipment.PlayerGear += Convert.ToInt32(GearToCraft.Text);
                //StorageModel.UpdateStorage(player, storage);
                EquipmentModel.UpdateEquipment(player, equipment);

                labelGearAmount.Content = Convert.ToInt32(labelGearAmount.Content) + Convert.ToInt32(GearToCraft.Text);
                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - (Convert.ToInt32(labelGearAmount1.Content) * (Convert.ToInt32(GearToCraft.Text)));
                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - (Convert.ToInt32(labelGearAmount2.Content) * (Convert.ToInt32(GearToCraft.Text)));
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
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
        private void Space_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}

