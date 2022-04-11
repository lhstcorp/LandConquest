using LandConquest.DialogWIndows;
using LandConquest.Forms;
using LandConquest.Logic;
using LandConquest.WindowDialogViewModels;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.Forms
{
    public partial class WarWindow : Window
    {
        // GLOBALS -->
        Player player;
        War war;

        int selectedTile = 0;
        Rectangle selectedArea;
        // <--

        // CONST -->
        const int AREA_COUNT = 18;
        // <--

        public WarWindow(Player _player, War _war)
        {
            player = _player;
            war = _war;

            InitializeComponent();
            DataContext = new WarWindowViewModel();
            initWar();
            initWarCaption();
            initPlayerGrids();
            initFreePlayerArmy();
            initWarAreas();
            updateWarWindowInfoAsync();
        }

        private async void updateWarWindowInfoAsync()
        {
            await Task.Run(() => updateInfoAsync());
        }

        private async void updateInfoAsync()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(10000);

                    Dispatcher.Invoke(new ThreadStart(delegate
                    {
                        initPlayerGrids();
                        initWarAreas();
                        loadLossesDataGrid();
                    }));
                }
                catch { }
            }
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
            AttackerPlayers.Children.Clear();
            DefenderPlayers.Children.Clear();

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
                initFreePlayerArmy();
                initPlayerGrids();
                initWarAreas();
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
            ((WarWindowViewModel)DataContext).CloseWindow();
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
            cancelFrameHighlight();
            selectedArea = this.FindName("Area" + ((Label)sender).Tag) as Rectangle;
            selectedTile = Convert.ToInt32(selectedArea.Tag);
            highlightSelectedFrame();
            updateArmiesDataGrid();
            loadLossesDataGrid();
        }

        private void highlightSelectedFrame()
        {
            selectedArea.Stroke = new SolidColorBrush((Color.FromRgb(199, 176, 8)));
            selectedArea.StrokeThickness = 3;
        }

        private void cancelFrameHighlight()
        {
            if (selectedArea != null)
            {
                selectedArea.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                selectedArea.StrokeThickness = 1;
            }
        }

        private void updateArmiesDataGrid()
        {
            List<ArmyInBattle> armiesInBattle = new List<ArmyInBattle>();
            armiesInBattle = BattleModel.GetArmiesInfoInCurrentTile(war, selectedTile);

            armiesDataGrid.ItemsSource = armiesInBattle;
        }

        private void TileMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void TileMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void removeArmyBtn_Click(object sender, RoutedEventArgs e)
        {
            ArmyInBattle armyInBattle = (ArmyInBattle)armiesDataGrid.SelectedItem;

            if (armyInBattle != null)
            {
                if (armyInBattle.PlayerId == player.PlayerId)
                {
                    BattleModel.DeleteArmyById(armyInBattle);

                    updateArmiesDataGrid();
                    initFreePlayerArmy();
                    initPlayerGrids();
                    initWarAreas();
                }
            }
        }

        private void initWarAreas()
        {
            for (int i = 0; i < AREA_COUNT; i++)
            {
                List<ArmyInBattle> armiesInBattle = new List<ArmyInBattle>();
                armiesInBattle = BattleModel.GetArmiesInfoInCurrentTile(war, i + 1);

                ArmyInBattle defenderArmy = WarModel.calculateArmy(armiesInBattle, 0);
                ArmyInBattle attackerArmy = WarModel.calculateArmy(armiesInBattle, 1);

                Label areaLabel = this.FindName("Area" + (i + 1) + "Label") as Label;
                Rectangle areaRect = this.FindName("Area" + areaLabel.Tag) as Rectangle;

                if (attackerArmy.ArmySizeCurrent == defenderArmy.ArmySizeCurrent)
                {
                    areaLabel.Content = "0.5";
                    areaRect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                }
                else if (attackerArmy.ArmySizeCurrent == 0)
                {
                    areaLabel.Content = "1";
                    areaRect.Fill = new SolidColorBrush(Color.FromRgb(62, 93, 179));
                }
                else if (defenderArmy.ArmySizeCurrent == 0)
                {
                    areaLabel.Content = "1";
                    areaRect.Fill = new SolidColorBrush(Color.FromRgb(179, 62, 62));
                }
                else
                {
                    if (defenderArmy.ArmySizeCurrent > attackerArmy.ArmySizeCurrent)
                    {
                        areaLabel.Content = Math.Round((decimal)defenderArmy.ArmySizeCurrent / (decimal)(defenderArmy.ArmySizeCurrent + attackerArmy.ArmySizeCurrent), 2);
                        areaRect.Fill = new SolidColorBrush(Color.FromRgb(62, 93, 179));
                    }
                    else
                    {
                        areaLabel.Content = Math.Round((decimal)attackerArmy.ArmySizeCurrent / (decimal)(defenderArmy.ArmySizeCurrent + attackerArmy.ArmySizeCurrent), 2);
                        areaRect.Fill = new SolidColorBrush(Color.FromRgb(179, 62, 62));
                    }

                }
            }
        }

        private void infImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Convert.ToInt32(InfInput.Text) > 0)
            {
                InfInput.Text = "0";
            }
            else
            {
                InfInput.Text = Convert.ToString(FreeInf.Content);
            }
        }

        private void arImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Convert.ToInt32(ArInput.Text) > 0)
            {
                ArInput.Text = "0";
            }
            else
            {
                ArInput.Text = Convert.ToString(FreeAr.Content);
            }
        }

        private void kntImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Convert.ToInt32(KntInput.Text) > 0)
            {
                KntInput.Text = "0";
            }
            else
            {
                KntInput.Text = Convert.ToString(FreeKnt.Content);
            }
        }

        private void sieImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Convert.ToInt32(SieInput.Text) > 0)
            {
                SieInput.Text = "0";
            }
            else
            {
                SieInput.Text = Convert.ToString(FreeSie.Content);
            }
        }

        private void armyImg_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void armyImg_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        public void loadLossesDataGrid()
        {
            List<LogWar> warLogs = new List<LogWar>();
            warLogs = LogWarModel.GetLogWarListForArea(war.WarId, selectedTile);

            logWarDataGrid.ItemsSource = warLogs;
        }
    }
}
