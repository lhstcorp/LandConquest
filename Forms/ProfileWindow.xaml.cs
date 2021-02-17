using EmailValidation;
using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System.Linq;
using System.Windows;

namespace LandConquest.Forms
{

    public partial class ProfileWindow : Window
    {
        private Player player;
        private User user;

        public ProfileWindow(Player _player, User _user)
        {
            InitializeComponent();
            player = _player;
            user = _user;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            user = new User();
            user = UserModel.GetUserInfo(player.PlayerId);

            player = new Player();
            player = PlayerModel.GetPlayerInfo(user, player);


            labelName.Content = player.PlayerName.ToString();
            labelTitle.Content = player.PlayerTitle.ToString();
            labelLand.Content = player.PlayerCurrentRegion.ToString();

            labelEmail.Content = user.UserEmail.ToString();
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
            newNameBox.Text.Replace(" ", "");
            bool validNameChangeLogin = UserModel.ValidateUserByLogin(newNameBox.Text);
            if (newNameBox.Text.Length > 6 && validNameChangeLogin == true)
            {
                PlayerModel.UpdatePlayerName(player.PlayerId, newNameBox.Text);
                player.PlayerName = newNameBox.Text;
                this.Loaded += Window_Loaded;
                newNameBox.Visibility = Visibility.Hidden;
                buttonSaveName.Visibility = Visibility.Hidden;
                buttonChangeName.Visibility = Visibility.Visible;
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error changing login!");
            }
        }

        private void buttonSaveEmail_Click(object sender, RoutedEventArgs e)
        {
            newEmailBox.Text.Replace(" ", "");
            bool validEmailChangeEmail = UserModel.ValidateUserByEmail(newEmailBox.Text);
            if (validEmailChangeEmail == true && EmailValidator.Validate(newEmailBox.Text, true, true))
            {
                UserModel.UpdateUserEmail(user.UserId, newEmailBox.Text);
                this.Loaded += Window_Loaded;
                newEmailBox.Visibility = Visibility.Hidden;
                buttonSaveEmail.Visibility = Visibility.Hidden;
                buttonChangeEmail.Visibility = Visibility.Visible;
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Error changing email!");
            }
        }

        private void buttonSavePass_Click(object sender, RoutedEventArgs e)
        {
            newPassBox.Text.Replace(" ", "");
            //ДОБАВИТЬ ВАЛИДАЦИЮ
            UserModel.UpdateUserPass(user.UserId, newPassBox.Text);
            this.Loaded += Window_Loaded;
            newPassBox.Visibility = Visibility.Hidden;
            buttonSavePass.Visibility = Visibility.Hidden;
            buttonChangePass.Visibility = Visibility.Visible;
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
    }
}
