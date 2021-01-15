using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Forces;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LandConquest.Forms
{
    public partial class WarWindow : Window
    {
        System.Windows.Controls.Primitives.UniformGrid localWarMap = new System.Windows.Controls.Primitives.UniformGrid();
        Image imgArmySelected;
        Boolean f_armySelected = false;
        Boolean f_canMoveArmy = false;
        int INDEX;
        const int syncTick = 30; //sec
        private int timerValue = 30;

        Player player;
        List<ArmyInBattle> armies;
        List<Image> armyImages;
        ArmyInBattle army;
        ArmyInBattle selectedArmy;
        List<ArmyInBattle> armyInBattlesInCurrentTile;
        War war;
        int saveMovementSpeed;
        Image emptyImage = new Image();
        int armyPage;
        int index;
        List<bool> selectedArmiesForUnion;
        List<ArmyInBattle> yourArmiesInCurrentTile = new List<ArmyInBattle>();
        int SelectionCounter;

        DispatcherTimer syncTimer;
        List<Image> playerArmiesImages = new List<Image>();
        List<ArmyInBattle> playerArmies = new List<ArmyInBattle>();

        //Canvas localWarArmyLayer = new Canvas();
        public WarWindow(Player _player, ArmyInBattle _army, List<ArmyInBattle> _armies, War _war)
        {
            player = _player;
            army = _army;
            armies = _armies;
            war = _war;

            InitializeComponent();
            int columnWidth = 40;
            int rowHeight = 40;
            localWarMap.Columns = 30; //50
            localWarMap.Rows = 20;    //40
            gridForArmies.Columns = 30;
            gridForArmies.Rows = 20;

            localWarMap.Width = columnWidth * localWarMap.Columns;
            gridForArmies.Width = columnWidth * localWarMap.Columns;
            localWarMap.Height = rowHeight * localWarMap.Rows;
            gridForArmies.Height = rowHeight * localWarMap.Rows;

            localWarMap.HorizontalAlignment = HorizontalAlignment.Left;
            localWarMap.Margin = new Thickness(50, 0, 0, 0);
            localWarMap.VerticalAlignment = VerticalAlignment.Center;
            gridForArmies.HorizontalAlignment = HorizontalAlignment.Left;
            gridForArmies.Margin = new Thickness(50, 0, 0, 0);
            gridForArmies.VerticalAlignment = VerticalAlignment.Center;

            timerLabel.Content = "syncing...";
            Loaded += WarWin_Loaded;

        }

        private void WarWin_Loaded(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < localWarMap.Rows; x++)
            {
                for (int y = 0; y < localWarMap.Columns; y++)
                {
                    Image tile = new Image();
                    tile = AddSourceForTile(tile, 0, x, y);
                    localWarMap.Children.Add(tile);
                    gridForArmies.Children.Add(new Image());
                }
            }
            mainWarWinGrid.Children.Add(localWarMap);

            ShowArmiesOnMap();
            SyncWithServer();
        }

        public void ShowArmiesOnMap()
        {
            armies.Clear();
            armies = BattleModel.GetArmiesInfo(armies, war);

            armyImages = new List<Image>();
            for (int i = 0; i < armies.Count(); i++)
            {
                Image imgArmy = new Image();
                imgArmy.MouseLeftButtonDown += ImgArmy_MouseLeftButtonDown;
                imgArmy.MouseEnter += ImgArmy_MouseEnter;
                imgArmy.MouseLeave += ImgArmy_MouseLeave;
                imgArmy.MouseRightButtonDown += ImgArmy_MouseRightButtonDown;
                imgArmy.Width = 40;
                imgArmy.Height = 40;



                if (((Image)gridForArmies.Children[armies[i].LocalLandId]).Source == emptyImage.Source)
                {
                    if (armies[i].ArmySide == 0)
                    {
                        SwitchArmyTypeNoSide(armies[i].ArmyType, imgArmy);
                    }
                    else
                    {
                        SwitchArmyTypeWithSide(armies[i].ArmyType, imgArmy);
                    }

                    armyImages.Add(imgArmy);

                    gridForArmies.Children.RemoveAt(armies[i].LocalLandId);
                }
                else
                {
                    if (((Image)gridForArmies.Children[armies[i].LocalLandId]).Source != imgArmy.Source)
                    {
                        gridForArmies.Children.RemoveAt(armies[i].LocalLandId);
                        List<ArmyInBattle> armyInOneTileTest = new List<ArmyInBattle>();
                        for (int j = 0; j < armies.Count(); j++)
                        {
                            if (armies[j].LocalLandId == armies[i].LocalLandId)
                            {
                                armyInOneTileTest.Add(armies[j]);
                            }
                        }

                        if (BattleModel.IfTheBattleShouldStart(armyInOneTileTest))
                        {
                            Image imgBattle = new Image();
                            imgBattle.Source = new BitmapImage(new Uri("/Pictures/war-test.png", UriKind.Relative));
                            //imgArmy.MouseDown += ImgWar_MouseButtonDown;
                            imgBattle.MouseLeftButtonDown += ImgWar_MouseLeftButtonDown;
                            imgBattle.MouseRightButtonDown += ImgArmy_MouseRightButtonDown;
                            imgBattle.Cursor = Cursors.Hand;
                            imgArmy = imgBattle;
                        }
                        else
                        {
                            if (armies[i].ArmySide == 0)
                            {
                                SwitchArmyTypeNoSide(BattleModel.ReturnTypeOfArmy(armyInOneTileTest), imgArmy);
                            }
                            else
                            {
                                SwitchArmyTypeWithSide(BattleModel.ReturnTypeOfArmy(armyInOneTileTest), imgArmy);
                            }
                        }

                        //imgArmy.Source = new BitmapImage(new Uri("/Pictures/peasants_total.png", UriKind.Relative));
                    }
                }
                gridForArmies.Children.Insert(armies[i].LocalLandId, imgArmy);
            }
        }

        public int ReturnNumberOfCell(int row, int column)
        {
            int index = (row - 1) * localWarMap.Columns + column - 1;
            return index;
        }

        private void ImgArmy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            f_canMoveArmy = false;
            armyPage = 0;
            HideAvailableTilesToMove(INDEX);
            imgArmySelected = (Image)sender;
            index = gridForArmies.Children.IndexOf(imgArmySelected);
            localWarMap.Children.RemoveAt(index);
            localWarMap.Children.Insert(index, new Image { Source = new BitmapImage(new Uri("/Pictures/tile-test-red.jpg", UriKind.Relative)) });

            f_armySelected = true;
            INDEX = index;

            ShowInfoAboutArmies(index);

            if (f_canMoveArmy)
                ShowAvailableTilesToMove(index);
        }

        private void tile_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (f_armySelected)
            {
                f_armySelected = false;

                armyInBattlesInCurrentTile = new List<ArmyInBattle>();

                for (int i = 0; i < BattleModel.SelectLastIdOfArmiesInCurrentTile(INDEX, war); i++)
                {
                    armyInBattlesInCurrentTile.Add(new ArmyInBattle());
                }

                armyInBattlesInCurrentTile = BattleModel.GetArmiesInfoInCurrentTile(armyInBattlesInCurrentTile, war, INDEX);

                Image imgArmyThatStay = new Image();
                imgArmyThatStay.MouseLeftButtonDown += ImgArmy_MouseLeftButtonDown;
                imgArmyThatStay.MouseEnter += ImgArmy_MouseEnter;
                imgArmyThatStay.MouseLeave += ImgArmy_MouseLeave;
                imgArmyThatStay.MouseRightButtonDown += ImgArmy_MouseRightButtonDown;
                imgArmyThatStay.Width = 40;
                imgArmyThatStay.Height = 40;

                if (armyInBattlesInCurrentTile.Count > 1)
                {
                    for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++)
                    {
                        if (armyInBattlesInCurrentTile[i].PlayerId == player.PlayerId)
                        {
                            armyInBattlesInCurrentTile.Remove(armyInBattlesInCurrentTile[i]);

                            if (selectedArmy.ArmySide == 0)
                            {
                                SwitchArmyTypeNoSide(selectedArmy.ArmyType, imgArmySelected);
                            }
                            else
                            {
                                SwitchArmyTypeWithSide(selectedArmy.ArmyType, imgArmySelected);
                            }
                        }
                    }

                    int typeOfUniteArmy = BattleModel.ReturnTypeOfArmy(armyInBattlesInCurrentTile);

                    if (armyInBattlesInCurrentTile[0].ArmySide == 0)
                    {
                        SwitchArmyTypeNoSide(typeOfUniteArmy, imgArmyThatStay);
                    }
                    else
                    {
                        SwitchArmyTypeWithSide(typeOfUniteArmy, imgArmyThatStay);
                    }


                    int index = localWarMap.Children.IndexOf((Image)sender);
                    gridForArmies.Children.RemoveAt(INDEX);
                    gridForArmies.Children.Insert(INDEX, imgArmyThatStay);
                    gridForArmies.Children.RemoveAt(index);
                    gridForArmies.Children.Insert(index, imgArmySelected);

                    HideAvailableTilesToMove(INDEX);
                    BattleModel.UpdateLocalLandOfArmy(selectedArmy, index);
                }
                else
                {
                    int index = localWarMap.Children.IndexOf((Image)sender);
                    gridForArmies.Children.RemoveAt(INDEX);
                    gridForArmies.Children.Insert(INDEX, new Image());
                    gridForArmies.Children.RemoveAt(index);
                    gridForArmies.Children.Insert(index, imgArmySelected);

                    HideAvailableTilesToMove(INDEX);
                    BattleModel.UpdateLocalLandOfArmy(selectedArmy, index);
                }

                imgArmySelected.IsEnabled = false;
            }
        }

        private void ImgArmy_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (f_armySelected)
            {
                int index = gridForArmies.Children.IndexOf((Image)sender);
                if (CheckDistanceBetweenTwoArmies(index, INDEX))
                {
                    f_armySelected = false;
                    gridForArmies.Children.RemoveAt(INDEX);
                    gridForArmies.Children.Insert(INDEX, new Image());

                    gridForArmies.Children.RemoveAt(index);
                    // call method retype;

                    armyInBattlesInCurrentTile = new List<ArmyInBattle>();

                    for (int i = 0; i < BattleModel.SelectLastIdOfArmiesInCurrentTile(index, war); i++)
                    {
                        armyInBattlesInCurrentTile.Add(new ArmyInBattle());
                    }

                    armyInBattlesInCurrentTile = BattleModel.GetArmiesInfoInCurrentTile(armyInBattlesInCurrentTile, war, index);
                    armyInBattlesInCurrentTile.Add(selectedArmy);

                    if (BattleModel.IfTheBattleShouldStart(armyInBattlesInCurrentTile))
                    {
                        imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/war-test.png", UriKind.Relative));
                        imgArmySelected.MouseLeftButtonDown += ImgWar_MouseLeftButtonDown;
                        imgArmySelected.MouseRightButtonDown += ImgArmy_MouseRightButtonDown;
                        Battle battle = new Battle();
                        battle.BattleId = generateId();
                        battle.WarId = war.WarId;
                        battle.LocalLandId = armyInBattlesInCurrentTile[0].LocalLandId;

                        if (!theWarStarted(index))
                        {

                            BattleModel.InsertBattle(battle);
                        }
                        else
                        {
                            //imgArmySelected = null;
                        }
                    }
                    else
                    {
                        int typeOfUniteArmy = BattleModel.ReturnTypeOfArmy(armyInBattlesInCurrentTile);

                        if (selectedArmy.ArmySide == 0)
                        {
                            SwitchArmyTypeNoSide(typeOfUniteArmy, imgArmySelected);
                        }
                        else
                        {
                            SwitchArmyTypeWithSide(typeOfUniteArmy, imgArmySelected);
                        }
                    }
                    //armyImages.Add(imgArmySelected);

                    gridForArmies.Children.Insert(index, imgArmySelected);
                    HideAvailableTilesToMove(INDEX);

                    BattleModel.UpdateLocalLandOfArmy(selectedArmy, index);


                    //перезаписываем в этот лист армии что остались
                    armyInBattlesInCurrentTile.Clear();

                    for (int i = 0; i < BattleModel.SelectLastIdOfArmiesInCurrentTile(INDEX, war); i++)
                    {
                        armyInBattlesInCurrentTile.Add(new ArmyInBattle());
                    }

                    armyInBattlesInCurrentTile = BattleModel.GetArmiesInfoInCurrentTile(armyInBattlesInCurrentTile, war, INDEX);

                    if (armyInBattlesInCurrentTile.Count >= 1)
                    {
                        Image imgArmyThatStay = new Image();
                        imgArmyThatStay.MouseLeftButtonDown += ImgArmy_MouseLeftButtonDown;
                        imgArmyThatStay.MouseEnter += ImgArmy_MouseEnter;
                        imgArmyThatStay.MouseLeave += ImgArmy_MouseLeave;
                        imgArmyThatStay.MouseRightButtonDown += ImgArmy_MouseRightButtonDown;
                        imgArmyThatStay.Width = 40;
                        imgArmyThatStay.Height = 40;

                        int typeOfUniteArmy2 = BattleModel.ReturnTypeOfArmy(armyInBattlesInCurrentTile);

                        if (armyInBattlesInCurrentTile[0].ArmySide == 0)
                        {
                            SwitchArmyTypeNoSide(typeOfUniteArmy2, imgArmyThatStay);
                        }
                        else
                        {
                            SwitchArmyTypeWithSide(typeOfUniteArmy2, imgArmyThatStay);
                        }


                        gridForArmies.Children.RemoveAt(INDEX);
                        gridForArmies.Children.Insert(INDEX, imgArmyThatStay);
                    }

                    imgArmySelected.IsEnabled = false;
                }
            }
        }

        private void ImgArmy_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void ImgArmy_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        public void ShowAvailableTilesToMove(int index) //+ int forces movement speed;
        {
            // army type
            //gridForArmies.ro
            Console.WriteLine("INDEX = " + index);

            int row = index / localWarMap.Columns + 1;
            int col = index - localWarMap.Columns * (row - 1) + 1;

            Console.WriteLine("ROW = " + row + " COL = " + col);

            int movement = 0;
            switch (selectedArmy.ArmyType)
            {
                case 1:
                    {
                        movement = (int)ForcesEnum.Infantry.Movement;
                        break;
                    }
                case 2:
                    {
                        movement = (int)ForcesEnum.Archers.Movement;
                        break;
                    }
                case 3:
                    {
                        movement = (int)ForcesEnum.Knights.Movement;
                        break;
                    }
                case 4:
                    {
                        movement = (int)ForcesEnum.Siege.Movement;
                        break;
                    }
                case 5:
                    {
                        movement = (int)ForcesEnum.Infantry.Movement;
                        break;
                    }

            }

            saveMovementSpeed = movement;
            //????????????????????????????????????????????????????????
            for (int i = row - movement; i <= row + movement; i++)
            {
                for (int j = col - movement; j <= col + movement; j++)
                {
                    if ((i < 1) || (j < 1) || (j > localWarMap.Columns) || (i > localWarMap.Rows)) continue;  //
                    int ind = ReturnNumberOfCell(i, j);
                    localWarMap.Children.RemoveAt(ind);
                    //Image availableTileForMoving = new Image { Source = new BitmapImage(new Uri("/Pictures/tile-test-red.jpg", UriKind.Relative)) };
                    Image availableTileForMoving = new Image();
                    availableTileForMoving = AddSourceForTile(availableTileForMoving, 1, i, j);
                    availableTileForMoving.MouseRightButtonDown += tile_MouseRightButtonDown;
                    localWarMap.Children.Insert(ind, availableTileForMoving);
                }
            }
        }

        public void HideAvailableTilesToMove(int index) //+ int forces movement speed;
        {
            // АНАЛОГ ФУНКЦИИ ВЫШЕ, (не супер оптимизирован, но лучше чем перекрашивать вообще все клетки)
            Console.WriteLine("INDEX = " + index);

            int row = index / localWarMap.Columns + 1;
            int col = index - localWarMap.Columns * (row - 1) + 1;

            Console.WriteLine("ROW = " + row + " COL = " + col);

            //????????????????????????????????????????????????????????
            for (int i = row - saveMovementSpeed; i <= row + saveMovementSpeed; i++)
            {
                for (int j = col - saveMovementSpeed; j <= col + saveMovementSpeed; j++)
                {
                    if ((i < 1) || (j < 1) || (j > localWarMap.Columns) || (i > localWarMap.Rows)) continue;  //
                    int ind = ReturnNumberOfCell(i, j);
                    localWarMap.Children.RemoveAt(ind);
                    //Image availableTileForMoving = new Image { Source = new BitmapImage(new Uri("/Pictures/test-tile.jpg", UriKind.Relative)) };
                    Image availableTileForMoving = new Image();
                    availableTileForMoving = AddSourceForTile(availableTileForMoving, 0, i, j);
                    //availableTileForMoving.MouseRightButtonDown += tile_MouseRightButtonDown;
                    localWarMap.Children.Insert(ind, availableTileForMoving);
                }
            }
        }

        public void ShowInfoAboutArmies(int index)
        {
            SelectionCounter = 0;
            selectedArmiesForUnion = new List<bool>();
            armyInBattlesInCurrentTile = new List<ArmyInBattle>();

            for (int i = 0; i < BattleModel.SelectLastIdOfArmiesInCurrentTile(index, war); i++)
            {
                armyInBattlesInCurrentTile.Add(new ArmyInBattle());
                selectedArmiesForUnion.Add(false);
            }

            armyInBattlesInCurrentTile = BattleModel.GetArmiesInfoInCurrentTile(armyInBattlesInCurrentTile, war, index);

            ShowInfoAboutArmy();

            for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++)
            {
                if (armyInBattlesInCurrentTile[i].PlayerId == player.PlayerId)
                {
                    yourArmiesInCurrentTile.Add(armyInBattlesInCurrentTile[i]);
                }
            }
        }

        private void splitArmiesButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SplitArmyDialog dialogWindow = new SplitArmyDialog(armyInBattlesInCurrentTile[0], war);
            dialogWindow.Show();
        }

        private void ImgWar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ArmyInBattle leftSide = new ArmyInBattle();
            ArmyInBattle rightSide = new ArmyInBattle();
            int index = gridForArmies.Children.IndexOf((Image)sender);

            List<ArmyInBattle> armyInBattlesInCurrentTile = new List<ArmyInBattle>();

            for (int i = 0; i < BattleModel.SelectLastIdOfArmiesInCurrentTile(index, war); i++)
            {
                armyInBattlesInCurrentTile.Add(new ArmyInBattle());
            }

            armyInBattlesInCurrentTile = BattleModel.GetArmiesInfoInCurrentTile(armyInBattlesInCurrentTile, war, index);

            for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++)
            {
                Console.WriteLine(armyInBattlesInCurrentTile[i].ArmyId + " = " + armyInBattlesInCurrentTile[i].ArmySide);
                if (armyInBattlesInCurrentTile[i].ArmySide == 0)
                {
                    leftSide.ArmySizeCurrent += armyInBattlesInCurrentTile[i].ArmySizeCurrent;
                    leftSide.ArmyInfantryCount += armyInBattlesInCurrentTile[i].ArmyInfantryCount;
                    leftSide.ArmyArchersCount += armyInBattlesInCurrentTile[i].ArmyArchersCount;
                    leftSide.ArmyHorsemanCount += armyInBattlesInCurrentTile[i].ArmyHorsemanCount;
                    leftSide.ArmySiegegunCount += armyInBattlesInCurrentTile[i].ArmySiegegunCount;
                }
                else
                {
                    rightSide.ArmySizeCurrent += armyInBattlesInCurrentTile[i].ArmySizeCurrent;
                    rightSide.ArmyInfantryCount += armyInBattlesInCurrentTile[i].ArmyInfantryCount;
                    rightSide.ArmyArchersCount += armyInBattlesInCurrentTile[i].ArmyArchersCount;
                    rightSide.ArmyHorsemanCount += armyInBattlesInCurrentTile[i].ArmyHorsemanCount;
                    rightSide.ArmySiegegunCount += armyInBattlesInCurrentTile[i].ArmySiegegunCount;
                }
            }

            warriorsAllLeft.Content = leftSide.ArmySizeCurrent;
            warriorsInfantryLeft.Content = leftSide.ArmyInfantryCount;
            warriorsArchersLeft.Content = leftSide.ArmyArchersCount;
            warriorsKnightsLeft.Content = leftSide.ArmyHorsemanCount;
            warriorsSiegeLeft.Content = leftSide.ArmySiegegunCount;

            warriorsAllRight.Content = rightSide.ArmySizeCurrent;
            warriorsInfantryRight.Content = rightSide.ArmyInfantryCount;
            warriorsArchersRight.Content = rightSide.ArmyArchersCount;
            warriorsKnightsRight.Content = rightSide.ArmyHorsemanCount;
            warriorsSiegeRight.Content = rightSide.ArmySiegegunCount;

            warGrid.Visibility = Visibility.Visible;
        }

        private void armyPageArrowLeft_Click(object sender, RoutedEventArgs e)
        {

            HideAvailableTilesToMove(index);
            if (armyPage == 0) armyPage = armyInBattlesInCurrentTile.Count - 1; else armyPage--;
            ShowInfoAboutArmy();
        }

        private void armyPageArrowRight_Click(object sender, RoutedEventArgs e)
        {
            HideAvailableTilesToMove(index);
            if ((armyPage % (armyInBattlesInCurrentTile.Count - 1) == 0) && (armyPage != 0)) armyPage = 0; else armyPage++;
            ShowInfoAboutArmy();
        }

        public void ShowInfoAboutArmy()
        {
            playerNameLbl.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            playerNameLbl.Content = armyInBattlesInCurrentTile[armyPage].PlayerId;
            warriorsInfantry.Content = armyInBattlesInCurrentTile[armyPage].ArmyInfantryCount;
            warriorsArchers.Content = armyInBattlesInCurrentTile[armyPage].ArmyArchersCount;
            warriorsKnights.Content = armyInBattlesInCurrentTile[armyPage].ArmyHorsemanCount;
            warriorsSiege.Content = armyInBattlesInCurrentTile[armyPage].ArmySiegegunCount;
            warriorsAll.Content = armyInBattlesInCurrentTile[armyPage].ArmySizeCurrent;

            if (player.PlayerId == armyInBattlesInCurrentTile[armyPage].PlayerId)
            {
                selectedArmy = armyInBattlesInCurrentTile[armyPage];
                f_canMoveArmy = true;

                btnSelectToUnite.Visibility = Visibility.Visible;
                btnUnite.Visibility = Visibility.Visible;
                splitArmiesButton.Visibility = Visibility.Visible;

                ShowAvailableTilesToMove(index);

                if (selectedArmiesForUnion[armyPage])
                {
                    playerNameLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
            }
            else
            {
                localWarMap.Children.RemoveAt(index);
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("/Pictures/tile-test-red.jpg", UriKind.Relative));
                localWarMap.Children.Insert(index, img);

                btnSelectToUnite.Visibility = Visibility.Hidden;
                btnUnite.Visibility = Visibility.Hidden;
                splitArmiesButton.Visibility = Visibility.Hidden;
            }
        }

        private void btnSelectToUnite_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++)
            {
                if (armyInBattlesInCurrentTile[i].ArmyId == selectedArmy.ArmyId)
                {
                    selectedArmiesForUnion[i] = !selectedArmiesForUnion[i];
                    if (selectedArmiesForUnion[i])
                    {
                        playerNameLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        SelectionCounter++;
                    }
                    else
                    {
                        playerNameLbl.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        SelectionCounter--;
                    }
                    break;
                }
            }
        }

        private void btnUnite_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionCounter > 1)
            {
                bool f = true;
                ArmyInBattle unionArmy = new ArmyInBattle();
                for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++)
                {
                    if (selectedArmiesForUnion[i])
                    {
                        unionArmy.ArmySizeCurrent += armyInBattlesInCurrentTile[i].ArmySizeCurrent;
                        unionArmy.ArmyArchersCount += armyInBattlesInCurrentTile[i].ArmyArchersCount;
                        unionArmy.ArmyInfantryCount += armyInBattlesInCurrentTile[i].ArmyInfantryCount;
                        unionArmy.ArmyHorsemanCount += armyInBattlesInCurrentTile[i].ArmyHorsemanCount;
                        unionArmy.ArmySiegegunCount += armyInBattlesInCurrentTile[i].ArmySiegegunCount;

                        if (f)
                        {
                            f = !f;
                            unionArmy.ArmyId = armyInBattlesInCurrentTile[i].ArmyId;
                            unionArmy.PlayerId = player.PlayerId;
                            continue;
                        }
                        else
                        {
                            BattleModel.DeleteArmyById(armyInBattlesInCurrentTile[i]);
                        }
                    }
                }

                unionArmy.ArmyType = BattleModel.ReturnTypeOfUnionArmy(unionArmy);
                BattleModel.UpdateArmyInBattle(unionArmy);
                HideAvailableTilesToMove(index);
                armyPage = 0;
                ShowInfoAboutArmies(index);
            }
        }

        public Image AddSourceForTile(Image tile, int tileColor, int Row, int Column)
        {
            //tileColor = 0 ? green
            //tileColor = 1 ? red

            if (tileColor == 0)
            {
                if ((Column % 2 == 0) && (Row % 2 == 0) || (Column % 2 == 1) && (Row % 2 == 1))
                {
                    tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/bj1.jpg", UriKind.Relative));
                }
                else
                {
                    tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/br2.jpg", UriKind.Relative));
                }
            }
            else
            {
                if (((Column % 2 == 0) && (Row % 2 == 0)) || ((Column % 2 == 1) && (Row % 2 == 1)))
                {
                    tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/bj2.jpg", UriKind.Relative));
                }
                else tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/br1.jpg", UriKind.Relative));
            }
            return tile;
        }



        private void SwitchArmyTypeNoSide(int armyType, Image armyImage)
        {
            switch (armyType)
            {
                case 1:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/INF-0.png", UriKind.Relative));
                        break;
                    }
                case 2:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/AR-0.png", UriKind.Relative));
                        break;
                    }
                case 3:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/KNT-0.png", UriKind.Relative));
                        break;
                    }
                case 4:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/SIE-0.png", UriKind.Relative));
                        break;
                    }
                case 5:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/TRO-0.png", UriKind.Relative));
                        break;
                    }
            }
        }

        private void SwitchArmyTypeWithSide(int armyType, Image armyImage)
        {
            switch (armyType)
            {
                case 1:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/INF-1.png", UriKind.Relative));
                        break;
                    }
                case 2:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/AR-1.png", UriKind.Relative));
                        break;
                    }
                case 3:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/KNT-1.png", UriKind.Relative));
                        break;
                    }
                case 4:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/SIE-1.png", UriKind.Relative));
                        break;
                    }
                case 5:
                    {
                        armyImage.Source = new BitmapImage(new Uri("/Pictures/Armies/TRO-1.png", UriKind.Relative));
                        break;
                    }
            }
        }
        private void btnWarWindowClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool CheckDistanceBetweenTwoArmies(int index, int INDEX)
        {
            int rowI1 = ReturnRowByIndex(index);
            int rowI2 = ReturnRowByIndex(INDEX);
            int colI1 = ReturnColumnByIndex(index);
            int colI2 = ReturnColumnByIndex(INDEX);

            if ((Math.Abs(rowI1 - rowI2) <= saveMovementSpeed) && ((Math.Abs(colI1 - colI2) <= saveMovementSpeed)))
                return true;
            return false;

        }


        public int ReturnRowByIndex(int index)
        {
            int row = index / localWarMap.Columns + 1;

            return row;
        }

        public int ReturnColumnByIndex(int index)
        {
            int row = index / localWarMap.Columns + 1;
            int col = index - localWarMap.Columns * (row - 1) + 1;

            return col;
        }

        public string generateId()
        {
            Thread.Sleep(15);
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public bool theWarStarted(int index)
        {
            bool theWarStarted = BattleModel.DidTheWarStarted(index, war);
            Console.WriteLine(theWarStarted);
            return theWarStarted;
        }


        public void SyncWithServer()
        {
            syncTimer = new DispatcherTimer();
            if (DateTime.UtcNow.Second >= syncTick)
            {
                syncTimer.Interval = TimeSpan.FromSeconds(60 - DateTime.UtcNow.Second);
            }
            else
            {
                syncTimer.Interval = TimeSpan.FromSeconds(syncTick - DateTime.UtcNow.Second);
            }
            syncTimer.Tick += LetsTick;
            syncTimer.Start();

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += LocalTimer_Tick;
            timerLabel.Visibility = Visibility.Visible;
            dt.Start();

            searchPlayerArmies();
            lockAllPlayerArmies();
            calculateMoveCounter();
        }

        public void LetsTick(object sender, EventArgs e)
        {
            syncTimer.Stop();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(syncTick);
            timer.Tick += timer_Tick;
            timer.Start();

            firstTick();
        }


        private void LocalTimer_Tick(object sender, EventArgs e)
        {
            timerValue--;
            timerLabel.Content = timerValue.ToString();
        }

        public void firstTick()
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            searchPlayerArmies();
            ShowArmiesOnMap();
            unlockAllPlayerArmies();

        }

        public void timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            searchPlayerArmies();
            ShowArmiesOnMap();
            unlockAllPlayerArmies();
            timerValue = 30;
        }


        public void searchPlayerArmies()
        {
            playerArmies.Clear();
            playerArmiesImages.Clear();
            playerArmies = BattleModel.GetPlayerArmiesInfo(playerArmies, war, player);
            addPlayerArmiesImagesToList();

        }

        public void addPlayerArmiesImagesToList()
        {
            for (int i = 0; i < playerArmies.Count; i++)
            {
                playerArmiesImages.Add((Image)gridForArmies.Children[playerArmies[i].LocalLandId]);
            }
        }

        public void lockAllPlayerArmies()
        {
            foreach (Image image in playerArmiesImages)
            {
                image.IsEnabled = false;
            }
        }

        public void unlockAllPlayerArmies()
        {
            foreach (Image image in playerArmiesImages)
            {
                image.IsEnabled = true;
            }
        }

        public void calculateMoveCounter()
        {
            int moveCounter = 0;
            //DateTime warLength = war.WarDateTimeStart;

            TimeSpan warLength = DateTime.UtcNow.Subtract(war.WarDateTimeStart);

            double currentwarLengthInSeconds = warLength.Hours * 3600 + warLength.Minutes * 60 + warLength.Seconds;

            moveCounter = Convert.ToInt32(Math.Floor(currentwarLengthInSeconds / 30));

            if (moveCounter > 720)
            {
                //delete war;
            }

            moveCounterLbl.Content = Convert.ToString(moveCounter);
        }
    }

}


