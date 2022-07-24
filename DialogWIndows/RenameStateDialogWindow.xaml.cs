using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        Person person;

        public RenameStateDialogWindow(Country _country, Person _person)
        {
            country = _country;
            person  = _person;

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
            while (newCountryNameTB.Text.Contains("  "))
            {
                newCountryNameTB.Text = newCountryNameTB.Text.Replace("  ", " ");
            }

            newCountryNameTB.Select(newCountryNameTB.Text.Length, 0);
        }

        public static bool IsValid(string str)
        {
            char ch = str[str.Length - 1];

            return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
        }

        private void renameBtn_Click(object sender, RoutedEventArgs e)
        {
            editUserInput();

            if (newCountryNameTB.Text.Length >= 3)
            {
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.Operation = 1;
                law.PlayerId = person.PlayerId;
                law.PersonId = person.PersonId;
                law.Value1 = newCountryNameTB.Text;

                LawModel.insertLaw(law);

                this.Close();
                WarningDialogWindow.CallWarningDialogNoResult(Languages.Resources.LocLabelTheLawWasInitiated_Text);
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult(Languages.Resources.LocLabelCountryNameLengthValidation);
                newCountryNameTB.Focus();
            }
        }
    }
}
