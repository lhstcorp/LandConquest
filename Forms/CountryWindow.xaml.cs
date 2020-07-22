using LandConquest.Entities;
using LandConquest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для CountryWindow.xaml
    /// </summary>
    public partial class CountryWindow : Window
    {
        SqlConnection connection;
        Player player;
        CountryModel countryModel;
        PlayerModel playerModel;
        LandModel landModel;
        List<Land> countryLands;
        List<Country> countries;
        Land selectedLand;
        Country transferCountry;
        int operation = 0;
        public CountryWindow(SqlConnection _connection, Player _player)
        {
            connection = _connection;
            player = _player;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            countryModel = new CountryModel();
            playerModel = new PlayerModel();
            Country country = countryModel.GetCountryById(connection, countryModel.GetCountryId(connection, player));
            Player ruler = new Player();
            User rulerUser = new User();
            rulerUser.UserId = country.CountryRuler;
            ruler = playerModel.GetPlayerInfo(rulerUser, connection, ruler);
            RulerNameLbl.Content = ruler.PlayerName;
            CountryNameLbl.Content = country.CountryName;

            landModel = new LandModel();
            countryLands = landModel.GetCountryLands(connection, country);

            int count = countryModel.SelectLastIdOfStates(connection);

            countries = new List<Country>();
            for (int i = 0; i < count; i++)
            {
                countries.Add(new Country());
            }
            countries = countryModel.GetCountriesInfo(countries, connection);

            //Label lab1 = new Label();
            //lab1.Margin = new Thickness(100, 100, 15, 15);
            //lab1.Content = "govno";
            //lab1.FontSize = 50;
            //MainGrid.Children.Add(lab1);
        }

        private void CbAct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            //MessageBox.Show(selectedItem.Content.ToString());

            if (selectedItem.Content.ToString() == "Land transfer")
            {
                operation = 1;
                for (int i = 0; i < countryLands.Count; i++)
                {
                    CbLandToTransfer.Items.Add(countryLands[i].LandName);
                }

                for (int i = 0; i < countries.Count; i++)
                {
                    CbCountryToTransfer.Items.Add(countries[i].CountryName);
                }
            }
            if (selectedItem.Content.ToString() == "Declare a war")
            {
                operation = 2;
                for (int i = 0; i < countryLands.Count; i++)
                {
                    CbLandToTransfer.Items.Add(countryLands[i].LandName);
                }

                for (int i = 0; i < countries.Count; i++)
                {
                    CbCountryToTransfer.Items.Add(countries[i].CountryName);
                }

                CbCountryWarLand.Visibility = Visibility.Visible;


                //for (int i = 0; i < countryLands.Count; i++)
                //{
                //    CbCountryWarLand.Items.Add(countryLands[i].LandName);
                //}
            }

        }

        private void IssueALaw_Click(object sender, RoutedEventArgs e)
        {
            switch (operation)
            {
                case 1:
                    {
                        Player player = new Player();
                        player.PlayerCurrentRegion = selectedLand.LandId;
                        Country ThisCountry = countryModel.GetCountryById(connection, countryModel.GetCountryId(connection, player));

                        landModel.UpdateLandInfo(connection, selectedLand, transferCountry);

                        countryLands = landModel.GetCountryLands(connection, ThisCountry);
                        if (countryLands.Count == 0)
                        {
                            countryModel.DisbandCountry(connection, ThisCountry);
                        }
                        //тут нужно написать функцию на чек пустых государств. Если гос-во пустое - король теряет свой титул.
                        break;
                    }
                case 2:
                    {
                        //Player player = new Player();
                        //player.PlayerCurrentRegion = selectedLand.LandId;
                        //Country ThisCountry = countryModel.GetCountryById(connection, countryModel.GetCountryId(connection, player));

                        break;
                    }
            }
                

        }

        private void CbLandToTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedLand = new Land();
            selectedLand = countryLands[CbLandToTransfer.SelectedIndex];      
        }

        private void CbCountryToTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            transferCountry = new Country();

            transferCountry = countries[CbCountryToTransfer.SelectedIndex];
            Console.WriteLine(transferCountry.CountryName);
        }
    }
}
