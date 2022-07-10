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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using LandConquest.DialogWIndows;



namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        private Player player;
        private Person selectedPerson;
        public event Action<int> PrestigeChanged;
        int skillPoints;
        private Window openedWindow;
        private List<Person> persons;
        private Border selectedBorder;

        public PersonWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
            initPlayerPersons();
            //CalculateSkillPoints();
            openedWindow = this;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void initPlayerPersons()
        {
            persons = PersonModel.GetPlayerPersons(player.PlayerId);

            if (persons.Count > 0)
            {
                initSelectedPerson(persons[0]);
                initPersonGrid();
            }
            else
            {
                selectedPersonGrid.Visibility = Visibility.Hidden;
            }
            
        }

        private void initSelectedPerson(Person _person)
        {           
            Power.Content = _person.Power;
            Agility.Content = _person.Agility;
            Intelligence.Content = _person.Intellect;
            Experience.Content = _person.Exp;
            MaxExperience.Content = Math.Pow(_person.Lvl, 2) * 500;
            NameSurname.Text = _person.Name +" "+ _person.Surname;
            PbPersonExp.Maximum = Math.Pow(_person.Lvl, 2) * 500;
            PbPersonExp.Value = _person.Exp;
            pbLevel.Text = _person.Lvl.ToString();
            PbPersonHealth.Maximum = 30 + _person.Power * 5 + _person.Agility * 2;
            PbPersonHealth.Value = _person.Health + _person.Power * 5 + _person.Agility * 2;
            PbMaxHealthLabel.Content = 30 + _person.Power * 5 + _person.Agility * 2;
            PbValueHealthLabel.Content = _person.Health + _person.Power * 5 + _person.Agility * 2;
            selectedPerson = _person;

        }

        private void initPersonGrid()
        {
            Thickness defaultBorderThickness = new Thickness(2, 2, 2, 0);
            Thickness personImageMargin = new Thickness(3, 3, 0, 0);
            Thickness powerImageMargin = new Thickness(66, 34, 0, 0);
            Thickness agilityImageMargin = new Thickness(97, 34, 0, 0);
            Thickness intelligenceImageMargin = new Thickness(128, 34, 0, 0);

            BitmapImage powerImageSource = new BitmapImage(new Uri("/Pictures/WarriorSkill_44.png", UriKind.Relative));
            BitmapImage agilityImageSource = new BitmapImage(new Uri("/Pictures/Archerskill_16.png", UriKind.Relative));
            BitmapImage intelligenceImageSource = new BitmapImage(new Uri("/Pictures/Engineerskill_15.png", UriKind.Relative));

            for (int i = 0; i < persons.Count; i++)
            {
                Border border = new Border();                
                if (i < persons.Count - 1)
                {
                    border.BorderThickness = defaultBorderThickness;
                }
                else
                {
                    border.BorderThickness = new Thickness(2, 2, 2, 2);
                }
                border.Width = 164;
                border.Height = 69;
                if (i == 0)
                {
                    selectedBorder = border;
                    border.BorderBrush = Brushes.Brown;
                }
                else
                {
                    border.BorderBrush = Brushes.Black;
                }
                border.Tag = persons[i].PersonId;
                border.MouseEnter += element_MouseEnter;
                border.MouseLeave += element_MouseLeave;
                border.MouseDown += border_MouseDown;
                personGrid.Children.Add(border);

                Grid grid = new Grid();
                grid.VerticalAlignment = VerticalAlignment.Top;
                grid.HorizontalAlignment = HorizontalAlignment.Right;
                grid.Width = 161;
                grid.Height = 66;
                grid.Background = new LinearGradientBrush(
                                        Color.FromArgb(255, 60, 38, 38),   // Opaque red
                                        Color.FromArgb(255, 128, 128, 94),
                                        new Point(0.5, 0),
                                        new Point(0.5, 1));
                border.Child = grid;

                Image personImage = new Image();
                personImage.Source = new BitmapImage(new Uri("pack://application:,,,/Pictures/Hero.png", UriKind.Absolute)); // For Savva TODO
                personImage.HorizontalAlignment = HorizontalAlignment.Left;
                personImage.VerticalAlignment = VerticalAlignment.Top;
                personImage.Width = 60;
                personImage.Height = 60;
                personImage.Stretch = Stretch.UniformToFill;
                personImage.Margin = personImageMargin;
                grid.Children.Add(personImage);

                Image powerImage = new Image();
                powerImage.Source = powerImageSource;
                powerImage.HorizontalAlignment = HorizontalAlignment.Left;
                powerImage.VerticalAlignment = VerticalAlignment.Top;
                powerImage.Width = 29;
                powerImage.Height = 29;
                powerImage.Stretch = Stretch.UniformToFill;
                powerImage.Margin = new Thickness(66, 34, 0, 0);
                grid.Children.Add(powerImage);

                Image agilityImage = new Image();
                agilityImage.Source = agilityImageSource;
                agilityImage.HorizontalAlignment = HorizontalAlignment.Left;
                agilityImage.VerticalAlignment = VerticalAlignment.Top;
                agilityImage.Width = 29;
                agilityImage.Height = 29;
                agilityImage.Stretch = Stretch.UniformToFill;
                agilityImage.Margin = new Thickness(97, 34, 0, 0);
                grid.Children.Add(agilityImage);

                Image intelligenceImage = new Image();
                intelligenceImage.Source = intelligenceImageSource;
                intelligenceImage.HorizontalAlignment = HorizontalAlignment.Left;
                intelligenceImage.VerticalAlignment = VerticalAlignment.Top;
                intelligenceImage.Width = 29;
                intelligenceImage.Height = 29;
                intelligenceImage.Stretch = Stretch.UniformToFill;
                intelligenceImage.Margin = new Thickness(128, 34, 0, 0);
                grid.Children.Add(intelligenceImage);

                Label powerLabel = new Label();
                powerLabel.Foreground = Brushes.White;
                powerLabel.FontWeight = FontWeights.SemiBold;
                powerLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                powerLabel.HorizontalAlignment = HorizontalAlignment.Left;
                powerLabel.VerticalContentAlignment = VerticalAlignment.Center;
                powerLabel.VerticalAlignment = VerticalAlignment.Top;
                powerLabel.Margin = powerImageMargin;
                powerLabel.Width = 29;
                powerLabel.Height = 29;
                powerLabel.Content = persons[i].Power;
                grid.Children.Add(powerLabel);

                Label agilityLabel = new Label();
                agilityLabel.Foreground = Brushes.White;
                agilityLabel.FontWeight = FontWeights.SemiBold;
                agilityLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                agilityLabel.HorizontalAlignment = HorizontalAlignment.Left;
                agilityLabel.VerticalContentAlignment = VerticalAlignment.Center;
                agilityLabel.VerticalAlignment = VerticalAlignment.Top;
                agilityLabel.Margin = agilityImageMargin;
                agilityLabel.Width = 29;
                agilityLabel.Height = 29;
                agilityLabel.Content = persons[i].Agility;
                grid.Children.Add(agilityLabel);

                Label intelligenceLabel = new Label();
                intelligenceLabel.Foreground = Brushes.White;
                intelligenceLabel.FontWeight = FontWeights.SemiBold;
                intelligenceLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                intelligenceLabel.HorizontalAlignment = HorizontalAlignment.Left;
                intelligenceLabel.VerticalContentAlignment = VerticalAlignment.Center;
                intelligenceLabel.VerticalAlignment = VerticalAlignment.Top;
                intelligenceLabel.Margin = intelligenceImageMargin;
                intelligenceLabel.Width = 29;
                intelligenceLabel.Height = 29;
                intelligenceLabel.Content = persons[i].Intellect;
                grid.Children.Add(intelligenceLabel);
            }
        }

        private void BtnUpgradePersonPower_Click(object sender, RoutedEventArgs e)
        {
            Person person = PersonModel.GetPersonInfo(selectedPerson.PersonId);
            if (player.PlayerPrestige > person.Power && skillPoints > 0)
            {
                person.Power += 1;
                player.PlayerPrestige -= person.Power;
            //    Console.WriteLine(person.Lvl + " LEVEL");
            //    Console.WriteLine(player.PlayerPrestige + " Prestige");
            }
            else if (player.PlayerPrestige < person.Power)
            {
                WarningDialogWindow.CallWarningDialogNoResult("No enough Prestige");
            }
            //else
            //{
            //    BtnUpgradePersonPower.IsEnabled = false;
            //}
            //Power.Content = person.Power;
            PersonModel.UpdatePersonPower(player, person);
            PersonModel.UpdatePersonHealth(player, person);
            PlayerModel.UpdatePlayerPrestige(player);
            CalculateSkillPoints();
            initSelectedPerson(selectedPerson);
            int prestige = player.PlayerPrestige;
           
            if (PrestigeChanged != null)
            {
                PrestigeChanged(prestige);
            }

        }

        private void BtnUpgradePersonAgility_Click(object sender, RoutedEventArgs e)
        {
            Person person =  PersonModel.GetPersonInfo(selectedPerson.PersonId);
            if (player.PlayerPrestige > person.Agility && skillPoints > 0)
            {
                person.Agility += 1;
                player.PlayerPrestige -= person.Agility;
                //    Console.WriteLine(person.Lvl + " LEVEL");
                //    Console.WriteLine(player.PlayerPrestige + " Prestige");
            }
            else if (player.PlayerPrestige < person.Agility)
            {
                WarningDialogWindow.CallWarningDialogNoResult("No enough Prestige");
            }
            //else
            //{
            //    BtnUpgradePersonAgility.IsEnabled = false;
            //}
            //Agility.Content = person.Agility;
            PersonModel.UpdatePersonAgility(player, person);
            PersonModel.UpdatePersonHealth(player, person);
            PlayerModel.UpdatePlayerPrestige(player);
            CalculateSkillPoints();
            initSelectedPerson(selectedPerson);
            int prestige = player.PlayerPrestige;

            if (PrestigeChanged != null)
            {
                PrestigeChanged(prestige);
            }

        }

        private void BtnUpgradePersonIntellect_Click(object sender, RoutedEventArgs e)
        {
            Person person = PersonModel.GetPersonInfo(selectedPerson.PersonId);
            if (player.PlayerPrestige > person.Intellect && skillPoints>0)
            {
                person.Intellect += 1;
                player.PlayerPrestige -= person.Intellect;
                //    Console.WriteLine(person.Lvl + " LEVEL");
                //    Console.WriteLine(player.PlayerPrestige + " Prestige");
            }
            else if (player.PlayerPrestige<person.Intellect)
            {
                WarningDialogWindow.CallWarningDialogNoResult("No enough Prestige");
            }
            //else
            //{
            //    BtnUpgradePersonIntellect.IsEnabled = false;             
            //}
            //Intelligence.Content = person.Intellect;
            PersonModel.UpdatePersonIntellect(player, person);
            PersonModel.UpdatePersonHealth(player, person);
            PlayerModel.UpdatePlayerPrestige(player);
            CalculateSkillPoints();

            initSelectedPerson(selectedPerson);
            int prestige = player.PlayerPrestige;

            if (PrestigeChanged != null)
            {
                PrestigeChanged(prestige);
            }

        }

        private void CalculateSkillPoints()
        {
            Person person =  PersonModel.GetPersonInfo(selectedPerson.PersonId);
            SkillPoints.Content = person.Lvl*3 - person.Power - person.Agility - person.Intellect;
            skillPoints = Convert.ToInt32(SkillPoints.Content);
            if (skillPoints < 1) 
            {
                WarningDialogWindow.CallWarningDialogNoResult("No enough Skill Points");
                //BtnUpgradePersonAgility.IsEnabled = false;               
                //BtnUpgradePersonIntellect.IsEnabled = false;
                //BtnUpgradePersonPower.IsEnabled = false;
            }

            
        }
        private void CloseUnusedWindows()
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window != this)
                    window.Close();
            }
        }

        private void FreeData(object data, EventArgs e)
        {
            openedWindow = null;
            Activate();
            GC.Collect();
        }

        private void ButtonCreatePerson_Click(object sender, RoutedEventArgs e)
        {
            //CloseUnusedWindows();
            openedWindow = new NewPersonDialogWindow(player);

            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void element_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void element_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            Person person = persons.Find(o => o.PersonId == border.Tag.ToString());

            initSelectedPerson(person);

            selectedBorder.BorderBrush = Brushes.Black;
            selectedBorder = border;
            selectedBorder.BorderBrush = Brushes.Brown;
        }
    }
}
