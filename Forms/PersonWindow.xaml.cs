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
        private Person person;
        public event Action<int> PrestigeChanged;





        public PersonWindow(Player _player)
        {           
            InitializeComponent();
            player = _player;
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

        private void BtnUpgradePersonLevel_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();
            person = PersonModel.GetPersonInfo(player, person);
            if (player.PlayerPrestige > person.Lvl)
            {
                person.Lvl += 1;
                player.PlayerPrestige -= person.Lvl;
            //    Console.WriteLine(person.Lvl + " LEVEL");
            //    Console.WriteLine(player.PlayerPrestige + " Prestige");
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough prestige!"); 
            }
            Level.Content = person.Lvl;
            PersonModel.UpdatePersonLvl(player, person);
            PlayerModel.UpdatePlayerPrestige(player);          
            
            InitPersonCharacteristics();
            int prestige = player.PlayerPrestige;
           
            if (PrestigeChanged != null)
            {

                PrestigeChanged(prestige);
            }

        }
    }
}
