//using LandConquest.DialogWIndows;
//using LandConquest.Forms;
//using LandConquest.Logic;
//using LandConquestDB.Entities;
//using LandConquestDB.Models;
//using LandConquestYD;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Security;
//using System.Windows;

//namespace LandConquest.WindowViewModels
//{
//    public class AuthorisationWindowViewModel : INotifyPropertyChanged
//    {
//        public AuthorisationWindowViewModel()
//        {

//        }

//        private string _loginText;
//        public string LoginText
//        {
//            get
//            {
//                return _loginText;
//            }
//            set
//            {
//                _loginText = value;
//                OnPropertyChanged("LoginText");
//            }
//        }

//        public void WindowLoaded()
//        {
//            YDContext.OpenYD();
//            //LauncherLogic.DisableActiveCheatsAsync();
//            LauncherLogic.CheckLocalUtcDateTime();
//            LandConquestDB.DbContext.OpenConnectionPool();
//            CheckVersion();
//            currentOnlineLabel.Content = YDContext.CountConnections();
//            LoginText = Properties.Settings.Default.UserLogin;
//        }


//        public void LoginClick(string password)
//        {

//            User user = UserModel.UserAuthorisation(LoginText, YDCrypto.SHA512(password));

//            if (user.UserLogin == LoginText && user.UserPass == YDCrypto.SHA512(password))
//            {
//                LauncherLogic.CallSplashScreen();

//                MainWindow mainWindow = new MainWindow(user);
//                mainWindow.Show();

//                if (CheckboxRemember.IsChecked == true)
//                {
//                    Properties.Settings.Default.UserLogin = LoginText;
//                    Properties.Settings.Default.UserPassword = password;
//                    Properties.Settings.Default.Save();
//                }
//                else
//                {
//                    Properties.Settings.Default.UserLogin = "";
//                    Properties.Settings.Default.UserPassword = "";
//                    Properties.Settings.Default.Save();
//                }
//                this.Close();
//            }
//            else
//            {
//                WarningDialogWindow.CallWarningDialogNoResult("No account with this username/password combination was found!");
//            }
//        }

//        public void RegistrateClick()
//        {
//            string newLogin = textBoxNewLogin.Text.Replace(" ", "");
//            string newEmail = textBoxNewEmail.Text.Replace(" ", "");
//            string newPass = textBoxNewPass.Text.Replace(" ", "");

//            string confirmNewPass = textBoxConfirmNewPass.Text.Replace(" ", "");

//            bool validNewUserLogin = LandConquestDB.Models.UserModel.CheckLoginExistence(newLogin);
//            if (!validNewUserLogin)
//            {
//                WarningDialogWindow.CallWarningDialogNoResult("User with this login already registered!");
//                textBoxNewLogin.Text = "";
//                return;
//            }
//            bool validNewUserEmail = LandConquestDB.Models.UserModel.CheckEmailExistence(YDCrypto.Encrypt(newEmail));
//            if (validNewUserEmail)
//            {
//                WarningDialogWindow.CallWarningDialogNoResult("User with this email already registered!");
//                textBoxNewEmail.Text = "";
//                return;
//            }

//            if (newLogin.Length >= 6 &&
//                newLogin.Any(x => char.IsLetter(x)) &&
//                EmailValidation.EmailValidator.Validate(newEmail, true, true) &&
//                newPass.Length >= 6 &&
//                newPass.Any(x => char.IsLetter(x)) &&
//                newLogin != newPass &&
//                newPass == confirmNewPass)
//            {
//                string userId = GenerateUserId();
//                int userCreationResult = LandConquestDB.Models.UserModel.CreateUser(newLogin, YDCrypto.Encrypt(newEmail), YDCrypto.SHA512(newPass), userId);
//                if (userCreationResult < 0)
//                {
//                    WarningDialogWindow.CallWarningDialogNoResult("Error creating new user!");
//                }
//                else
//                {
//                    User registeredUser = new User();
//                    int playerResult = PlayerModel.CreatePlayer(newLogin, YDCrypto.Encrypt(newEmail), YDCrypto.SHA512(newPass), userId, registeredUser);

//                    if (playerResult < 0)
//                    {
//                        WarningDialogWindow.CallWarningDialogNoResult("Error creating new player!");
//                    }
//                    else
//                    {
//                        PlayerModel.CreatePlayerResources(userId, registeredUser);
//                        TaxesModel.CreateTaxesData(userId);

//                        CreatePersonDialogWindow dialogWindow = new CreatePersonDialogWindow(registeredUser);
//                        dialogWindow.Show();
//                        this.Close();

//                    }
//                    LandConquestDB.Entities.Army army = new LandConquestDB.Entities.Army();
//                    army.PlayerId = userId;
//                    army.ArmyId = GenerateUserId();
//                    LandConquestDB.Models.ArmyModel.InsertArmyFromReg(army);
//                }
//            }
//            else
//            {
//                confirmNewPass = "";
//                WarningDialogWindow.CallWarningDialogNoResult("Password and login should contain letters, not match and be at least 6 characters long.");
//            }
//        }

