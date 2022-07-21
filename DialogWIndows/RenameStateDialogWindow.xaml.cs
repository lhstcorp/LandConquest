using LandConquestDB.Entities;
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
    /// Логика взаимодействия для RenameStateDialogWindow.xaml
    /// </summary>
    public partial class RenameStateDialogWindow : Window
    {
        Country country;

        public RenameStateDialogWindow(Country _country)
        {
            country = _country;

            InitializeComponent();
            initializeFormText();
        }

        private void initializeFormText()
        {
            descriptionTB.Text = String.Format(Languages.Resources.LocLabelRenameStateDialogWindow_Text, country.CountryName);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void newCountryNameTB_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            bool retH = !(IsValid(((TextBox)sender).Text + e.Text));

            editUserInput();

            e.Handled = retH;
        }

        private void editUserInput()
        {
            for (int i = 0; i < newCountryNameTB.Text.Length; i++)
            {
                newCountryNameTB.Text = newCountryNameTB.Text.Replace("  ", " ");
            }

            if (newCountryNameTB.Text[newCountryNameTB.Text.Length - 1] == ' ')
            {
                newCountryNameTB.Text.Remove(newCountryNameTB.Text.Length - 1, 1);
            }

            newCountryNameTB.Select(newCountryNameTB.Text.Length, 0);
        }

        public static bool IsValid(string str)
        {
            char ch = str[str.Length - 1];

            return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
        }
    }
}
