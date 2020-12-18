using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Threading;
using LandConquest.Models;
using LandConquest.Entities;
using LandConquest.Forms;
using LandConquest.DialogWIndows;

namespace LandConquest
{

    public partial class AuthorisationWindow : Window
    {
        SqlConnection connection;
        User user;
        UserModel userModel;
        PlayerModel playerModel;
        TaxesModel taxesModel;
        ArmyModel armyModel;
        WarningDialogWindow warningWindow;

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

            user = userModel.UserAuthorisation(this, connection);

            if (user.UserLogin == textBoxLogin.Text && user.UserPass == textBoxPass.Password)
            {
                MainWindow mainWindow = new MainWindow(connection, user);
                mainWindow.Show();

                if (CheckboxRemember.IsChecked == true)
                {
                    Properties.Settings.Default.UserLogin = textBoxLogin.Text;
                    Properties.Settings.Default.UserPassword = textBoxPass.Password;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.UserLogin = null;
                    Properties.Settings.Default.UserPassword = null;
                    Properties.Settings.Default.Save();
                }
                this.Close();
            }
            else
            {
                WarningDialogWindow warningWindow = new WarningDialogWindow("No account with this username/password combination was found!");
                warningWindow.Show();
            }
        }

        private void AuthorisationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //greendend
            //connection = new SqlConnection(@"Data Source=DESKTOP-3S39QPO\SQLEXPRESS;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
            //connection = new SqlConnection(@"Data Source=SMA-NB\SQLEXPRESS;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
            //user-pass 
            connection = new SqlConnection(@"Data Source=DESKTOP-EQUN2R7;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
            //glandeil
            //connection = new SqlConnection(@"Data Source=DESKTOP-P19BATV\SQLEXPRESS;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
            //online connection link
            //connection = new SqlConnection(@"workstation id=LandConquest1.mssql.somee.com;packet size=4096;user id=LandConquest_SQLLogin_1;pwd=3xlofdewbj;data source=LandConquest1.mssql.somee.com;persist security info=False;initial catalog=LandConquest1");

            connection.Open();

            textBoxLogin.Text = Properties.Settings.Default.UserLogin;
            textBoxPass.Password = Properties.Settings.Default.UserPassword;
        }

        private void buttonRegistrate_Click(object sender, RoutedEventArgs e)
        {
            userModel = new UserModel();
            playerModel = new PlayerModel();
            taxesModel = new TaxesModel();
            armyModel = new ArmyModel();

            bool validNewUserLogin = userModel.ValidateUserByLogin(textBoxNewLogin.Text, connection);
            bool validNewUserEmail = userModel.ValidateUserByEmail(textBoxNewEmail.Text, connection);

            if (textBoxNewLogin.Text.Length > 3 &&
                textBoxNewEmail.Text.Length > 3 &&
                textBoxNewPass.Text.Length > 3 &&
                validNewUserLogin == true &&
                validNewUserEmail == true &&
                textBoxNewPass.Text == textBoxConfirmNewPass.Text)
            {
                String userId = generateUserId();
                int userCreationResult = userModel.CreateUser(this, connection, userId);
                if (userCreationResult < 0)
                {
                    Console.WriteLine("Error creating new user!");
                    warningWindow = new WarningDialogWindow("Error creating new user!");
                    warningWindow.Show();
                }
                else
                {
                    User registeredUser = new User();
                    int playerResult = playerModel.CreatePlayer(this, connection, userId, registeredUser);

                    if (playerResult < 0)
                    {
                        Console.WriteLine("Error creating new player!");
                        warningWindow = new WarningDialogWindow("Error creating new user!");
                        warningWindow.Show();
                    }
                    else
                    {
                        playerModel.CreatePlayerResources(this, connection, userId, registeredUser);
                        taxesModel.CreateTaxesData(connection, userId);

                        MainWindow mainWindow = new MainWindow(connection, registeredUser);
                        mainWindow.Show();
                        this.Close();
                    }
                    Army army = new Army();
                    army.PlayerId = userId;
                    army.ArmyId = generateUserId();
                    armyModel.InsertArmyFromReg(connection, army);
                }
            }
            else
            {
                warningWindow = new WarningDialogWindow("Your login and email should be unique. Password, login and email length should be more then 3.");
                warningWindow.Show();
            }
        }


        private static Random random;
        public static string generateUserId()
        {
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


