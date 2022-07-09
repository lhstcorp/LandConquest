using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.DialogWIndows
{

    public partial class EstablishStateDialog : Window
    {
        private Player player;
        private Land land;
        private LandModel landModel;
        private CountryModel countryModel;

        public EstablishStateDialog(Player _player, Land _land)
        {
            InitializeComponent();
            player = _player;
            land = _land;
            Loaded += Window_Loaded;
        }

        private void EstablishState_Click(object sender, RoutedEventArgs e)
        {
            Country country = CountryModel.EstablishState(player, land, StateColor.Color);
            LandModel.UpdateLandInfo(land, country);
            CountryModel.UpdateCountryCapital(country, land.LandId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            landModel = new LandModel();
            countryModel = new CountryModel();

            landDescriptionTextBlock.Text = String.Format(Languages.Resources.LocLabelThisLandIsIndependentDescription_Text, land.LandName);

            populatePersonGrid();
        }

        public void populatePersonGrid()
        {
            List<Person> persons = PersonModel.GetPlayerPersons(player.PlayerId);

            if (persons.Count > 0)
            {
                //ImageBrush personImg = new ImageBrush();
                //personImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Pictures/Hero.png", UriKind.Absolute));
                //selectedPersonEllipse.Fill = personImg;
                selectedPersonEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Hero.png", UriKind.Absolute)));
                selectedPersonNameText.Text = persons[0].Name + ' ' + persons[0].Surname;

                //initPersonGridsSize(persons);
                populatePersonGrids(persons);
            }
            else
            {
                //selectedPersonEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("/Pictures/epic_brown_square.png", UriKind.Relative)));
                selectedPersonNameText.Text = "No person";
            }
        }

        //private void initPersonGridsSize(List<Person> _playerPersons)
        //{
        //    int remainder = _playerPersons.Count - 1 - personGrid.Columns * 2; // 2 hardcode.
        //    int addRows = 0;

        //    if (remainder > 0)
        //    {
        //        addRows = remainder / personGrid.Columns;

        //        if (remainder % personGrid.Columns != 0)
        //        {
        //            addRows++;
        //        }
        //    }

        //    personGrid.Rows = 2 + addRows + 10;
        //}

        private void populatePersonGrids(List<Person> _persons)
        {
            personGrid.Children.Clear();

            for (int i = 0; i < _persons.Count + 5; i++)
            {
                Ellipse personEllipse = new Ellipse();
                personEllipse.Width = 60;
                personEllipse.Height = 60;
                personEllipse.MouseEnter += personEllipse_MouseEnter;
                personEllipse.MouseLeave += personEllipse_MouseLeave;
                personEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Hero.png", UriKind.Absolute)));
                //personEllipse.SetValue(Grid.RowProperty, 1);
                //personEllipse.SetValue(Grid.ColumnProperty, i);

                ToolTip toolTip = new ToolTip();
                ToolTipService.SetInitialShowDelay(toolTip, 100);
                toolTip.Content = _persons[0].Name + ' ' + _persons[0].Surname + " [" + _persons[0].Lvl + ']';
                personEllipse.ToolTip = toolTip;

                personGrid.Children.Add(personEllipse);
            }
        }

        private void personEllipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void personEllipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
