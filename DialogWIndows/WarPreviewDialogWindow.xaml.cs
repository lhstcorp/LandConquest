using LandConquest.Forms;
using LandConquest.Logic;
using LandConquest.WindowDialogViewModels;
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
        int selectedTile = 0;
        public WarPreviewDialogWindow(Player _player, War _war)
        {
            player = _player;
            war = _war;

            InitializeComponent();
            DataContext = new WarPreviewDialogWindowViewModel();
            initWar();
            initWarCaption();
            initPlayerGrids();
            initFreePlayerArmy();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            closeWindow();
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
                toolTip.Content = PlayerModel.GetPlayerNameById(_armiesD[i].PlayerId);

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

            InfDef.Content = armyDefender.ArmyInfantryCount;
            ArDef.Content = armyDefender.ArmyArchersCount;
            KntDef.Content = armyDefender.ArmyHorsemanCount;
            SieDef.Content = armyDefender.ArmySiegegunCount;

            TotalScore.Content = Convert.ToString(armyAttacker.ArmySizeCurrent) + " : " + Convert.ToString(armyDefender.ArmySizeCurrent);
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

        /* OLD WAR
        private void JoinWarBtn_Click(object sender, RoutedEventArgs e)
        {
            setDefaultValuesIntoBlankFields();

            if (validateAvailableTroops())
            {
                ArmyInBattle armyInBattle = new ArmyInBattle();

                armyInBattle.ArmyInfantryCount = Convert.ToInt32(InfInput.Text);
                armyInBattle.ArmyArchersCount = Convert.ToInt32(ArInput.Text);
                armyInBattle.ArmyHorsemanCount = Convert.ToInt32(KntInput.Text);
                armyInBattle.ArmySiegegunCount = Convert.ToInt32(SieInput.Text);
                armyInBattle.calculateSizeCurrent();
                armyInBattle.ArmyType = ArmyModel.ReturnTypeOfArmy(armyInBattle);

                armyInBattle.PlayerId = player.PlayerId;
                armyInBattle.ArmyId = AssistantLogic.GenerateId();

                WarLogic.EnterInWar(war, player, armyInBattle);
                closeWindow();
            }
        } */ 

        private void JoinWarBtn_Click(object sender, RoutedEventArgs e)
        {
            setDefaultValuesIntoBlankFields();

            if (validateAvailableTroops()
             && checkSelectedTile())
            {
                ArmyInBattle armyInBattle = new ArmyInBattle();

                armyInBattle.ArmyInfantryCount = Convert.ToInt32(InfInput.Text);
                armyInBattle.ArmyArchersCount = Convert.ToInt32(ArInput.Text);
                armyInBattle.ArmyHorsemanCount = Convert.ToInt32(KntInput.Text);
                armyInBattle.ArmySiegegunCount = Convert.ToInt32(SieInput.Text);
                armyInBattle.calculateSizeCurrent();

                armyInBattle.PlayerId = player.PlayerId;
                armyInBattle.ArmyId = AssistantLogic.GenerateId();

                WarLogic.EnterInWar(war, player, armyInBattle, selectedTile);

                updateArmiesDataGrid();
            }
        }

        private void setDefaultValuesIntoBlankFields()
        {
            if (InfInput.Text == "")
            {
                InfInput.Text = "0";
            }

            if (ArInput.Text == "")
            {
                ArInput.Text = "0";
            }

            if (KntInput.Text == "")
            {
                KntInput.Text = "0";
            }

            if (SieInput.Text == "")
            {
                SieInput.Text = "0";
            }
        }

        private void initFreePlayerArmy()
        {
            Army army = new Army();
            army = ArmyModel.GetArmyInfo(player, army);

            ArmyInBattle armyInBattle = WarLogic.CheckFreeArmies(army, player);

            FreeAr.Content = armyInBattle.ArmyArchersCount;
            FreeInf.Content = armyInBattle.ArmyInfantryCount;
            FreeKnt.Content = armyInBattle.ArmyHorsemanCount;
            FreeSie.Content = armyInBattle.ArmySiegegunCount;

            InfInput.Text = armyInBattle.ArmyInfantryCount.ToString();
            ArInput.Text = armyInBattle.ArmyArchersCount.ToString();
            KntInput.Text = armyInBattle.ArmyHorsemanCount.ToString();
            SieInput.Text = armyInBattle.ArmySiegegunCount.ToString();
        }

        private void closeWindow()
        {
            ((WarPreviewDialogWindowViewModel)DataContext).CloseWindow();
        }

        private bool validateAvailableTroops()
        {
            bool ret = true;

            if (Convert.ToInt32(InfInput.Text) > Convert.ToInt32(FreeInf.Content)
             || Convert.ToInt32(ArInput.Text) > Convert.ToInt32(FreeAr.Content)
             || Convert.ToInt32(KntInput.Text) > Convert.ToInt32(FreeKnt.Content)
             || Convert.ToInt32(SieInput.Text) > Convert.ToInt32(FreeSie.Content))
            {
                ret = false;
                WarningDialogWindow.CallWarningDialogNoResult("You can't send more troops than available.");
            }

            return ret;
        }

        private bool checkSelectedTile()
        {
            bool ret = true;

            if (selectedTile == 0)
            {
                ret = false;
                WarningDialogWindow.CallWarningDialogNoResult("Please, select a tile where you want to send troops.");
            }

            return ret;
        }

        private void TileMouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = (Rectangle)sender;
            selectedTile = Convert.ToInt32(rectangle.Tag);
        }

        private void updateArmiesDataGrid()
        {
            List<ArmyInBattle> armiesInBattle = new List<ArmyInBattle>();
            armiesInBattle = BattleModel.GetArmiesInfoInCurrentTile(war, selectedTile);

            armiesDataGrid.ItemsSource = armiesInBattle;
        }

    }
}
