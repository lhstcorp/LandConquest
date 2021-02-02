using LandConquestDB.Entities;
using System.Windows;


namespace LandConquest.Forms
{
    public partial class CastleManagementWindow : Window
    {
        private protected enum Level : int
        {
            Wood = 2500,
            Stone = 5000,
            Iron = 1000,
            Copper = 500,
            Money = 1000
        }

        private Player player;
        public CastleManagementWindow(Player _player)
        {
            player = _player;
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonShowMenu_Click(object sender, RoutedEventArgs e)
        {
            if (castleMenuGrid.Visibility == Visibility.Hidden)
            {
                castleMenuGrid.Visibility = Visibility.Visible;
                castleMenuGridBorder.Visibility = Visibility.Visible;
            }
            else
            {
                castleMenuGrid.Visibility = Visibility.Hidden;
                castleMenuGridBorder.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            castleMenuGrid.Visibility = Visibility.Hidden;
            castleMenuGridBorder.Visibility = Visibility.Hidden;
        }
    }
}
