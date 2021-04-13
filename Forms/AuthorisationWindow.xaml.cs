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
            LauncherLogic.DisableActiveCheatsAsync();
            LauncherLogic.CheckLocalUtcDateTime();
            LandConquestDB.DbContext.OpenConnectionPool();
            CheckVersion();
            currentOnlineLabel.Content = YDContext.CountConnections();
            textBoxLogin.Text = Properties.Settings.Default.UserLogin;
            textBoxPass.Password = Properties.Settings.Default.UserPassword;
        }


        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            
            LandConquestDB.Entities.User user = LandConquestDB.Models.UserModel.UserAuthorisation(this.textBoxLogin.Text, YDCrypto.SHA512(this.textBoxPass.Password));

            if (user.UserLogin == textBoxLogin.Text && user.UserPass == YDCrypto.SHA512(textBoxPass.Password))
            {
                LauncherLogic.CallSplashScreen();

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

        private void ButtonRegistrate_Click(object sender, RoutedEventArgs e)
        {
            textBoxNewLogin.Text.Replace(" ", "");
            textBoxNewEmail.Text.Replace(" ", "");
            textBoxNewPass.Text.Replace(" ", "");
            textBoxConfirmNewPass.Text.Replace(" ", "");
            bool validNewUserLogin = LandConquestDB.Models.UserModel.ValidateUserByLogin(textBoxNewLogin.Text);
            bool validNewUserEmail = LandConquestDB.Models.UserModel.ValidateUserByEmail(YDCrypto.Encrypt(textBoxNewEmail.Text));

            if (textBoxNewLogin.Text.Length > 6 &&
                textBoxNewLogin.Text.Any(x => char.IsLetter(x)) &&
                EmailValidator.Validate(textBoxNewEmail.Text, true, true) &&
                textBoxNewPass.Text.Length > 6 &&
                textBoxNewPass.Text.Any(x => char.IsLetter(x)) &&
                validNewUserLogin == true &&
                validNewUserEmail == true &&
                textBoxNewPass.Text == textBoxConfirmNewPass.Text)
            {
                string userId = GenerateUserId();
                int userCreationResult = LandConquestDB.Models.UserModel.CreateUser(this.textBoxNewLogin.Text, YDCrypto.Encrypt(this.textBoxNewEmail.Text), YDCrypto.SHA512(this.textBoxNewPass.Text), userId);
                if (userCreationResult < 0)
                {
                    WarningDialogWindow.CallWarningDialogNoResult("Error creating new user!");
                }
                else
                {
                    LandConquestDB.Entities.User registeredUser = new LandConquestDB.Entities.User();
                    int playerResult = LandConquestDB.Models.PlayerModel.CreatePlayer(this.textBoxNewLogin.Text, YDCrypto.Encrypt(this.textBoxNewEmail.Text), YDCrypto.SHA512(this.textBoxNewPass.Text), userId, registeredUser);

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
                    army.ArmyId = GenerateUserId();
                    LandConquestDB.Models.ArmyModel.InsertArmyFromReg(army);
                }
            }
            else
            {
                textBoxConfirmNewPass.Text = "";
                WarningDialogWindow.CallWarningDialogNoResult("Your login and email should be unique. Password and login should contain letters and be at least 6 characters long.");
            }
        }

        private static Random random;
        public static string GenerateUserId()
        {
            Thread.Sleep(15);
            random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void ButtonShowRegistration_Click(object sender, RoutedEventArgs e)
        {
            ShowRegistrationFields(Visibility.Visible);
            buttonShowRegistration.Visibility = Visibility.Hidden;
        }

        private void ButtonCancelRegistrate_Click(object sender, RoutedEventArgs e)
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

        private void IconDownload_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LauncherLogic.DownloadGame();
        }

        private void LabelAgreement_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UserAgreementDialog.ShowUserAgreement();
        }
    }
}


