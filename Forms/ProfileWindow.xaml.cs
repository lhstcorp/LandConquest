using EmailValidation;
using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using LandConquestYD;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LandConquest.Forms
{

    public partial class ProfileWindow : Window
    {
        private Player player;
        private User user;
        private Window openedWindow;
        public event Action<string> NameChanged;

        public ProfileWindow(Player _player, User _user)
        {
            InitializeComponent();
            player = _player;
            user = _user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //user = new User();
            //user = UserModel.GetUserInfo(player.PlayerId);

            //player = new Player();
            //player = PlayerModel.GetPlayerById(user.UserId);

            
            labelName.Content = player.PlayerName.ToString();
            labelTitle.Content = player.PlayerTitle.ToString();
            labelLand.Content = player.PlayerCurrentRegion.ToString();

            labelEmail.Content = YDCrypto.Decrypt(user.UserEmail);
            labelLogin.Content = user.UserLogin.ToString();

            newEmailBox.Visibility = Visibility.Hidden;
            newNameBox.Visibility = Visibility.Hidden;
            newPassBox.Visibility = Visibility.Hidden;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonSaveName_Click(object sender, RoutedEventArgs e)
        {
            string newName = newNameBox.Text.Replace(" ", "");
            bool validNameChangeLogin = UserModel.CheckLoginExistence(newNameBox.Text);
            if (newName.Length >= 6 && validNameChangeLogin == true && newNameBox.Text.Any(x => char.IsLetter(x)))
            {
                PlayerModel.UpdatePlayerName(player.PlayerId, newNameBox.Text);
                player.PlayerName = newNameBox.Text;
                newNameBox.Visibility = Visibility.Hidden;
                buttonSaveName.Visibility = Visibility.Hidden;
                buttonChangeName.Visibility = Visibility.Visible;
                labelName.Content = newName;
                NameChanged(newName);
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error changing login!");
            }
        }

        private void buttonSaveEmail_Click(object sender, RoutedEventArgs e)
        {
            string newEmail = newEmailBox.Text.Replace(" ", "");
            bool emailExist = UserModel.CheckEmailExistence(newEmail);
            if (!emailExist && EmailValidator.Validate(newEmail, true, true))
            {
                UserModel.UpdateUserEmail(user.UserId, YDCrypto.Encrypt(newEmail));
                newEmailBox.Visibility = Visibility.Hidden;
                buttonSaveEmail.Visibility = Visibility.Hidden;
                buttonChangeEmail.Visibility = Visibility.Visible;
                labelEmail.Content = newEmail;
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error changing email!");
            }
        }

        private void buttonSavePass_Click(object sender, RoutedEventArgs e)
        {
            newPassBox.Text.Replace(" ", "");
            if (newPassBox.Text.Length >= 6 && newNameBox.Text.Any(x => char.IsLetter(x)))
            {
                UserModel.UpdateUserPass(user.UserId, YDCrypto.SHA512(newPassBox.Text));
                newPassBox.Visibility = Visibility.Hidden;
                buttonSavePass.Visibility = Visibility.Hidden;
                buttonChangePass.Visibility = Visibility.Visible;
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error changing password!");
            }
        }

        private void buttonChangeName_Click(object sender, RoutedEventArgs e)
        {
            buttonChangeName.Visibility = Visibility.Hidden;
            buttonSaveName.Visibility = Visibility.Visible;
            newNameBox.Visibility = Visibility.Visible;
        }

        private void buttonChangeEmail_Click(object sender, RoutedEventArgs e)
        {
            buttonChangeEmail.Visibility = Visibility.Hidden;
            buttonSaveEmail.Visibility = Visibility.Visible;
            newEmailBox.Visibility = Visibility.Visible;
        }

        private void buttonChangePass_Click(object sender, RoutedEventArgs e)
        {
            buttonChangePass.Visibility = Visibility.Hidden;
            buttonSavePass.Visibility = Visibility.Visible;
            newPassBox.Visibility = Visibility.Visible;
        }

        private void ChangeAvatarLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            openedWindow = new CoatOfArmsWindow(player);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }
        private void FreeData(object data, EventArgs e)
        {
            openedWindow = null;
            GC.Collect();
        }

        private void ChangeAvatarLabel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void ChangeAvatarLabel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Space_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
