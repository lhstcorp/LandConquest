using LandConquest.DialogWIndows;
using LandConquest.Forms;
using LandConquest.Logic;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using LandConquestYD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Media;

namespace LandConquest.WindowViewModels
{
    public class AuthorisationWindowViewModel : INotifyPropertyChanged
    {
        public AuthorisationWindowViewModel()
        {
            WindowLoaded();
        }

        private string _loginText;
        public string LoginText
        {
            get
            {
                return _loginText;
            }
            set
            {
                _loginText = value;
                OnPropertyChanged("LoginText");
            }
        }

        private string _passText;
        public string PassText
        {
            get
            {
                return _passText;
            }
            set
            {
                _passText = value;
                OnPropertyChanged("PassText");
            }
        }

        private string _newLoginText;
        public string NewLoginText
        {
            get
            {
                return _newLoginText;
            }
            set
            {
                _newLoginText = value;
                OnPropertyChanged("NewLoginText");
            }
        }

        private string _newPassText;
        public string NewPassText
        {
            get
            {
                return _newPassText;
            }
            set
            {
                _newPassText = value;
                OnPropertyChanged("NewPassText");
            }
        }

        private string _newEmailText;
        public string NewEmailText
        {
            get
            {
                return _newEmailText;
            }
            set
            {
                _newEmailText = value;
                OnPropertyChanged("NewEmailText");
            }
        }

        private string _ConfirmNewPassText;
        public string ConfirmNewPassText
        {
            get
            {
                return _ConfirmNewPassText;
            }
            set
            {
                _ConfirmNewPassText = value;
                OnPropertyChanged("ConfirmNewPassText");
            }
        }

        private string _labelNameValidContent;
        public string NameValidContent
        {
            get
            {
                return _labelNameValidContent;
            }
            set
            {
                _labelNameValidContent = value;
                OnPropertyChanged("NameValidContent");
            }
        }

        private Brush _labelNameValidForeground;
        public Brush NameValidForeground
        {
            get
            {
                return _labelNameValidForeground;
            }
            set
            {
                _labelNameValidForeground = value;
                OnPropertyChanged("NameValidForeground");
            }
        }

        private string _labelEmailValidContent;
        public string EmailValidContent
        {
            get
            {
                return _labelEmailValidContent;
            }
            set
            {
                _labelEmailValidContent = value;
                OnPropertyChanged("EmailValidContent");
            }
        }

        private Brush _labelEmailValidForeground;
        public Brush EmailValidForeground
        {
            get
            {
                return _labelEmailValidForeground;
            }
            set
            {
                _labelEmailValidForeground = value;
                OnPropertyChanged("EmailValidForeground");
            }
        }

        private string _labelPassValidContent;
        public string PassValidContent
        {
            get
            {
                return _labelPassValidContent;
            }
            set
            {
                _labelPassValidContent = value;
                OnPropertyChanged("PassValidContent");
            }
        }

        private Brush _labelPasslValidForeground;
        public Brush PassValidForeground
        {
            get
            {
                return _labelPasslValidForeground;
            }
            set
            {
                _labelPasslValidForeground = value;
                OnPropertyChanged("PassValidForeground");
            }
        }

        private string _labelConfirmPassValidContent;
        public string ConfirmPassValidContent
        {
            get
            {
                return _labelConfirmPassValidContent;
            }
            set
            {
                _labelConfirmPassValidContent = value;
                OnPropertyChanged("ConfirmPassValidContent");
            }
        }

        private Brush _labelConfirmPasslValidForeground;
        public Brush ConfirmPassValidForeground
        {
            get
            {
                return _labelConfirmPasslValidForeground;
            }
            set
            {
                _labelConfirmPasslValidForeground = value;
                OnPropertyChanged("ConfirmPassValidForeground");
            }
        }

        private string _labelCurrentOnlineContent;
        public string CurrentOnlineContent
        {
            get
            {
                return _labelCurrentOnlineContent;
            }
            set
            {
                _labelCurrentOnlineContent = value;
                OnPropertyChanged("CurrentOnlineContent");
            }
        }

        private string _labelGameFilesContent;
        public string GameFilesContent
        {
            get
            {
                return _labelGameFilesContent;
            }
            set
            {
                _labelGameFilesContent = value;
                OnPropertyChanged("GameFilesContent");
            }
        }

        private bool _checkboxRememberIsChecked;
        public bool CheckboxRememberIsChecked
        {
            get
            {
                return _checkboxRememberIsChecked;
            }
            set
            {
                _checkboxRememberIsChecked = value;
                OnPropertyChanged("CheckboxRememberIsChecked");
            }
        }

