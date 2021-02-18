using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
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

namespace LandConquest.Forms
{
    public partial class MailboxWindow : Window
    {
        public MailboxWindow(Player _player)
        {
            InitializeComponent();
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

                    }
                }
            }
        }
    }
}
