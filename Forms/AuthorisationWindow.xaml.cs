using LandConquest.Logic;
using LandConquest.WindowViewModels;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace LandConquest
{
    public partial class AuthorisationWindow : Window
    {
        public AuthorisationWindow()
        {
            InitializeComponent();
            DataContext = new AuthorisationWindowViewModel();
            CheckVersion();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            ((AuthorisationWindowViewModel)DataContext).LoginClick();
        }

        private void ButtonRegistrate_Click(object sender, RoutedEventArgs e)
        {
            ((AuthorisationWindowViewModel)DataContext).RegistrateClick();
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
            ((AuthorisationWindowViewModel)DataContext).Download();
        }

        private void LabelAgreement_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((AuthorisationWindowViewModel)DataContext).ShowAgreement();
        }

        private void textBoxNewLogin_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            ((AuthorisationWindowViewModel)DataContext).LoginLostFocus();

        }

        private void textBoxNewEmail_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            ((AuthorisationWindowViewModel)DataContext).EmailLostFocus();
        }

        private void textBoxNewPass_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            ((AuthorisationWindowViewModel)DataContext).NewPassLostFocus();
        }

        private void textBoxConfirmNewPass_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            ((AuthorisationWindowViewModel)DataContext).ConfirmNewPassFocus();
        }

        private void ClearAllValidationNotifications()
        {
            ((AuthorisationWindowViewModel)DataContext).ClearAllValidationNotifications();
        }

        public static string GenerateUserId() //this method moved to AuthorisationWindowViewModel. 
        {
            System.Threading.Thread.Sleep(15);
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}