        private object _windowTag;
        public object WindowTag
        {
            get
            {
                return _windowTag;
            }
            set
            {
                _windowTag = value;
                OnPropertyChanged("WindowTag");
            }
        }


        public void WindowLoaded()
        {
            YDContext.OpenYD();
            //LauncherLogic.DisableActiveCheatsAsync();
            AssistantLogic.CheckLocalUtcDateTime();
            LandConquestDB.DbContext.OpenConnectionPool();
            //CheckVersion();
            CurrentOnlineContent        = YDContext.CountConnections().ToString();
            LoginText                   = Properties.Settings.Default.UserLogin;
            PassText                    = Properties.Settings.Default.UserPassword;
            CheckboxRememberIsChecked   = true;
        }


        public void LoginClick()
        {

            User user = UserModel.UserAuthorisation(LoginText, YDCrypto.SHA512(PassText));

            if (user.UserLogin == LoginText && user.UserPass == YDCrypto.SHA512(PassText))
            {
                AssistantLogic.CallSplashScreen();

                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();

                if (CheckboxRememberIsChecked == true)
                {
                    Properties.Settings.Default.UserLogin = LoginText;
                    Properties.Settings.Default.UserPassword = PassText;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.UserLogin = "";
                    Properties.Settings.Default.UserPassword = "";
                    Properties.Settings.Default.Save();
                }
                AssistantLogic.CloseWindowByTag(WindowTag = 1);
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("No account with this username/password combination was found!");
            }
        }

        public void RegistrateClick()
        {
            if (String.IsNullOrWhiteSpace(NewLoginText) ||
                String.IsNullOrWhiteSpace(NewEmailText) ||
                String.IsNullOrWhiteSpace(NewPassText)  ||
                String.IsNullOrWhiteSpace(ConfirmNewPassText))
            {
                return;
            }

            string newLogin         = NewLoginText.Replace(" ", "");
            string newEmail         = NewEmailText.Replace(" ", "");
            string newPass          = NewPassText.Replace(" ", "");
            string confirmNewPass   = ConfirmNewPassText.Replace(" ", "");

            bool validNewUserLogin = UserModel.CheckLoginExistence(newLogin);
            if (!validNewUserLogin)
            {
                WarningDialogWindow.CallWarningDialogNoResult("User with this login already registered!");
                NewLoginText = "";
                return;
            }
            bool validNewUserEmail = UserModel.CheckEmailExistence(YDCrypto.Encrypt(newEmail));
            if (validNewUserEmail)
            {
                WarningDialogWindow.CallWarningDialogNoResult("User with this email already registered!");
                NewEmailText = "";
                return;
            }

            if (newLogin.Length >= 6 &&
                newLogin.Any(x => char.IsLetter(x)) &&
                EmailValidation.EmailValidator.Validate(newEmail, true, true) &&
                newPass.Length >= 6 &&
                newPass.Any(x => char.IsLetter(x)) &&
                newLogin != newPass &&
                newPass == confirmNewPass)
            {
                string userId = GenerateUserId();
                int userCreationResult = UserModel.CreateUser(newLogin, YDCrypto.Encrypt(newEmail), YDCrypto.SHA512(newPass), userId);
                if (userCreationResult < 0)
                {
                    WarningDialogWindow.CallWarningDialogNoResult("Error creating new user!");
                }
                else
                {
                    User registeredUser = new User();
                    int playerResult = PlayerModel.CreatePlayer(newLogin, YDCrypto.Encrypt(newEmail), YDCrypto.SHA512(newPass), userId, registeredUser);

                    if (playerResult < 0)
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Error creating new player!");
                    }
                    else
                    {
                        PlayerModel.CreatePlayerResources(userId, registeredUser);
                        TaxesModel.CreateTaxesData(userId);

                        CreatePersonDialogWindow dialogWindow = new CreatePersonDialogWindow(registeredUser);
                        dialogWindow.Show();

                        AssistantLogic.CloseWindowByTag(WindowTag = 1);

                    }
                    Army army = new Army();
                    army.PlayerId = userId;
                    army.ArmyId = GenerateUserId();
                    ArmyModel.InsertArmyFromReg(army);
                }
            }
            else
            {
                confirmNewPass = "";
                WarningDialogWindow.CallWarningDialogNoResult("Password and login should contain letters, not match and be at least 6 characters long.");
            }
        }

