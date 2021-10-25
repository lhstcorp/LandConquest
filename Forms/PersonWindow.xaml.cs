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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        Player player;

        public PersonWindow(Player _player)
        {
            player = _player; 
            InitializeComponent();
            InitPersonCharacteristics();
        }
       
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void InitPersonCharacteristics()
        {
            Person person = new Person();
            person = PersonModel.GetPersonInfo(player, person);
            Power.Content = person.Power;
            Agility.Content = person.Agility;
            Intelligence.Content = person.Intellect;
            Level.Content = person.Lvl;
            Health.Content = person.Health;
            Experience.Content = person.Exp;
            NameSurname.Content = person.Name +" "+ person.Surname;
        }
    }
}
