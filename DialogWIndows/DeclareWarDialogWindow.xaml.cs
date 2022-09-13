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
    public partial class DeclareWarDialogWindow : Window
    {
        Country country;
        Person person;
        CountryWindow countryWindow;

        public DeclareWarDialogWindow(Country _country, Person _person, CountryWindow _countryWindow)
        {
            country = _country;
            person = _person;
            countryWindow = _countryWindow;

            InitializeComponent();
            initCbbxs();
        }

        private void initCbbxs()
        {
            List<Land> countryLands = LandModel.GetCountryLands(country.CountryId);

            for (int i = 0; i < countryLands.Count; i++)
            {
                if (!LandModel.isLandInWar(countryLands[i].LandId))
                {
                    attackersLandCbbx.Items.Add(countryLands[i].LandName);
                }
            }

            List<Country> countries = CountryModel.GetCountriesInfo();
            countries.RemoveAll(c => c.CountryId == country.CountryId);

            for (int i = 0; i < countries.Count; i++)
            {
                defendersCountryCbbx.Items.Add(countries[i].CountryName);
            }

            List<Land> defendersLands = LandModel.GetCountryLands(countries[0].CountryId);

            for (int i = 0; i < defendersLands.Count; i++)
            {
                defendersLandCbbx.Items.Add(defendersLands[i].LandName);
            }
        }

            private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
