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
    /// Логика взаимодействия для PlayerProfileDialog.xaml
    /// </summary>
    public partial class PlayerProfileDialog : Window
    {
        private string playerId;
        public PlayerProfileDialog(string _playerId)
        {
            InitializeComponent();
            playerId = _playerId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Player player = new Player();
            player = PlayerModel.GetPlayerById(playerId);

            labelName.Content = player.PlayerName;
            labelTitle.Content = player.PlayerTitle;
            labelLand.Content = player.PlayerCurrentRegion;
            labelDateVisit.Content = PlayerEntranceModel.GetLastEntrance(player);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
