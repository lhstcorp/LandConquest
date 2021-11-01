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
using WPFLocalizeExtension.Engine;

namespace LandConquest
{
    public partial class AuthorisationWindow : Window
    {
        public AuthorisationWindow()
        {
            InitializeComponent();
            Loaded += AuthorisationWindow_Loaded;
            ShowRegistrationFields(Visibility.Hidden);
            ClearAllValidationNotifications();           
        }

        private void AuthorisationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            YDContext.OpenYD();
            //LauncherLogic.DisableActiveCheatsAsync();
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
                    Properties.Settings.Default.UserLogin       = textBoxLogin.Text;
                    Properties.Settings.Default.UserPassword    = textBoxPass.Password;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.UserLogin       = "";
                    Properties.Settings.Default.UserPassword    = "";
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
            string newLogin         = textBoxNewLogin.Text.Replace(" ", "");
            string newEmail         = textBoxNewEmail.Text.Replace(" ", "");
            string newPass          = textBoxNewPass.Text.Replace(" ", "");

            string confirmNewPass   = textBoxConfirmNewPass.Text.Replace(" ", "");

            bool validNewUserLogin  = LandConquestDB.Models.UserModel.CheckLoginExistence(newLogin);
            if (!validNewUserLogin)
            {
                WarningDialogWindow.CallWarningDialogNoResult("User with this login already registered!");
                textBoxNewLogin.Text = "";
                return;
            }
            bool validNewUserEmail  = LandConquestDB.Models.UserModel.CheckEmailExistence(YDCrypto.Encrypt(newEmail));
            if (validNewUserEmail)
            {
                WarningDialogWindow.CallWarningDialogNoResult("User with this email already registered!");
                textBoxNewEmail.Text = "";
                return;
            }

