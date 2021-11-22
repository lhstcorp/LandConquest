using LandConquest.WindowViewModels;
using LandConquestDB.Entities;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class AuctionWindow : Window
    {
        public AuctionWindow(Player _player)
        {
            InitializeComponent();
            DataContext = new AuctionWindowViewModel(_player);
        }
    }
}
