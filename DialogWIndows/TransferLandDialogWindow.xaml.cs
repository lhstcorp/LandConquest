using LandConquest.Forms;
using LandConquestDB.Entities;
using LandConquestDB.Models;
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
    /// Логика взаимодействия для TransferLandDialogWindow.xaml
    /// </summary>
    public partial class TransferLandDialogWindow : Window
    {
        Country country;
        Person person;
        CountryWindow countryWindow;

        public TransferLandDialogWindow(Country _country, Person _person, CountryWindow _countryWindow)
        {
            country = _country;
            person  = _person;
            countryWindow = _countryWindow;

            InitializeComponent();
            initCbbxs();
        }

        private void initCbbxs()
        {
            List<Land> countryLands = LandModel.GetCountryLands(country.CountryId);

            for (int i = 0; i < countryLands.Count; i++)
            {
                landsCbbx.Items.Add(countryLands[i].LandName);
            }

            List<Country> countries = CountryModel.GetCountriesInfo();
            countries.RemoveAll(c => c.CountryId == country.CountryId);

            for (int i = 0; i < countries.Count; i++)
            {
                countriesCbbx.Items.Add(countries[i].CountryName);
            }
        }

        private void transferLandBtn_Click(object sender, RoutedEventArgs e)
        {
            Law law = new Law();
            law.CountryId = country.CountryId;
            law.Operation = 2;
            law.PlayerId = person.PlayerId;
            law.PersonId = person.PersonId;
            law.Value1 = landsCbbx.SelectedItem.ToString();
            law.Value2 = countriesCbbx.SelectedItem.ToString();

            LawModel.insertLaw(law);

            countryWindow.populateActiveLawGrid();

            this.Close();
            WarningDialogWindow.CallWarningDialogNoResult(Languages.Resources.LocLabelTheLawWasInitiated_Text);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
