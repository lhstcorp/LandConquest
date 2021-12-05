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
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.DialogWIndows
{
    /// <summary>
    /// Логика взаимодействия для CreatePersonDialogWindow.xaml
    /// </summary>
    public partial class CreatePersonDialogWindow : Window
    {
        User user;
        Person person;
        
        public CreatePersonDialogWindow(User _user)
        {
            user = _user;
            person = new Person();
            InitializeComponent();
            generateMaleName();
            //generateSurname()!
        }

        private void generateSurname()
        {
            Random random = new Random();
            int namePosition = random.Next(0, Names.Surnames.Length);
            Person person;// = PersonModel.NotExistsBySurname();
            if (true)// TODO
            {

            }
            else
            {
                //generateSurname();
            }
            Dynasty.Text = Names.Surnames[namePosition];
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

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            AuthorisationWindow window = new AuthorisationWindow();
            window.Show();
            this.Close();
        }

        private void createPersonBtn_Click(object sender, RoutedEventArgs e)
        {
            person.PlayerId = user.UserId;
            person.PersonId = AuthorisationWindow.GenerateUserId();
            person.Name = personName.Text;
            person.Surname = Dynasty.Text;
            person.MaleFemale = false;
            person.Agility = 1;
            person.Intellect = 1;
            person.Health = 1;
            person.Power = 1;

            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }
     
        private void Male_Checked(object sender, RoutedEventArgs e)
        {
            person.MaleFemale = true;
            generateMaleName();
        }
        private void Female_Checked(object sender, RoutedEventArgs e)
        {
            person.MaleFemale = false;
            generateFemaleName();
        }

        private void personName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //person.Name=File.ReadLines(fileName)
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (person.MaleFemale == true)
            {
                generateMaleName();
            }

            if (person.MaleFemale == false)
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
            return int.TryParse(str, out i) &&  i < 'A' || i > 'Z';
        }

        private void personName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {    
           e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }
    }
}
