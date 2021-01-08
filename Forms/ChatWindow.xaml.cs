using LandConquest.Entities;
using LandConquest.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace LandConquest.Forms
{
    public partial class ChatWindow : Window
    {
        SqlConnection connection;
        Player player;
        ChatModel chatModel;
        List<ChatMessages> messages;
        SqlTableDependency<ChatMessages> sqlTableDependency;

        public ChatWindow(Player _player, SqlConnection _connection)
        {
            InitializeComponent();
            player = _player;
            connection = _connection;

            var gridView = new GridView();
            this.listViewChat.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Width = 50,
                DisplayMemberBinding = new Binding("PlayerName")

            });
            gridView.Columns.Add(new GridViewColumn
            {
                Width = 600,
                DisplayMemberBinding = new Binding("PlayerMessage")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Width = 150,
                DisplayMemberBinding = new Binding("MessageTime")
            });
            Loaded += listViewChat_Loaded;
        }

        private void buttonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            chatModel.SendMessage(textBoxNewMessage.Text, connection, player.PlayerName);
        }

        private void listViewChat_Loaded(object sender, RoutedEventArgs e)
        {
            chatModel = new ChatModel();
            updateChat();

            var mapper = new ModelToTableMapper<ChatMessages>();
            mapper.AddMapping(c => c.PlayerName, "player_name");
            mapper.AddMapping(c => c.PlayerMessage, "player_message");
            mapper.AddMapping(c => c.MessageTime, "message_sent_time");


            // Не удалять! Если перестанет работать чат, обязательно сделать этот запрос к бд: 
            // ALTER DATABASE LandConquestDB SET ENABLE_BROKER with rollback immediate
            // Если не помогло то те что ниже
            //CREATE QUEUE ContactChangeMessages;
            //CREATE SERVICE ContactChangeNotifications
            //  ON QUEUE ContactChangeMessages
            //([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);  
            //ALTER AUTHORIZATION ON DATABASE:: LandCoqnuestDB TO имя_компа
            try
            {
                sqlTableDependency = new SqlTableDependency<ChatMessages>(connection.ConnectionString, "ChatMessages", "dbo", mapper);
                sqlTableDependency.OnChanged += Changed;
                sqlTableDependency.Start();
            }
            catch (TableDependency.SqlClient.Exceptions.ServiceBrokerNotEnabledException)
            {
                ChatModel.EnableBroker(connection);         //УБРАТЬ ЭТО ПЕРЕД АЛЬФА ТЕСТОМ, ВЫНЕСТИ В ОТДЕЛЬНОЕ АДМИН-ПРИЛОЖЕНИЕ
            }


        }

        public void Changed(object sender, RecordChangedEventArgs<ChatMessages> e)
        {
            var changedEntity = e.Entity;

            Debug.WriteLine("DML operation: " + e.ChangeType);
            Debug.WriteLine("PlayerMessage: " + changedEntity.PlayerMessage);
            Debug.WriteLine("PlayerName: " + changedEntity.PlayerName);
            Debug.WriteLine("MessageTime: " + changedEntity.MessageTime);

            Dispatcher.BeginInvoke(new ThreadStart(delegate { updateChat(); }));

        }

        private void updateChat()
        {
            messages = chatModel.GetMessages(connection);
            listViewChat.ItemsSource = messages;
            listViewChat.Items.Refresh();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            sqlTableDependency.Stop();
            this.Close();
        }
    }
}
