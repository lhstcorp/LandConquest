using LandConquest.DialogWIndows;
using LandConquest.Entities;
using LandConquest.Forms;
using LandConquest.Launcher;
using LandConquest.Models;
using Syroot.Windows.IO;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace LandConquest
{

    public partial class AuthorisationWindow : System.Windows.Window
    {
        User user;
        WarningDialogWindow warningWindow;

        public AuthorisationWindow()
        {
            InitializeComponent();
            Loaded += AuthorisationWindow_Loaded;
            ShowRegistrationFields(Visibility.Hidden);
        }

        private void AuthorisationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DbContext.OpenConnectionPool();
            LauncherController.CheckLocalUtcDateTime();
            LauncherController.DisableActiveCheats();
            CheckVersion();
            textBoxLogin.Text = Properties.Settings.Default.UserLogin;
            textBoxPass.Password = Properties.Settings.Default.UserPassword;
            iconDownload.Visibility = Visibility.Hidden;
        }


        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            user = new User();

            user = UserModel.UserAuthorisation(this);

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

        private void buttonRegistrate_Click(object sender, RoutedEventArgs e)
        {
            bool validNewUserLogin = UserModel.ValidateUserByLogin(textBoxNewLogin.Text);
            bool validNewUserEmail = UserModel.ValidateUserByEmail(textBoxNewEmail.Text);

            if (textBoxNewLogin.Text.Length > 6 &&
                textBoxNewEmail.Text.Length > 6 &&
                textBoxNewEmail.Text.Contains("@") &&
                textBoxNewPass.Text.Length > 6 &&
                validNewUserLogin == true &&
                validNewUserEmail == true &&
                textBoxNewPass.Text == textBoxConfirmNewPass.Text)
            {
                String userId = generateUserId();
                int userCreationResult = UserModel.CreateUser(this, userId);
                if (userCreationResult < 0)
                {
                    Console.WriteLine("Error creating new user!");
                    warningWindow = new WarningDialogWindow("Error creating new user!");
                    warningWindow.Show();
                }
                else
                {
                    User registeredUser = new User();
                    int playerResult = PlayerModel.CreatePlayer(this, userId, registeredUser);

                    if (playerResult < 0)
                    {
                        Console.WriteLine("Error creating new player!");
                        warningWindow = new WarningDialogWindow("Error creating new user!");
                        warningWindow.Show();
                    }
                    else
                    {
                        PlayerModel.CreatePlayerResources(this, userId, registeredUser);
                        TaxesModel.CreateTaxesData(userId);

                        MainWindow mainWindow = new MainWindow(registeredUser);
                        mainWindow.Show();
                        this.Close();
                    }
                    Army army = new Army();
                    army.PlayerId = userId;
                    army.ArmyId = generateUserId();
                    ArmyModel.InsertArmyFromReg(army);
                }
            }
            else
            {
                textBoxNewLogin.Text = "";
                textBoxNewEmail.Text = "";
                textBoxNewPass.Text = "";
                textBoxConfirmNewPass.Text = "";
                warningWindow = new WarningDialogWindow("Your login and email should be unique. Password, login and email length should be more then 6.");
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

        private async void CheckVersion()
        {
            await LauncherController.CheckGameVersion();
            string downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;
            if (File.ReadAllText(downloadsPath + @"\GameVersion.txt").SequenceEqual(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion))
            {
                File.Delete(downloadsPath + @"\GameVersion.txt");
                labelGameFiles.Content = "Game is up to date";
                labelGameFiles.Foreground = System.Windows.Media.Brushes.GreenYellow;
                labelGameFiles.Margin = new Thickness(520, 427, 0, 0);
                iconSpinner.Spin = false;
                iconSpinner.Foreground = System.Windows.Media.Brushes.GreenYellow;
            }
            else
            {
                File.Delete(downloadsPath + @"\GameVersion.txt");
                labelGameFiles.Content = "Update required";
                labelGameFiles.Foreground = System.Windows.Media.Brushes.Red;
                labelGameFiles.Margin = new Thickness(550, 427, 0, 0);
                iconSpinner.Spin = false;
                iconSpinner.Foreground = System.Windows.Media.Brushes.Red;
                iconDownload.Visibility = Visibility.Visible;
                iconDownload.Margin = new Thickness(300, 427, 40, 9);
            }
        }

        private void iconDownload_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var err = LauncherController.DownloadGame();
            WarningDialogWindow window;
            if (err.Message != null)
            {
                window = new WarningDialogWindow(err.Message);
            }
            else
            {
                window = new WarningDialogWindow("New version is successfully downloaded!");
            }
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
            if (window.DialogResult.HasValue)
            {
                Environment.Exit(0);
            }
        }

    }
}