//        public static Random random;
//        public static string GenerateUserId()
//        {
//            System.Threading.Thread.Sleep(15);
//            random = new Random();
//            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
//            return new string(Enumerable.Repeat(chars, 16)
//              .Select(s => s[random.Next(s.Length)]).ToArray());
//        }

//        public void ShowRegistrationClick()
//        {
//            ShowRegistrationFields(Visibility.Visible);
//            buttonShowRegistration.Visibility = Visibility.Hidden;
//        }

//        public void CancelRegistrateClick()
//        {
//            ShowRegistrationFields(Visibility.Hidden);
//            buttonShowRegistration.Visibility = Visibility.Visible;
//            ClearAllValidationNotifications();
//            textBoxNewLogin.Text = "";
//            textBoxNewEmail.Text = "";
//            textBoxNewPass.Text = "";
//            textBoxConfirmNewPass.Text = "";
//        }
//        public void ShowRegistrationFields()
//        {
//            textBoxNewLogin.Visibility = visibility;
//            textBoxNewEmail.Visibility = visibility;
//            textBoxNewPass.Visibility = visibility;
//            textBoxConfirmNewPass.Visibility = visibility;
//            registerGui.Visibility = visibility;
//            buttonRegistrate.Visibility = visibility;
//            buttonCancelRegistrate.Visibility = visibility;
//            labelAgreement.Visibility = visibility;
//        }

//        public void CheckVersion()
//        {
//            if (LauncherLogic.CheckGameVersion().SequenceEqual(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion))
//            {
//                labelGameFiles.Content = "Game is up to date";
//                labelGameFiles.Foreground = System.Windows.Media.Brushes.GreenYellow;
//                iconDownload.Visibility = Visibility.Hidden;
//            }
//            else
//            {
//                labelGameFiles.Content = "Update required";
//                labelGameFiles.Foreground = System.Windows.Media.Brushes.Red;
//                iconDownload.Visibility = Visibility.Visible;
//            }
//        }

//        public void Download()
//        {
//            LauncherLogic.DownloadGame();
//        }

//        public void ShowAgreement()
//        {
//            UserAgreementDialog.ShowUserAgreement();
//        }

//        public void LoginLostFocus()
//        {
//            string newLogin = textBoxNewLogin.Text.Replace(" ", "");

//            if (newLogin.Length < 6)
//            {
//                labelNameValid.Content = "Your login is too short!";
//                labelNameValid.Foreground = System.Windows.Media.Brushes.Red;
//            }
//            else if (!newLogin.Any(x => char.IsLetter(x)))
//            {
//                labelNameValid.Content = "Your login must contain letters!";
//                labelNameValid.Foreground = System.Windows.Media.Brushes.Red;
//            }
//            else
//            {
//                labelNameValid.Content = "✓";
//                labelNameValid.Foreground = System.Windows.Media.Brushes.GreenYellow;
//            }

//        }

//        public void EmailLostFocus()
//        {
//            string newEmail = textBoxNewEmail.Text.Replace(" ", "");

//            if (!EmailValidation.EmailValidator.Validate(newEmail, true, true))
//            {
//                labelEmailValid.Content = "Your email is incorrect!";
//                labelEmailValid.Foreground = System.Windows.Media.Brushes.Red;
//            }
//            else
//            {
//                labelEmailValid.Content = "✓";
//                labelEmailValid.Foreground = System.Windows.Media.Brushes.GreenYellow;
//            }
//        }

//        public void NewPassLostFocus()
//        {
//            string newPass = textBoxNewPass.Text.Replace(" ", "");

//            if (newPass.Length < 6)
//            {
//                labelPassValid.Content = "Your password is too short!";
//                labelPassValid.Foreground = System.Windows.Media.Brushes.Red;
//            }
//            else if (!newPass.Any(x => char.IsLetter(x)))
//            {
//                labelPassValid.Content = "Your password must contain letters!";
//                labelPassValid.Foreground = System.Windows.Media.Brushes.Red;
//            }
//            else if (newPass == textBoxNewLogin.Text)
//            {
//                labelPassValid.Content = "Your password is the same as your login!";
//                labelPassValid.Foreground = System.Windows.Media.Brushes.Red;
//            }
//            else
//            {
//                labelPassValid.Content = "✓";
//                labelPassValid.Foreground = System.Windows.Media.Brushes.GreenYellow;
//            }
//        }

//        public void ConfirmNewPassFocus()
//        {
//            if (textBoxNewPass.Text != textBoxConfirmNewPass.Text)
//            {
//                labelConfirmPassValid.Content = "Passwords do not match!";
//                labelConfirmPassValid.Foreground = System.Windows.Media.Brushes.Red;
//            }
//            else
//            {
//                labelConfirmPassValid.Content = "✓";
//                labelConfirmPassValid.Foreground = System.Windows.Media.Brushes.GreenYellow;
//            }
//        }

//        public void ClearAllValidationNotifications()
//        {
//            labelNameValid.Content = "";
//            labelEmailValid.Content = "";
//            labelPassValid.Content = "";
//            labelConfirmPassValid.Content = "";
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        public void OnPropertyChanged([CallerMemberName] string prop = "")
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
//        }
//    }
//}
