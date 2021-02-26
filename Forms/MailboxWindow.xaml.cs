using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestYD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class MailboxWindow : Window
    {
        private Player player;
        private List<string> messages;
        public MailboxWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCreateNewDialog_Click(object sender, RoutedEventArgs e)
        {
            EnterTextDialogWindow inputDialog = new EnterTextDialogWindow("Enter player name:");
            if (inputDialog.ShowDialog() == true)
            {
                if(inputDialog.ValueText != null)
                {
                    if (!LandConquestDB.Models.UserModel.ValidateUserByLogin(inputDialog.ValueText))
                    {
                        YDContext.CreateDialog(player.PlayerName, inputDialog.ValueText);
                    }
                }
            }
        }
        
        private void RefreshListItems()
        {
            ListViewDialogs.ItemsSource = YDContext.CheckForMessages(player.PlayerName);
            ListViewDialogs.Items.Refresh();
        }

        private void ButtonRefreshDialog_Click(object sender, RoutedEventArgs e)
        {
            RefreshListItems();
        }

        private void ListViewDialogs_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshListItems();
        }
    }
}
