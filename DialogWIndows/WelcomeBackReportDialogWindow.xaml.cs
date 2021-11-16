using LandConquest.Logic;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LandConquest.DialogWIndows
{
    public partial class WelcomeBackReportDialogWindow : Window
    {
        private Player player;
        public WelcomeBackReportDialogWindow(Player _player)
        {
            InitializeComponent();

            player = _player;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.labelExpOld.Content    = player.PlayerExp;
            this.labelExpNew.Content    = IncomeCalculationLogic.CountPlayerExp(player);
            this.labelHours.Content     = $"{(DateTime.UtcNow - PlayerEntranceModel.GetLastEntrance(player)).Hours} hr(s)" ;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonGrabRes_Click(object sender, RoutedEventArgs e)
        {
            PlayerStorage storage =  StorageModel.GetPlayerStorage(player);
            Taxes taxes = TaxesModel.GetTaxesInfo(player.PlayerId);

            string playerLvl;
            IncomeCalculationLogic.CountResources(player, storage, taxes, out player, out storage, out taxes, out playerLvl);

            this.Close();
        }
    }
}
