using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Threading;
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
        CancellationTokenSource cancelTokenSource;
        CancellationToken token;

        public ChatWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            ChatModel.SendMessage(textBoxNewMessage.Text, player.PlayerName);
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
                await Dispatcher.BeginInvoke(new CrossAppDomainDelegate(delegate { messages = ChatModel.GetMessages(); listViewChat.ItemsSource = messages; listViewChat.Items.Refresh();}));
                await Task.Delay(1000);
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
        }
    }
}
