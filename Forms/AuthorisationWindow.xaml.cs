using EmailValidation;
using LandConquest.DialogWIndows;
using LandConquest.Forms;
using LandConquest.Logic;
using LandConquestYD;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace LandConquest
{
    public partial class AuthorisationWindow : Window
    {
        public AuthorisationWindow()
        {
            InitializeComponent();
            Loaded += AuthorisationWindow_Loaded;
            ShowRegistrationFields(Visibility.Hidden);
        }

        private void AuthorisationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            YDContext.OpenYD();
            LandConquestDB.DbContext.OpenConnectionPool();
            //LauncherLogic.CheckLocalUtcDateTime();
            //LauncherLogic.DisableActiveCheats();
            CheckVersion();
            currentOnlineLabel.Content = YDContext.CountConnections();
            textBoxLogin.Text = Properties.Settings.Default.UserLogin;
            textBoxPass.Password = Properties.Settings.Default.UserPassword;
        }


        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            LandConquestDB.Entities.User user = LandConquestDB.Models.UserModel.UserAuthorisation(this.textBoxLogin.Text, this.textBoxPass.Password);

            if (user.UserLogin == textBoxLogin.Text && user.UserPass == textBoxPass.Password)
            {
                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();

                if (CheckboxRemember.IsChecked == true)
                {
                    Properties.Settings.Default.UserLogin = textBoxLogin.Text;
                    Properties.Settings.Default.UserPassword = textBoxPass.Password;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.UserLogin = "";
                    Properties.Settings.Default.UserPassword = "";
                    Properties.Settings.Default.Save();
                }
                this.Close();
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("No account with this username/password combination was found!");
            }
        }

        private void buttonRegistrate_Click(object sender, RoutedEventArgs e)
        {
            bool validNewUserLogin = LandConquestDB.Models.UserModel.ValidateUserByLogin(textBoxNewLogin.Text);
            bool validNewUserEmail = LandConquestDB.Models.UserModel.ValidateUserByEmail(textBoxNewEmail.Text);

            if (textBoxNewLogin.Text.Length > 6 &&
                EmailValidator.Validate(textBoxNewEmail.Text, true, true) &&
                textBoxNewPass.Text.Length > 6 &&
                validNewUserLogin == true &&
                validNewUserEmail == true &&
                textBoxNewPass.Text == textBoxConfirmNewPass.Text)
            {
                string userId = generateUserId();
                int userCreationResult = LandConquestDB.Models.UserModel.CreateUser(this.textBoxNewLogin.Text, this.textBoxNewEmail.Text, this.textBoxNewPass.Text, userId);
                if (userCreationResult < 0)
                {
                    WarningDialogWindow.CallWarningDialogNoResult("Error creating new user!");
                }
                else
                {
                    LandConquestDB.Entities.User registeredUser = new LandConquestDB.Entities.User();
                    int playerResult = LandConquestDB.Models.PlayerModel.CreatePlayer(this.textBoxNewLogin.Text, this.textBoxNewEmail.Text, this.textBoxNewPass.Text, userId, registeredUser);

                    if (playerResult < 0)
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Error creating new player!");
                    }
                    else
                    {
                        LandConquestDB.Models.PlayerModel.CreatePlayerResources(userId, registeredUser);
                        LandConquestDB.Models.TaxesModel.CreateTaxesData(userId);

                        MainWindow mainWindow = new MainWindow(registeredUser);
                        mainWindow.Show();
                        this.Close();
                    }
                    LandConquestDB.Entities.Army army = new LandConquestDB.Entities.Army();
                    army.PlayerId = userId;
                    army.ArmyId = generateUserId();
                    LandConquestDB.Models.ArmyModel.InsertArmyFromReg(army);
                }
            }
            else
            {
                textBoxNewLogin.Text = "";
                textBoxNewEmail.Text = "";
                textBoxNewPass.Text = "";
                textBoxConfirmNewPass.Text = "";
                WarningDialogWindow.CallWarningDialogNoResult("Your login and email should be unique. Password and login length should be more than 6.");
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
            labelAgreement.Visibility = visibility;
        }

        private void CheckVersion()
        {
            if (LauncherLogic.CheckGameVersion().SequenceEqual(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion))
            {
                labelGameFiles.Content = "Game is up to date";
                labelGameFiles.Foreground = System.Windows.Media.Brushes.GreenYellow;
                iconDownload.Visibility = Visibility.Hidden;
            }
            else
            {
                labelGameFiles.Content = "Update required";
                labelGameFiles.Foreground = System.Windows.Media.Brushes.Red;
                iconDownload.Visibility = Visibility.Visible;
            }
        }

        private void iconDownload_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LauncherLogic.DownloadGame();
        }

        private void labelAgreement_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UserAgreementDialog.ShowUserAgreement();
        }
    }
}


