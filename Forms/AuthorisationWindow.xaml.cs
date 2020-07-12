using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Threading;
using LandConquest.Models;
using LandConquest.Entities;
using LandConquest.Forms;

namespace LandConquest
{

    public partial class AuthorisationWindow : Window
    {
        SqlConnection connection;
        User user;
        UserModel userModel;
        PlayerModel playerModel;
        TaxesModel taxesModel;

        public AuthorisationWindow()
        {
            InitializeComponent();
            Loaded += AuthorisationWindow_Loaded;
            ShowRegistrationFields(Visibility.Hidden);
            
        }


        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            user = new User();

            userModel = new UserModel();

            user = userModel.UserLogin(this, user, connection);

            if (user.UserLogin == textBoxLogin.Text && user.UserPass == textBoxPass.Password)
            {
                MainWindow mainWindow = new MainWindow(connection, user);
                mainWindow.Show();
                this.Close();
            }
        }

        private void AuthorisationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //greendend
            connection = new SqlConnection(@"Data Source=DESKTOP-3S39QPO\SQLEXPRESS;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
            //user-pass 
            //connection = new SqlConnection(@"Data Source=DESKTOP-EQUN2R7;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
            //glandeil
            //connection = new SqlConnection(@"Data Source=LEXICH\SQLEXPRESS;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
            //online connection link
            //connection = new SqlConnection(@"workstation id=LandConquest.mssql.somee.com;packet size=4096;user id=Steolod_SQLLogin_1;pwd=st9s2yqew9;data source=LandConquest.mssql.somee.com;persist security info=False;initial catalog=LandConquest");

            connection.Open();
        }

        private void buttonRegistrate_Click(object sender, RoutedEventArgs e)
        {
            userModel = new UserModel();
            playerModel = new PlayerModel();
            taxesModel = new TaxesModel();


            String userId = generateUserId();

            int userCreationResult = userModel.CreateUser(this, connection, userId);

            if (userCreationResult < 0)
            {
                Console.WriteLine("Error creating new user!");
            } else
            {
                User registeredUser = new User();
                int playerResult = playerModel.CreatePlayer(this, connection, userId, registeredUser);

                if (playerResult < 0)
                {
                    Console.WriteLine("Error creating new player!");
                }
                else
                {
                    playerModel.CreatePlayerResources(this, connection, userId, registeredUser);
                    taxesModel.CreateTaxesData(connection, userId);
                    MainWindow mainWindow = new MainWindow(connection, registeredUser);
                    mainWindow.Show();
                    this.Close();
                }

            }
        }


        private static Random random;
        public static string generateUserId() {
            Thread.Sleep(15);
            random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());     
        }

        private void buttonShowRegistration_Click(object sender, RoutedEventArgs e)
        {
            ShowRegistrationFields(Visibility.Visible);
            buttonShowRegistration.Visibility = Visibility.Hidden;
        }

        private void buttonCancelRegistrate_Click(object sender, RoutedEventArgs e)
        {
            ShowRegistrationFields(Visibility.Hidden);
            buttonShowRegistration.Visibility = Visibility.Visible;

        }

        private void ShowRegistrationFields(Visibility visibility)
        {
            textBoxNewLogin.Visibility = visibility;
            textBoxNewEmail.Visibility = visibility;
            textBoxNewPass.Visibility = visibility;
            textBoxConfirmNewPass.Visibility = visibility;
            registerGui.Visibility = visibility;
            buttonRegistrate.Visibility = visibility;
            buttonCancelRegistrate.Visibility = visibility;
        }
    }
}


