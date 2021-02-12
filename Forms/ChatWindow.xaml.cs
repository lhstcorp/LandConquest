using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LandConquest.Forms
{
    public partial class ChatWindow : Window
    {
        private Player player;
        private List<ChatMessages> messages;
        private bool isActive;

        public ChatWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
            isActive = true;
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            ChatModel.SendMessage(textBoxNewMessage.Text, player.PlayerName);
        }

        public async void CallUpdateChatAsync()
        {
            await Task.Run(() => UpdateChat());
        }

        private async Task UpdateChat()
        {
            while (isActive)
            {         
                await Dispatcher.BeginInvoke(new CrossAppDomainDelegate(delegate { messages = ChatModel.GetMessages(); listViewChat.ItemsSource = messages; listViewChat.Items.Refresh();}));
                await Task.Delay(3000);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            isActive = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CallUpdateChatAsync();
        }
    }
}