            if (newLogin.Length >= 6                            &&
                newLogin.Any(x => char.IsLetter(x))             &&
                EmailValidator.Validate(newEmail, true, true)   &&
                newPass.Length >= 6                             &&
                newPass.Any(x => char.IsLetter(x))              &&
                newLogin != newPass                             &&
                newPass == confirmNewPass)
            {
                string userId = GenerateUserId();
                int userCreationResult = LandConquestDB.Models.UserModel.CreateUser(newLogin, YDCrypto.Encrypt(newEmail), YDCrypto.SHA512(newPass), userId);
                if (userCreationResult < 0)
                {
                    WarningDialogWindow.CallWarningDialogNoResult("Error creating new user!");
                }
                else
                {
                    LandConquestDB.Entities.User registeredUser = new LandConquestDB.Entities.User();
                    int playerResult = LandConquestDB.Models.PlayerModel.CreatePlayer(newLogin, YDCrypto.Encrypt(newEmail), YDCrypto.SHA512(newPass), userId, registeredUser);

                    if (playerResult < 0)
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Error creating new player!");
                    }
                    else
                    {
                        LandConquestDB.Models.PlayerModel.CreatePlayerResources(userId, registeredUser);
                        LandConquestDB.Models.TaxesModel.CreateTaxesData(userId);

                        /*MainWindow mainWindow = new MainWindow(registeredUser);
                        mainWindow.Show();
                        this.Close();*/

                        CreatePersonDialogWindow dialogWindow = new CreatePersonDialogWindow(registeredUser);
                        dialogWindow.Show();
                        this.Close();

                        //CreatePersonDialogWindow window = new CreatePersonDialogWindow(registeredUser);
                        //window.Show();
                        //this.Close();  Помещайте плиз в комменты недопиленный функционал, чтобы ошибок не выскакивало 
                    }
                    LandConquestDB.Entities.Army army = new LandConquestDB.Entities.Army();
                    army.PlayerId = userId;
                    army.ArmyId = GenerateUserId();
                    LandConquestDB.Models.ArmyModel.InsertArmyFromReg(army);
                }
            }
            else
            {
                confirmNewPass = "";
                WarningDialogWindow.CallWarningDialogNoResult("Password and login should contain letters, not match and be at least 6 characters long.");
            }
            ///метод для открытия нового окна
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
            ClearAllValidationNotifications();
            textBoxNewLogin.Text        = "";
            textBoxNewEmail.Text        = "";
            textBoxNewPass.Text         = "";
            textBoxConfirmNewPass.Text  = "";
        }
        private void ShowRegistrationFields(Visibility visibility)
        {
            textBoxNewLogin.Visibility          = visibility;
            textBoxNewEmail.Visibility          = visibility;
            textBoxNewPass.Visibility           = visibility;
            textBoxConfirmNewPass.Visibility    = visibility;
            registerGui.Visibility              = visibility;
            buttonRegistrate.Visibility         = visibility;
            buttonCancelRegistrate.Visibility   = visibility;
            labelAgreement.Visibility           = visibility;
        }

        private void CheckVersion()
        {
            if (LauncherLogic.CheckGameVersion().SequenceEqual(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion))
            {
                labelGameFiles.Content      = "Game is up to date";
                labelGameFiles.Foreground   = System.Windows.Media.Brushes.GreenYellow;
                iconDownload.Visibility     = Visibility.Hidden;
            }
            else
            {
                labelGameFiles.Content      = "Update required";
                labelGameFiles.Foreground   = System.Windows.Media.Brushes.Red;
                iconDownload.Visibility     = Visibility.Visible;
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

        private void textBoxNewLogin_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            string newLogin = textBoxNewLogin.Text.Replace(" ", "");

            if (newLogin.Length < 6)
            {
                labelNameValid.Content      = "Your login is too short!";
                labelNameValid.Foreground   = System.Windows.Media.Brushes.Red;
            } 
            else if (!newLogin.Any(x => char.IsLetter(x)))
            {
                labelNameValid.Content      = "Your login must contain letters!";
                labelNameValid.Foreground   = System.Windows.Media.Brushes.Red;
            }
            else
            {
                labelNameValid.Content      = "✓";
                labelNameValid.Foreground   = System.Windows.Media.Brushes.GreenYellow;
            }

        }

        private void textBoxNewEmail_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            string newEmail = textBoxNewEmail.Text.Replace(" ", "");

            if(!EmailValidator.Validate(newEmail, true, true))
            {
                labelEmailValid.Content     = "Your email is incorrect!";
                labelEmailValid.Foreground  = System.Windows.Media.Brushes.Red;
            }
            else
            {
                labelEmailValid.Content     = "✓";
                labelEmailValid.Foreground  = System.Windows.Media.Brushes.GreenYellow;
            }
        }

        private void textBoxNewPass_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            string newPass = textBoxNewPass.Text.Replace(" ", "");

            if (newPass.Length < 6)
            {
                labelPassValid.Content      = "Your password is too short!";
                labelPassValid.Foreground   = System.Windows.Media.Brushes.Red;
            }
            else if (!newPass.Any(x => char.IsLetter(x)))
            {
                labelPassValid.Content      = "Your password must contain letters!";
                labelPassValid.Foreground   = System.Windows.Media.Brushes.Red;
            }
            else if (newPass == textBoxNewLogin.Text)
            {
                labelPassValid.Content      = "Your password is the same as your login!";
                labelPassValid.Foreground   = System.Windows.Media.Brushes.Red;
            }
            else
            {
                labelPassValid.Content      = "✓";
                labelPassValid.Foreground   = System.Windows.Media.Brushes.GreenYellow;
            }
        }

        private void textBoxConfirmNewPass_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (textBoxNewPass.Text != textBoxConfirmNewPass.Text)
            {
                labelConfirmPassValid.Content = "Passwords do not match!";
                labelConfirmPassValid.Foreground = System.Windows.Media.Brushes.Red;
            }
            else
            {
                labelConfirmPassValid.Content       = "✓";
                labelConfirmPassValid.Foreground    = System.Windows.Media.Brushes.GreenYellow;
            }
        }

        private void ClearAllValidationNotifications()
        {
            labelNameValid.Content          = "";
            labelEmailValid.Content         = "";
            labelPassValid.Content          = "";
            labelConfirmPassValid.Content   = "";
        }
    }
}


