using LandConquest.DialogWIndows;
using LandConquest.Logic;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LandConquest.Forms
{
    public partial class CountryWindow : Window
    { 
        private Player player;
        private List<Land> countryLands;
        private List<Land> countryLandsToFight;
        private List<Land> capitalCountry;
        private List<Country> capitals;
        private List<Country> countries;
        private Land selectedLand;
        private Country transferCapital;
        private Country transferCountry;
        private Land countryLandDefender;
        private int operation = 0;
        private bool f = true;
        private Player ruler;

        private Person selectedPerson;
        private List<Person> playerPersons;
        private Ellipse selectedEllipse;

        Thickness defaultEllipseMargin = new Thickness(3, 3, 0, 0);

        //////////////////
        private Country currentCountry;
        private string currentChosenCapitalName;
        private List<string> potentialCapitalsNamesListing;
        //////////////////


        public CountryWindow(Player _player)
        {
            player = _player;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ///////////////////////////////
            currentCountry = CountryModel.GetCountryById(CountryModel.GetCountryIdByLandId(player.PlayerCurrentRegion));
            currentChosenCapitalName = LandModel.GetLandInfo(currentCountry.CapitalId).LandName;
            //////////////////////////////

            Country country = CountryModel.GetCountryById(CountryModel.GetCountryIdByLandId(player.PlayerCurrentRegion));

            populatePlayerPersonList();
            populatePlayerPersonGrid();

            populateActiveLawGrid();

            //ruler = new Player();
            //User rulerUser = new User();
            //rulerUser.UserId = country.CountryRuler;
            //ruler = PlayerModel.GetPlayerById(rulerUser.UserId);
            //RulerNameLbl.Content = ruler.PlayerName;
            //CountryNameLbl.Content = country.CountryName;
            //CapitalNameLbl.Content = country.CapitalId;

            countryLands = LandModel.GetCountryLands(country.CountryId);

            int count = CountryModel.SelectLastIdOfStates();

            countries = CountryModel.GetCountriesInfo();

            ////////////////////
            potentialCapitalsNamesListing = new List<string>();
            potentialCapitalsNamesListing = CountryModel.GetCountryLandsNamesNotWarInvolved(currentCountry);
            CbCapitalToTransfer.ItemsSource = potentialCapitalsNamesListing;
            /////////////////////
        }

        private void populateActiveLawGrid()
        {
            /// consts
            Thickness defaultBorderMargin = new Thickness(2, 2, 2, 0);
            Thickness defaultBorderThickness = new Thickness(1);
            Thickness defaultRectangleMargin = new Thickness(1, 1, 0, 0);
            Thickness defaultCountryNameVBMargin = new Thickness(63, 0, 5, 32.4);
            Thickness defaultEllipseMargin = new Thickness(40, 26, 0, -3.6);
            Thickness defaultInitializedByVieboxMargin = new Thickness(74, 29, 0, 7.4);
            Thickness defaultLine1Margin = new Thickness(63, 22, 0, 0);
            Thickness defaultLine2Margin = new Thickness(181.5, 29.5, 0, 0);
            Thickness defaultLine3Margin = new Thickness(275, 29.5, 0, 0);
            Thickness defaultInitializedByPersonNameVieboxMargin = new Thickness(73, 42, 165, -5.6);
            Thickness defaulthoursLeftVieboxMargin = new Thickness(183, 22, 69, -11.6);
            Point defaultGradientStartPoint = new Point(0.5, 0);
            Point defaultGradientEndPoint = new Point(0.5, 1);
            Thickness defaultApproveLawButtonMargin = new Thickness(312, 30, 0, 0);
            Thickness defaultDeclineLawButtonMargin = new Thickness(285, 34, 0, 0);
            ///

            List<Law> activeLaws = LawModel.getCountryLaws(currentCountry.CountryId);

            /// creation laws holders
            for (int i = 0; i < activeLaws.Count; i++)
            {
                Border border = new Border();
                if (i < activeLaws.Count - 1)
                {
                    border.Margin = defaultBorderMargin;
                }
                else
                {
                    border.Margin = new Thickness(2, 2, 2, 2);
                }
                border.BorderThickness = defaultBorderThickness;
                border.Width = 341.5;
                border.Height = 60;
                border.BorderBrush = Brushes.Black;
                activeLawGrid.Children.Add(border);

                Grid grid = new Grid();
                border.Child = grid;
                grid.Background = new LinearGradientBrush(Color.FromRgb(188, 168, 142), Color.FromRgb(223, 211, 152), defaultGradientStartPoint, defaultGradientEndPoint);

                Rectangle rectangle = new Rectangle();
                rectangle.Width = 57;
                rectangle.Height = 57;
                rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                rectangle.VerticalAlignment = VerticalAlignment.Top;
                rectangle.Margin = defaultRectangleMargin;
                rectangle.Stroke = Brushes.Black;
                rectangle.Stretch = Stretch.Fill;
                rectangle.Fill = getLawIcon(activeLaws[i].Operation);
                grid.Children.Add(rectangle);

                Viewbox countryNameVB = new Viewbox();
                countryNameVB.Margin = defaultCountryNameVBMargin;
                grid.Children.Add(countryNameVB);

                Label countryNameLbl = new Label();
                countryNameLbl.FontFamily = new FontFamily("Papyrus");
                countryNameLbl.FontWeight = FontWeights.Bold;
                countryNameLbl.Content = getLawTitle(activeLaws[i]);
                countryNameVB.Child = countryNameLbl;

                Ellipse ellipse = new Ellipse();
                ellipse.Height = 36;
                ellipse.Width = 36;
                ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                ellipse.VerticalAlignment = VerticalAlignment.Top;
                ellipse.Stroke = Brushes.Black;
                ellipse.StrokeThickness = 1;
                ellipse.Margin = defaultEllipseMargin;
                ellipse.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Hero.png", UriKind.Absolute)));
                grid.Children.Add(ellipse);

                Viewbox initializedByViebox = new Viewbox();
                initializedByViebox.Margin = defaultInitializedByVieboxMargin;
                initializedByViebox.Width = 109;
                initializedByViebox.HorizontalAlignment = HorizontalAlignment.Left;
                grid.Children.Add(initializedByViebox);

                Label initializedByLabel = new Label();
                initializedByLabel.Content = Languages.Resources.LocLabelInitializedBy_Content;
                initializedByLabel.FontFamily = new FontFamily("Papyrus");
                initializedByLabel.FontWeight = FontWeights.Bold;
                initializedByViebox.Child = initializedByLabel;

                Rectangle line1 = new Rectangle();
                line1.Height = 1;
                line1.Width = 272;
                line1.HorizontalAlignment = HorizontalAlignment.Left;
                line1.VerticalAlignment = VerticalAlignment.Top;
                line1.Margin = defaultLine1Margin;
                line1.StrokeThickness = 1;
                line1.Stroke = Brushes.Black;
                grid.Children.Add(line1);

                Rectangle line2 = new Rectangle();
                line2.Height = 25;
                line2.Width = 1;
                line2.HorizontalAlignment = HorizontalAlignment.Left;
                line2.VerticalAlignment = VerticalAlignment.Top;
                line2.Margin = defaultLine2Margin;
                line2.StrokeThickness = 1;
                line2.Stroke = Brushes.Black;
                grid.Children.Add(line2);

                Rectangle line3 = new Rectangle();
                line3.Height = 25;
                line3.Width = 1;
                line3.HorizontalAlignment = HorizontalAlignment.Left;
                line3.VerticalAlignment = VerticalAlignment.Top;
                line3.Margin = defaultLine3Margin;
                line3.StrokeThickness = 1;
                line3.Stroke = Brushes.Black;
                grid.Children.Add(line3);

                Viewbox initializedByPersonNameViebox = new Viewbox();
                initializedByPersonNameViebox.Margin = defaultInitializedByPersonNameVieboxMargin;
                initializedByPersonNameViebox.HorizontalAlignment = HorizontalAlignment.Left;
                grid.Children.Add(initializedByPersonNameViebox);

                Label initializedByPersonNameLabel = new Label();
                initializedByPersonNameLabel.Content = PersonModel.GetPersonNameSurnameById(activeLaws[i].PersonId);
                initializedByPersonNameLabel.FontFamily = new FontFamily("Papyrus");
                initializedByPersonNameLabel.FontWeight = FontWeights.Bold;
                initializedByPersonNameViebox.Child = initializedByPersonNameLabel;

                Viewbox hoursLeftViebox = new Viewbox();
                hoursLeftViebox.Margin = defaulthoursLeftVieboxMargin;
                hoursLeftViebox.HorizontalAlignment = HorizontalAlignment.Left;
                grid.Children.Add(hoursLeftViebox);

                Label hoursLeftLabel = new Label();
                hoursLeftLabel.Content = activeLaws[i].InitDateTime;
                hoursLeftLabel.FontFamily = new FontFamily("Papyrus");
                hoursLeftLabel.FontWeight = FontWeights.Bold;
                hoursLeftViebox.Child = hoursLeftLabel;

                DispatcherTimer _lawTimer = new DispatcherTimer(); // :(
                TimeSpan _lawTime;

                _lawTime = activeLaws[i].InitDateTime.AddHours(12).Subtract(DateTime.UtcNow).StripMilliseconds();
                _lawTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    hoursLeftLabel.Content = _lawTime.ToString("c");
                    if (_lawTime <= TimeSpan.Zero)
                    {
                        _lawTimer.Stop();
                        hoursLeftLabel.Content = "00:00:00";
                    }
                    else
                    {
                        _lawTime = _lawTime.Add(TimeSpan.FromSeconds(-1));
                    }
                }, Application.Current.Dispatcher);

                _lawTimer.Start();

                Image approveLawImgBtn = new Image();
                approveLawImgBtn.Source = new BitmapImage(new Uri("/Pictures/Country/greenDarkArrow.png", UriKind.Relative));
                approveLawImgBtn.Stretch = Stretch.UniformToFill;
                approveLawImgBtn.Width = 17;
                approveLawImgBtn.Height = 21;
                approveLawImgBtn.HorizontalAlignment = HorizontalAlignment.Left;
                approveLawImgBtn.VerticalAlignment = VerticalAlignment.Top;
                approveLawImgBtn.Margin = defaultApproveLawButtonMargin;
                approveLawImgBtn.MouseEnter += approveLawImgBtn_MouseEnter;
                approveLawImgBtn.MouseLeave += approveLawImgBtn_MouseLeave;
                approveLawImgBtn.MouseDown += approveLawImgBtn_MouseDown;
                approveLawImgBtn.Tag = activeLaws[i].LawId;
                grid.Children.Add(approveLawImgBtn);

                Image declineLawImgBtn = new Image();
                declineLawImgBtn.Source = new BitmapImage(new Uri("/Pictures/Country/redDarkCross.png", UriKind.Relative));
                declineLawImgBtn.Stretch = Stretch.UniformToFill;
                declineLawImgBtn.Width = 15;
                declineLawImgBtn.Height = 19;
                declineLawImgBtn.HorizontalAlignment = HorizontalAlignment.Left;
                declineLawImgBtn.VerticalAlignment = VerticalAlignment.Top;
                declineLawImgBtn.Margin = defaultDeclineLawButtonMargin;
                declineLawImgBtn.MouseEnter += declineLawImgBtn_MouseEnter;
                declineLawImgBtn.MouseLeave += declineLawImgBtn_MouseLeave;
                declineLawImgBtn.MouseDown += declineLawImgBtn_MouseDown;
                declineLawImgBtn.Tag = activeLaws[i].LawId;
                grid.Children.Add(declineLawImgBtn);
            }
        }

        private void populatePlayerPersonList()
        {
            playerPersons = PersonModel.GetPlayerPersons(player.PlayerId);
        }

        private void populatePlayerPersonGrid()
        {
            if (playerPersons.Count > 0)
            {
                selectedPersonEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Hero.png", UriKind.Absolute)));
                selectedPersonEllipse.Tag = playerPersons[0].PersonId;
                selectedPlayerPersonName.Content = playerPersons[0].Name + ' ' + playerPersons[0].Surname;
                selectedPerson = playerPersons[0];

                createPlayerPersonEllipse();
            }
            else
            {
                //selectedPersonEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("/Pictures/epic_brown_square.png", UriKind.Relative)));
                selectedPlayerPersonName.Content = "No person";
            }
        }

        private void createPlayerPersonEllipse()
        {
            playerPersonGrid.Children.Clear();

            for (int i = 0; i < playerPersons.Count; i++)
            {
                Ellipse personEllipse = new Ellipse();
                personEllipse.Width = 60;
                personEllipse.Height = 60;
                personEllipse.Margin = defaultEllipseMargin;
                personEllipse.MouseEnter += personEllipse_MouseEnter;
                personEllipse.MouseLeave += personEllipse_MouseLeave;
                personEllipse.MouseDown += personEllipse_MouseDown;
                personEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Hero.png", UriKind.Absolute))); // person image
                personEllipse.Tag = playerPersons[i].PersonId;

                ToolTip toolTip = new ToolTip();
                ToolTipService.SetInitialShowDelay(toolTip, 100);
                toolTip.Content = playerPersons[i].Name + ' ' + playerPersons[i].Surname + " [" + playerPersons[i].Lvl + ']';
                personEllipse.ToolTip = toolTip;

                if (i == 0) // mark first ellipse as selected;
                {
                    selectedEllipse = personEllipse;
                    selectedEllipse.Stroke = Brushes.Brown;
                }

                playerPersonGrid.Children.Add(personEllipse);
            }
        }

        private void personEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse personEllipse = (Ellipse)sender;
            selectedPerson = playerPersons.Find(o => o.PersonId == personEllipse.Tag.ToString());

            selectedPersonEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Hero.png", UriKind.Absolute)));
            selectedPlayerPersonName.Content = selectedPerson.Name + ' ' + selectedPerson.Surname;

            selectedEllipse.Stroke = Brushes.Black;
            selectedEllipse = personEllipse;
            selectedEllipse.Stroke = Brushes.Brown;
        }

        private void personEllipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void personEllipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private ImageBrush getLawIcon(int _lawType)
        {
            ImageBrush lawIcon = new ImageBrush();

            switch (_lawType)
            {
                case 1:
                    lawIcon = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Country/CountryMenu/renameCountry.png", UriKind.Absolute)));
                    break;
                case 2:
                    lawIcon = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Country/CountryMenu/transferLand.png", UriKind.Absolute)));
                    break;
                case 3:
                    lawIcon = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Country/CountryMenu/makePact.png", UriKind.Absolute)));
                    break;
                case 4:
                    lawIcon = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Pictures/Country/CountryMenu/declareWar.png", UriKind.Absolute)));
                    break;
            }

            return lawIcon;
        }

        private string getLawTitle(Law _law)
        {
            string lawTitle = "";

            switch (_law.Operation)
            {
                case 1:
                    lawTitle = String.Format(Languages.Resources.LocLabelRenameStateTo_Text, _law.Value1); 
                    break;
                case 2:
                    lawTitle = String.Format(Languages.Resources.LocLabelRenameStateTo_Text, _law.Value1);
                    break;
                case 3:
                    lawTitle = String.Format(Languages.Resources.LocLabelRenameStateTo_Text, _law.Value1);
                    break;
                case 4:
                    lawTitle = String.Format(Languages.Resources.LocLabelRenameStateTo_Text, _law.Value1);
                    break;
            }

            return lawTitle;
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
            }

        }

        private void IssueALaw_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerId == ruler.PlayerId)
            {
                switch (operation)
                {
                    case 1:
                        {
                            selectedLand = LandModel.GetLandInfo(selectedLand.LandId);

                            Country ThisCountry = CountryModel.GetCountryById(selectedLand.CountryId);

                            LandModel.UpdateLandInfo(selectedLand, transferCountry);

                            countryLands = LandModel.GetCountryLands(ThisCountry.CountryId);
                            if (countryLands.Count == 0)
                            {
                                CountryModel.DisbandCountry(ThisCountry);
                            }
                            //тут нужно написать функцию на чек пустых государств. Если гос-во пустое - король теряет свой титул.
                            break;
                        }
                    case 2:
                        {
                            WarLogic warModel = new WarLogic();
                            WarModel.DeclareAWar(GenerateId(), selectedLand, countryLandDefender);

                            break;
                        }
                }
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Only a ruler of the country can issue a law!");
            }
        }

        private void CbLandToTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedLand = new Land();
            selectedLand = countryLands[CbLandToTransfer.SelectedIndex];
        }
       
        private void CbCapitalToTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //transferCapital = new Country();
            //transferCapital = capitals[CbCapitalToTransfer.SelectedIndex];
            //Console.WriteLine(transferCapital.CapitalId);
            //CbCountryWarLand.Items.Clear();
            //countryLandsToFight = LandModel.GetCountryLands(transferCapital);
            //for (int i = 0; i < countryLandsToFight.Count; i++)
            //{
            //    CbCountryWarLand.Items.Add(countryLandsToFight[i].LandName);
            //    Console.WriteLine(i);
            //}

            ///////////////////////////////////
            currentChosenCapitalName = (string)CbCapitalToTransfer.SelectedItem;
            ///////////////////////////////////
        }

        private void buttonChangeCapital_Click(object sender, RoutedEventArgs e)
        {
            /////////////////////////////////////////
            CountryModel.UpdateCountryCapital(currentCountry, LandModel.GetLandInfoByName(currentChosenCapitalName).LandId);
            //////////////////////////////////////////
        }

        private void CbCountryToTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            transferCountry = new Country();

            transferCountry = countries[CbCountryToTransfer.SelectedIndex];
            Console.WriteLine(transferCountry.CountryName);

            if (operation == 2)
            {
                CbCountryWarLand.Items.Clear();

                countryLandsToFight = LandModel.GetCountryLands(transferCountry.CountryId);

                for (int i = 0; i < countryLandsToFight.Count; i++)
                {
                    CbCountryWarLand.Items.Add(countryLandsToFight[i].LandName);
                    Console.WriteLine(i);
                }
            }
        }

        private void CbCountryWarLand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //countryLandsToFight = new List<Land>();
            if (CbCountryWarLand.SelectedIndex != -1)
                countryLandDefender = countryLandsToFight[CbCountryWarLand.SelectedIndex]; // STAR!
        }

        private static Random random;
        public static string GenerateId()
        {
            Thread.Sleep(15);
            random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void renameStateBtn_Click(object sender, RoutedEventArgs e)
        {
            RenameStateDialogWindow renameStateDialogWindow = new RenameStateDialogWindow(currentCountry, selectedPerson);

            renameStateDialogWindow.Show();
            renameStateDialogWindow.Owner = this;
            renameStateDialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void approveLawImgBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = (Image)sender;

            img.Source = new BitmapImage(new Uri("/Pictures/Country/greenLightArrow.png", UriKind.Relative));

            Cursor = Cursors.Hand;
        }

        private void approveLawImgBtn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = (Image)sender;

            img.Source = new BitmapImage(new Uri("/Pictures/Country/greenDarkArrow.png", UriKind.Relative));

            Cursor = Cursors.Arrow;
        }

        private void declineLawImgBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;

            img.Source = new BitmapImage(new Uri("/Pictures/Country/redLightCross.png", UriKind.Relative));

            Cursor = Cursors.Hand;
        }

        private void declineLawImgBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;

            img.Source = new BitmapImage(new Uri("/Pictures/Country/redDarkCross.png", UriKind.Relative));

            Cursor = Cursors.Arrow;
        }

        private void declineLawImgBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LawVote vote = new LawVote();
            vote.LawId = ((Image)sender).Tag.ToString();
            vote.PersonId = selectedPerson.PersonId;
            vote.PlayerId = selectedPerson.PlayerId;
            vote.VoteValue = -1; // Vote value weight.
            deletePersonVote(vote.LawId);
            LawModel.insertLawVote(vote);
        }

        private void approveLawImgBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LawVote vote = new LawVote();
            vote.LawId = ((Image)sender).Tag.ToString();
            vote.PersonId = selectedPerson.PersonId;
            vote.PlayerId = selectedPerson.PlayerId;
            vote.VoteValue = 1; // Vote value weight.
            deletePersonVote(vote.LawId);
            LawModel.insertLawVote(vote);
        }

        private void deletePersonVote(string _lawId)
        {
            LawModel.deletePersonLawVote(_lawId, selectedPerson.PersonId);
        }
    }
}
