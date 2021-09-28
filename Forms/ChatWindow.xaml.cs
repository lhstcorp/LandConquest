using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class ChatWindow : Window
    {
        private Player player;
        private List<ChatMessages> messages;
        CancellationTokenSource cancelTokenSource;
        CancellationToken token;
        private string playerTargetId;
        private string playerCurrentRoom;

        public ChatWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (playerTargetId == "")
            {
                ChatModel.SendMessage(textBoxNewMessage.Text, player.PlayerId, "[all]", playerCurrentRoom);
            }
            else
            {
                ChatModel.SendMessage(textBoxNewMessage.Text, player.PlayerId, playerTargetId, playerCurrentRoom);
            }
            textBoxNewMessage.Text = "";
            playerTargetId = "";
            lblSendTo.Content = "";
            borderSendTo.Visibility = Visibility.Hidden;
            gridSendTo.Visibility = Visibility.Hidden;
            viewProfile.Visibility = Visibility.Hidden;
            buttonToAll.Visibility = Visibility.Hidden;
        }

        public async void CallUpdateChatAsync()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
            await Task.Run(() => UpdateChat(token));
        }

        private async Task UpdateChat(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Dispatcher.BeginInvoke(new CrossAppDomainDelegate(delegate { messages = ChatModel.GetMessages(player.PlayerId, playerCurrentRoom); listViewChat.ItemsSource = messages; listViewChat.Items.Refresh(); }));
                await Task.Delay(5000);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource.Cancel();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CallUpdateChatAsync();
            playerTargetId = "";
            playerCurrentRoom = "0";
            borderSendTo.Visibility = Visibility.Hidden;
            gridSendTo.Visibility = Visibility.Hidden;
            viewProfile.Visibility = Visibility.Hidden;
            buttonToAll.Visibility = Visibility.Hidden;
        }

        private void listViewChat_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listViewChat.SelectedItem != null)
            {
                ChatMessages message = (ChatMessages)listViewChat.SelectedItem;
                if (player.PlayerId != message.PlayerId)
                {
                    playerTargetId = message.PlayerId;
                    borderSendTo.Visibility = Visibility.Visible;
                    gridSendTo.Visibility = Visibility.Visible;
                    viewProfile.Visibility = Visibility.Visible;
                    buttonToAll.Visibility = Visibility.Visible;
                    lblSendTo.Content = message.PlayerName;
                }
            }
        }

        private void buttonToAll_Click(object sender, RoutedEventArgs e)
        {
            playerTargetId = "";
            borderSendTo.Visibility = Visibility.Hidden;
            gridSendTo.Visibility = Visibility.Hidden;
            viewProfile.Visibility = Visibility.Hidden;
            buttonToAll.Visibility = Visibility.Hidden;

        }

        private void viewProfile_Click(object sender, RoutedEventArgs e)
        {
            if (playerTargetId != "")
            {
                PlayerProfileDialog profileDialog = new PlayerProfileDialog(playerTargetId);
                profileDialog.Owner = Application.Current.MainWindow;
                profileDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                profileDialog.Show();
            }
        }

        private void buttonMainGeneral_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
        }

        private void buttonNewPersonalChat_Click(object sender, RoutedEventArgs e)
        {
            CreateNewPersonalDialog inputDialog = new CreateNewPersonalDialog();
            if (inputDialog.ShowDialog() == true)
            {
                if (inputDialog.ValueText != null)
                {
                    string[] data = inputDialog.ValueText.Split(null, 2);

                    string playerId = data[0];
                    string welcomeText;
                    if (data.Length > 1)
                    {
                        welcomeText = data[1];
                    }
                }
            }
        }
    }
}
