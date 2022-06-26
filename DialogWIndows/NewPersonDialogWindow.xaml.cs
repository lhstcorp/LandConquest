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
    /// Логика взаимодействия для NewPersonDialogWindow.xaml
    /// </summary>
    public partial class NewPersonDialogWindow : Window
    {
        Person newPerson;
        Player player;
        public NewPersonDialogWindow(Player _player)
        {
            newPerson = new Person();
            player = _player;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dynasty dynasty = DynastyModel.GetDynastyByPlayerId(player.PlayerId);

            DynastySurname.Content = dynasty.DynastyName;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            AuthorisationWindow window = new AuthorisationWindow();
            window.Show();
            //this.Close();
        }
        private void createPersonBtn_Click(object sender, RoutedEventArgs e)
        {
            newPerson.PlayerId = player.PlayerId;
            newPerson.PersonId = AuthorisationWindow.GenerateUserId();
            newPerson.Name = personName.Text;
            newPerson.Surname =  (string)DynastySurname.Content;
            newPerson.Lvl = 1;
            newPerson.MaleFemale = false;
            newPerson.Agility = 1;
            newPerson.Intellect = 1;
            newPerson.Health = 30;
            newPerson.Power = 1;

            PersonModel.CreatePerson(newPerson);
            this.Close();
        }

        
        private void generateMaleName()
        {
            Random random = new Random();
            int namePosition = random.Next(0, Names.MaleNames.Length);
            personName.Text = Names.MaleNames[namePosition];
        }
        private void generateFemaleName()
        {
            Random random = new Random();
            int namePosition = random.Next(0, Names.FemaleNames.Length);
            personName.Text = Names.FemaleNames[namePosition];
        }

        private void Male_Checked(object sender, RoutedEventArgs e)
        {
            newPerson.MaleFemale = true;
            generateMaleName();
        }
        private void Female_Checked(object sender, RoutedEventArgs e)
        {
            newPerson.MaleFemale = false;
            generateFemaleName();
        }

        private void personName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //person.Name=File.ReadLines(fileName)
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (newPerson.MaleFemale == true)
            {
                generateMaleName();
            }

            if (newPerson.MaleFemale == false)
            {
                generateFemaleName();
            }
        }

        private void DynastyChange_Click(object sender, RoutedEventArgs e)
        {
            //generateSurname();
        }


        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i < 'A' || i > 'Z';
        }

        private void personName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }
    }
}
