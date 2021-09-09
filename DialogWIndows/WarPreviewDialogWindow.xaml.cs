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
    /// Логика взаимодействия для WarPreviewDialogWindow.xaml
    /// </summary>
    public partial class WarPreviewDialogWindow : Window
    {
        Player player;
        War war;
        public WarPreviewDialogWindow(Player _player, War _war)
        {
            player = _player;
            war = _war;

            InitializeComponent();
            initWar();
            initWarCaption();
            initPlayerGrids();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void initWar()
        {
            war = WarModel.GetWarById(war);
        }

        private void initWarCaption()
        {
            Country countryAttacker = CountryModel.GetCountryById(LandModel.GetLandInfo(war.LandAttackerId).CountryId);
            Country countryDefender = CountryModel.GetCountryById(LandModel.GetLandInfo(war.LandDefenderId).CountryId);

            warCaptionLbl.Content += countryAttacker.CountryName + " & " + countryDefender.CountryName;

            CountryAttackerName.Content = countryAttacker.CountryName;
            CountryDefenderName.Content = countryDefender.CountryName;
        }

        private void initPlayerGrids()
        {
            List<ArmyInBattle> armies = new List<ArmyInBattle>();
            armies = BattleModel.GetArmiesInfo(armies, war);

            List<ArmyInBattle> armiesA = new List<ArmyInBattle>();
            List<ArmyInBattle> armiesD = new List<ArmyInBattle>();

            for (int i = 0; i < armies.Count; i++)
            {
                if (armies[i].ArmySide == 0)
                {
                    armiesD.Add(armies[i]);
                }
                else
                {
                    armiesA.Add(armies[i]);
                }
            }

            populateTroopsData(armiesA, armiesD);

            armiesD = armiesD.GroupBy(x => x.PlayerId).Select(x => x.First()).ToList();
            armiesA = armiesA.GroupBy(x => x.PlayerId).Select(x => x.First()).ToList();

            initPlayerGridsSize(armiesA, armiesD);
            populatePlayerGrids(armiesA, armiesD);
        }

        private void initPlayerGridsSize(List<ArmyInBattle> _armiesA, List<ArmyInBattle> _armiesD)
        {
            int remainder = _armiesA.Count - AttackerPlayers.Columns * AttackerPlayers.Columns;
            int addRows = 0;

            // Attackers size
            if (remainder > 0)
            {
                addRows = remainder / AttackerPlayers.Columns;

                if (remainder % AttackerPlayers.Columns != 0)
                {
                    addRows++;
                }
            }
            AttackerPlayers.Rows = AttackerPlayers.Columns + addRows;

            // Defenders size
            remainder = _armiesD.Count - DefenderPlayers.Columns * DefenderPlayers.Columns;
            addRows = 0;

            if (remainder > 0)
            {
                addRows = remainder / DefenderPlayers.Columns;

                if (remainder % DefenderPlayers.Columns != 0)
                {
                    addRows++;
                }
            }
            DefenderPlayers.Rows = DefenderPlayers.Columns + addRows;
        }

        private void populatePlayerGrids(List<ArmyInBattle> _armiesA, List<ArmyInBattle> _armiesD)
        {
            for (int i = 0; i < _armiesA.Count; i++)
            {
                Image playerCoatOfArms = new Image();
                playerCoatOfArms.Source = new BitmapImage(new Uri("/Pictures/profileImage.png", UriKind.Relative));
                playerCoatOfArms.Width = 40;
                playerCoatOfArms.Height = 40;
                playerCoatOfArms.MouseEnter += playerCoatOfArms_MouseEnter;
                playerCoatOfArms.MouseLeave += playerCoatOfArms_MouseLeave;

                ToolTip toolTip = new ToolTip();
                ToolTipService.SetInitialShowDelay(toolTip, 100);
                toolTip.Content = PlayerModel.GetPlayerNameById(_armiesA[i].PlayerId);

                playerCoatOfArms.ToolTip = toolTip;

                AttackerPlayers.Children.Add(playerCoatOfArms);
            }

            for (int i = 0; i < _armiesD.Count; i++)
            {
                Image playerCoatOfArms = new Image();
                playerCoatOfArms.Source = new BitmapImage(new Uri("/Pictures/profileImage.png", UriKind.Relative));
                playerCoatOfArms.Width = 40;
                playerCoatOfArms.Height = 40;

                ToolTip toolTip = new ToolTip();
                ToolTipService.SetInitialShowDelay(toolTip, 100);
                toolTip.Content = PlayerModel.GetPlayerNameById(_armiesA[i].PlayerId);

                playerCoatOfArms.ToolTip = toolTip;

                DefenderPlayers.Children.Add(playerCoatOfArms);
            }
        }

        private void playerCoatOfArms_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void playerCoatOfArms_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void populateTroopsData(List<ArmyInBattle> _armiesA, List<ArmyInBattle> _armiesD)
        {
            ArmyInBattle armyAttacker = new ArmyInBattle();

            for (int i = 0; i < _armiesA.Count; i++)
            {
                armyAttacker.ArmyInfantryCount += _armiesA[i].ArmyInfantryCount;
                armyAttacker.ArmyArchersCount += _armiesA[i].ArmyArchersCount;
                armyAttacker.ArmyHorsemanCount += _armiesA[i].ArmyHorsemanCount;
                armyAttacker.ArmySiegegunCount += _armiesA[i].ArmySiegegunCount;
                armyAttacker.ArmySizeCurrent += _armiesA[i].ArmySizeCurrent;
            }

            ArmyInBattle armyDefender = new ArmyInBattle();

            for (int i = 0; i < _armiesD.Count; i++)
            {
                armyDefender.ArmyInfantryCount += _armiesD[i].ArmyInfantryCount;
                armyDefender.ArmyArchersCount += _armiesD[i].ArmyArchersCount;
                armyDefender.ArmyHorsemanCount += _armiesD[i].ArmyHorsemanCount;
                armyDefender.ArmySiegegunCount += _armiesD[i].ArmySiegegunCount;
                armyDefender.ArmySizeCurrent += _armiesD[i].ArmySizeCurrent;
            }

            InfAtt.Content = armyAttacker.ArmyInfantryCount;
            ArAtt.Content = armyAttacker.ArmyArchersCount;
            KntAtt.Content = armyAttacker.ArmyHorsemanCount;
            SieAtt.Content = armyAttacker.ArmySiegegunCount;

            InfDef.Content = armyAttacker.ArmyInfantryCount;
            ArDef.Content = armyAttacker.ArmyArchersCount;
            KntDef.Content = armyAttacker.ArmyHorsemanCount;
            SieDef.Content = armyAttacker.ArmySiegegunCount;
        }

        private void TroopsTextbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 1 && i <= 9999;
        }
    }
}
