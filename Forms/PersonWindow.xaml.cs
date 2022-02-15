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
        private User user;
        private Player player;
        private Person person;
        public event Action<int> PrestigeChanged;
        int skillPoints;
        private Window openedWindow;




        public PersonWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
            InitPersonCharacteristics();
            CalculateSkillPoints();
            openedWindow = this;
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
            Health.Content = person.Health + person.Power * 5 + person.Agility * 2;
            Experience.Content = person.Exp;
            NameSurname.Content = person.Name +" "+ person.Surname;
            PbPersonExp.Maximum = Math.Pow(person.Lvl, 2) * 500;
            PbPersonExp.Value = person.Exp;
            pbLevel.Content = person.Lvl;
            pbLevel1.Content = person.Lvl + 1;
            PbPersonHealth.Maximum = 30 + person.Power * 5 + person.Agility * 2;
            PbPersonHealth.Value = person.Health + person.Power * 5 + person.Agility * 2;
            PbMaxHealthLabel.Content = 30 + person.Power * 5 + person.Agility * 2;
            PbValueHealthLabel.Content = person.Health + person.Power * 5 + person.Agility * 2;

        }

        private void BtnUpgradePersonPower_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();
            person = PersonModel.GetPersonInfo(player, person);
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
            InitPersonCharacteristics();
            int prestige = player.PlayerPrestige;
           
            if (PrestigeChanged != null)
            {
                PrestigeChanged(prestige);
            }

        }

        private void BtnUpgradePersonAgility_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();
            person = PersonModel.GetPersonInfo(player, person);
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
            InitPersonCharacteristics();
            int prestige = player.PlayerPrestige;

            if (PrestigeChanged != null)
            {
                PrestigeChanged(prestige);
            }

        }

        private void BtnUpgradePersonIntellect_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();
            person = PersonModel.GetPersonInfo(player, person);
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

            InitPersonCharacteristics();
            int prestige = player.PlayerPrestige;

            if (PrestigeChanged != null)
            {
                PrestigeChanged(prestige);
            }

        }

        private void CalculateSkillPoints()
        {
            Person person = new Person();
            person = PersonModel.GetPersonInfo(player, person);
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
            CloseUnusedWindows();
            NewPersonDialogWindow newPersonDialogWindow = new NewPersonDialogWindow(user);
            
            newPersonDialogWindow.Owner = this;
            newPersonDialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newPersonDialogWindow.Show();
            newPersonDialogWindow.Closed += FreeData;
        }
    }
}
