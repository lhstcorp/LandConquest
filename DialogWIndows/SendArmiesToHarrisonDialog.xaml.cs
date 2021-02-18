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

namespace LandConquest.DialogWIndows
{
    /// <summary>
    /// Логика взаимодействия для SendArmiesToHarrisonDialog.xaml
    /// </summary>
    public partial class SendArmiesToHarrisonDialog : Window
    {
        public SendArmiesToHarrisonDialog()
        {
            InitializeComponent();
        }

        private void sliderInfantry_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderInfantry.Value = 0;
        }

        private void sliderArchers_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderInfantry.Value = 0;
        }

        private void sliderKnights_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderInfantry.Value = 0;
        }

        private void sliderSiege_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderInfantry.Value = 0;
        }

        private void CheckTypeAndReturnCount(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
