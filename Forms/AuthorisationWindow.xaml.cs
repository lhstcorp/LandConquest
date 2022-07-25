using LandConquest.Logic;
using LandConquest.WindowViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LandConquest
{
    public partial class AuthorisationWindow : Window
    {
        public AuthorisationWindow()
        {
            InitializeComponent();
            DataContext = new AuthorisationWindowViewModel();
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
            ((AuthorisationWindowViewModel)DataContext).ShowRegistration();
        }

        private void ButtonCancelRegistrate_Click(object sender, RoutedEventArgs e)
        {
            ((AuthorisationWindowViewModel)DataContext).CancelRegistration();
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

        public static string GenerateUserId() //this method moved to AssistantLogic. 
        {
            return AssistantLogic.GenerateId();
        }

        private void textBoxPass_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //var tb = (System.Windows.Controls.TextBox)sender;
            //using (tb.DeclareChangeBlock())
            //{
            //    foreach (var c in e.Changes)
            //    {
            //        if (c.AddedLength == 0) continue;
            //        tb.Select(c.Offset, c.AddedLength);
            //        if (tb.SelectedText.Contains("(?s)."))
            //        {
            //            tb.SelectedText = tb.SelectedText.Replace(System.Char.IsLetter, '*');
            //        }
            //        tb.Select(c.Offset + c.AddedLength, 0);
            //    }
            //}
        }
        private bool IsAlphabetic(string s)
        {
            Regex r = new Regex(@"^[a-zA-Z]+$");

            return r.IsMatch(s);
        }

        private void RegisterName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Prohibit non-alphabetic
            if (!IsAlphabetic(e.Text))
            {
                e.Handled = true;
            }
        }


        public int i = 0;
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
           
            if (textBoxNewLogin.Text.Length == 1)
            {
                textBoxNewLogin.Text = textBoxNewLogin.Text.ToUpper();
                textBoxNewLogin.Select(textBoxNewLogin.Text.Length, 0);
            }

        }

        private void Register_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==System.Windows.Input.Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}


