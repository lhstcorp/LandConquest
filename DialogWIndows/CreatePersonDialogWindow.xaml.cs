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
        }
        private void Female_Checked(object sender, RoutedEventArgs e)
        {
            person.MaleFemale = false;
        }

        private void personName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //person.Name=File.ReadLines(fileName)
        }
    }
}