        public static Random random;
        public static string GenerateUserId()
        {
            System.Threading.Thread.Sleep(15);
            random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //public void ShowRegistrationClick()
        //{
        //    ShowRegistrationFields(Visibility.Visible);
        //    buttonShowRegistration.Visibility = Visibility.Hidden;
        //}

        //public void CancelRegistrateClick()
        //{
        //    ShowRegistrationFields(Visibility.Hidden);
        //    buttonShowRegistration.Visibility = Visibility.Visible;
        //    ClearAllValidationNotifications();
        //    textBoxNewLogin.Text = "";
        //    textBoxNewEmail.Text = "";
        //    textBoxNewPass.Text = "";
        //    textBoxConfirmNewPass.Text = "";
        //}
        //public void ShowRegistrationFields(Visibility visibility)
        //{
        //    textBoxNewLogin.Visibility = visibility;
        //    textBoxNewEmail.Visibility = visibility;
        //    textBoxNewPass.Visibility = visibility;
        //    textBoxConfirmNewPass.Visibility = visibility;
        //    registerGui.Visibility = visibility;
        //    buttonRegistrate.Visibility = visibility;
        //    buttonCancelRegistrate.Visibility = visibility;
        //    labelAgreement.Visibility = visibility;
        //}

        //public void CheckVersion()
        //{
        //    if (LauncherLogic.CheckGameVersion().SequenceEqual(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion))
        //    {
        //        labelGameFiles.Content = "Game is up to date";
        //        labelGameFiles.Foreground = System.Windows.Media.Brushes.GreenYellow;
        //        iconDownload.Visibility = Visibility.Hidden;
        //    }
        //    else
        //    {
        //        labelGameFiles.Content = "Update required";
        //        labelGameFiles.Foreground = System.Windows.Media.Brushes.Red;
        //        iconDownload.Visibility = Visibility.Visible;
        //    }
        //}

        public void Download()
        {
            AssistantLogic.DownloadGame();
        }

        public void ShowAgreement()
        {
            UserAgreementDialog.ShowUserAgreement();
        }

        public void LoginLostFocus()
        {
            if (String.IsNullOrWhiteSpace(NewLoginText))
            {
                NameValidContent = "Enter login!";
                NameValidForeground = Brushes.Red;
                return;
            }

            string newLogin = NewLoginText.Replace(" ", "");

            if (newLogin.Length < 6)
            {
                NameValidContent    = "Your login is too short!";
                NameValidForeground = Brushes.Red;
            }
            else if (!newLogin.Any(x => char.IsLetter(x)))
            {
                NameValidContent    = "Your login must contain letters!";
                NameValidForeground = Brushes.Red;
            }
            else
            {
                NameValidContent    = "✓";
                NameValidForeground = Brushes.GreenYellow;
            }

        }

        public void EmailLostFocus()
        {
            if (String.IsNullOrWhiteSpace(NewEmailText))
            {
                EmailValidContent = "Enter email!";
                EmailValidForeground = Brushes.Red;
                return;
            }

            string newEmail = NewEmailText.Replace(" ", "");

            if (!EmailValidation.EmailValidator.Validate(newEmail, true, true))
            {
                EmailValidContent       = "Your email is incorrect!";
                EmailValidForeground    = Brushes.Red;
            }
            else
            {
                EmailValidContent       = "✓";
                EmailValidForeground    = Brushes.GreenYellow;
            }
        }

        public void NewPassLostFocus()
        {
            if (String.IsNullOrWhiteSpace(NewPassText))
            {
                PassValidContent = "Enter password!";
                PassValidForeground = Brushes.Red;
                return;
            }

            string newPass = NewPassText.Replace(" ", "");

            if (newPass.Length < 6)
            {
                PassValidContent    = "Your password is too short!";
                PassValidForeground = Brushes.Red;
            }
            else if (!newPass.Any(x => char.IsLetter(x)))
            {
                PassValidContent    = "Your password must contain letters!";
                PassValidForeground = Brushes.Red;
            }
            else if (newPass == NewLoginText)
            {
                PassValidContent    = "Your password is the same as your login!";
                PassValidForeground = Brushes.Red;
            }
            else
            {
                PassValidContent    = "✓";
                PassValidForeground = Brushes.GreenYellow;
            }
        }

        public void ConfirmNewPassFocus()
        {
            if (String.IsNullOrWhiteSpace(ConfirmNewPassText))
            {
                ConfirmPassValidContent = "Confirm password!";
                ConfirmPassValidForeground = Brushes.Red;
                return;
            }

            if (NewPassText != ConfirmNewPassText)
            {
                ConfirmPassValidContent     = "Passwords do not match!";
                ConfirmPassValidForeground  = Brushes.Red;
            }
            else
            {
                ConfirmPassValidContent     = "✓";
                ConfirmPassValidForeground  = Brushes.GreenYellow;
            }
        }

        public void ClearAllValidationNotifications()
        {
            NameValidContent        = "";
            EmailValidContent       = "";
            PassValidContent        = "";
            ConfirmPassValidContent = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
