using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для LandResourcesWindow.xaml
    /// </summary>
    public partial class LandResourcesWindow : Window
    {
        public LandResourcesWindow()
        {
            InitializeComponent();
        }
        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DonateAllButton_Click(object sender, RoutedEventArgs e)
        {
            ///
            ///
            ///
            ///
        }

        private void FoodToBuyTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }
        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 1 && i <= 99999;
        }
    }
}
