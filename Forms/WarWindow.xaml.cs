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
        // --> CONST part
        private const int syncTick = 30; //sec
        private const int castleAttackerLocalLandId = 270; //red castle
        private const int castleDefenderLocalLandId = 299; //blue castle
        // <--

        private System.Windows.Controls.Primitives.UniformGrid localWarMap;
        private Image imgArmySelected;
        private bool f_armySelected = false;
        private bool f_canMoveArmy = false;
        private int INDEX;
        private int timerValue = 30;
        private Player player;
        private int playerSide;
        private List<ArmyInBattle> armies;
        private List<Image> armyImages;
        private ArmyInBattle army;
        private ArmyInBattle selectedArmy;
        private List<ArmyInBattle> armyInBattlesInCurrentTile;
        private War war;
        private int saveMovement;
        private int saveRange;
        private Image emptyImage;
        private int armyPage;
        private int index;
        private List<bool> selectedArmiesForUnion;
        private List<ArmyInBattle> yourArmiesInCurrentTile;
        private int SelectionCounter;
        private bool shoot = false;
        private DispatcherTimer syncTimer;
        public static List<ArmyInBattle> playerArmies;
        private int moveCounter = 0;

        //Canvas localWarArmyLayer = new Canvas();
        public WarWindow(Player _player, int _playerSide, ArmyInBattle _army, List<ArmyInBattle> _armies, War _war)
        {
            player = _player;
            playerSide = _playerSide;
            army = _army;
            armies = _armies;
            war = _war;
            playerArmies = new List<ArmyInBattle>();
            yourArmiesInCurrentTile = new List<ArmyInBattle>();
            emptyImage = new Image();
            localWarMap = new System.Windows.Controls.Primitives.UniformGrid();

            InitializeComponent();
            int columnWidth = 40;
            int rowHeight = 40;
            localWarMap.Columns = 30; //50
            localWarMap.Rows = 20;    //40
            gridForArmies.Columns = 30;
            gridForArmies.Rows = 20;
            elementsWarMap.Columns = 30;
            elementsWarMap.Rows = 20;

            localWarMap.Width = columnWidth * localWarMap.Columns;
            gridForArmies.Width = columnWidth * localWarMap.Columns;
            elementsWarMap.Width = columnWidth * localWarMap.Columns;
            localWarMap.Height = rowHeight * localWarMap.Rows;
            gridForArmies.Height = rowHeight * localWarMap.Rows;
            elementsWarMap.Height = rowHeight * localWarMap.Rows;

            localWarMap.HorizontalAlignment = HorizontalAlignment.Left;
            localWarMap.Margin = new Thickness(35, 0, 0, 0);
            localWarMap.VerticalAlignment = VerticalAlignment.Center;
            localWarMap.Opacity = 0.8;
            gridForArmies.HorizontalAlignment = HorizontalAlignment.Left;
            gridForArmies.Margin = new Thickness(35, 0, 0, 0);
            gridForArmies.VerticalAlignment = VerticalAlignment.Center;
            elementsWarMap.HorizontalAlignment = HorizontalAlignment.Left;
            elementsWarMap.Margin = new Thickness(35, 0, 0, 0);
            elementsWarMap.VerticalAlignment = VerticalAlignment.Center;

            timerLabel.Content = "syncing...";
            Loaded += WarWin_Loaded;

        }

        public static List<ArmyInBattle> GetPlayerArmies()
        {
            return playerArmies;
        }

        public static void SetPlayerArmies(List<ArmyInBattle> _playerArmies)
        {
            playerArmies = _playerArmies;
        }

        private void WarWin_Loaded(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < localWarMap.Rows; x++)
            {
                for (int y = 0; y < localWarMap.Columns; y++)
                {
                    Image tile = new Image();
                    tile = AddSourceForTile(tile, "move", 0, x, y);
                    localWarMap.Children.Add(tile);
                    gridForArmies.Children.Add(new Image());
                    elementsWarMap.Children.Add(new Image());
                }
            }

            mainWarWinGrid.Children.Add(localWarMap);

            ShowElementsOnMap();
            ShowArmiesOnMap();
            SyncWithServer();
        }

        public void ShowElementsOnMap()
        {
            Image redCastle = new Image();
            redCastle.Source = new BitmapImage(new Uri("/Pictures/War/RedCastle.png", UriKind.Relative));

            elementsWarMap.Children.RemoveAt(castleAttackerLocalLandId);
            elementsWarMap.Children.Insert(castleAttackerLocalLandId, redCastle);

            Image blueCastle = new Image();
            blueCastle.Source = new BitmapImage(new Uri("/Pictures/War/BlueCastle.png", UriKind.Relative));

            elementsWarMap.Children.RemoveAt(castleDefenderLocalLandId);
            elementsWarMap.Children.Insert(castleDefenderLocalLandId, blueCastle);

            subscribeToCastleEvents();

        }

        public void ShowArmiesOnMap()
        {
            armies.Clear();
            armies = BattleModel.GetArmiesInfo(armies, war);

            deleteImagesOnWarGrid();
            addEmptyImagesOnWarMap();

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
                        SwitchArmyTypeOfDefenders(armies[i].ArmyType, imgArmy);
                    }
                    else
                    {
                        SwitchArmyTypeOfAttackers(armies[i].ArmyType, imgArmy);
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
                                SwitchArmyTypeOfDefenders(BattleModel.ReturnTypeOfArmy(armyInOneTileTest), imgArmy);
                            }
                            else
                            {
                                SwitchArmyTypeOfAttackers(BattleModel.ReturnTypeOfArmy(armyInOneTileTest), imgArmy);
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
            shoot = false;
            armyPage = 0;
            HideAvailableTilesToMove(INDEX);
            HideAvailableTilesToShoot(INDEX);
            imgArmySelected = (Image)sender;
            index = gridForArmies.Children.IndexOf(imgArmySelected);
            localWarMap.Children.RemoveAt(index);
            localWarMap.Children.Insert(index, new Image { Source = new BitmapImage(new Uri("/Pictures/Tiles/y1.jpg", UriKind.Relative)) });

            f_armySelected = true;
            INDEX = index;

            ShowInfoAboutArmies(index);

            if (f_canMoveArmy && findPlayerArmyCanMove())
            {
                ShowAvailableTilesToMove(index);
            }

            if (index == castleDefenderLocalLandId || index == castleAttackerLocalLandId) // if the tile is a castle tile
            {
                ShowGarrisonInfo(index);
            }
        }

        private void tile_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (f_armySelected)
            {
                f_armySelected = false;

                armyInBattlesInCurrentTile = new List<ArmyInBattle>();
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
                        if (armyInBattlesInCurrentTile[i].PlayerId == player.PlayerId && armyInBattlesInCurrentTile[i].ArmyId == selectedArmy.ArmyId)
                        {
                            armyInBattlesInCurrentTile.Remove(armyInBattlesInCurrentTile[i]);

                            if (selectedArmy.ArmySide == 0)
                            {
                                SwitchArmyTypeOfDefenders(selectedArmy.ArmyType, imgArmySelected);
                            }
                            else
                            {
                                SwitchArmyTypeOfAttackers(selectedArmy.ArmyType, imgArmySelected);
                            }
                        }
                    }

                    int typeOfUniteArmy = BattleModel.ReturnTypeOfArmy(armyInBattlesInCurrentTile);

                    if (armyInBattlesInCurrentTile[0].ArmySide == 0)
                    {
                        SwitchArmyTypeOfDefenders(typeOfUniteArmy, imgArmyThatStay);
                    }
                    else
                    {
                        SwitchArmyTypeOfAttackers(typeOfUniteArmy, imgArmyThatStay);
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

                //imgArmySelected.IsEnabled = false;
                lockSelectedArmy();
            }
        }

        private void ImgArmy_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (f_armySelected && findPlayerArmyCanMove() && !shoot) //<-- sma 16.01.2021
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
                            SwitchArmyTypeOfDefenders(typeOfUniteArmy, imgArmySelected);
                        }
                        else
                        {
                            SwitchArmyTypeOfAttackers(typeOfUniteArmy, imgArmySelected);
                        }
                    }
                    //armyImages.Add(imgArmySelected);

                    gridForArmies.Children.Insert(index, imgArmySelected);
                    HideAvailableTilesToMove(INDEX);

                    BattleModel.UpdateLocalLandOfArmy(selectedArmy, index);


                    //перезаписываем в этот лист армии что остались
                    armyInBattlesInCurrentTile.Clear();
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
                            SwitchArmyTypeOfDefenders(typeOfUniteArmy2, imgArmyThatStay);
                        }
                        else
                        {
                            SwitchArmyTypeOfAttackers(typeOfUniteArmy2, imgArmyThatStay);
                        }


                        gridForArmies.Children.RemoveAt(INDEX);
                        gridForArmies.Children.Insert(INDEX, imgArmyThatStay);
                    }
                    lockSelectedArmy();
                }
            }
            else if (shoot && findPlayerArmyCanShoot() && checkRange(gridForArmies.Children.IndexOf((Image)sender))) // не хватает условия проверки что в этом тайле кто-то есть из врагов
            {
                DistanceBattle distanceBattle = new DistanceBattle();
                DistanceBattle distanceBattleExist = new DistanceBattle();

                distanceBattle.BattleId = generateId();
                distanceBattle.Damage = BattleModel.CalculateDamageInDistanceBattle(selectedArmy);
                distanceBattle.LocalLandId = gridForArmies.Children.IndexOf((Image)sender);
                distanceBattle.WarId = war.WarId;
                if (selectedArmy.ArmySide == 0)
                {
                    distanceBattle.Side = 1;
                }
                else
                {
                    distanceBattle.Side = 0;
                }

                distanceBattleExist = BattleModel.DistanceBattleExist(distanceBattle);
                if (distanceBattleExist.BattleId != "")
                {
                    distanceBattleExist.Damage += distanceBattle.Damage;
                    BattleModel.UpdateDistanceBattle(distanceBattleExist);
                }
                else
                {
                    BattleModel.InsertDistanceBattle(distanceBattle);
                }

                HideAvailableTilesToShoot(index);
                lockArmyShoot();
            }
        }

        public bool checkRange(int indexArmyToShoot)
        {
            bool ret = true;

            int row1 = index / localWarMap.Columns + 1;
            int col1 = index - localWarMap.Columns * (row1 - 1) + 1;

            int row2 = indexArmyToShoot / localWarMap.Columns + 1;
            int col2 = indexArmyToShoot - localWarMap.Columns * (row2 - 1) + 1;

            int range = 0;

            if (selectedArmy.ArmySiegegunCount > 0)
            {
                range = (int)ForcesEnum.Siege.Range;
            }

            if (selectedArmy.ArmyArchersCount > 0)
            {
                range = (int)ForcesEnum.Archers.Range;
            }

            if ((Math.Abs(row1 - row2) > range) && (Math.Abs(col1 - col2) > range))
            {
                ret = false;
            }

            return ret;
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

            saveMovement = movement;
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
                    availableTileForMoving = AddSourceForTile(availableTileForMoving, "move", 1, i, j);
                    availableTileForMoving.MouseRightButtonDown += tile_MouseRightButtonDown;
                    localWarMap.Children.Insert(ind, availableTileForMoving);
                }
            }
        }

        public void ShowAvailableTilesToShoot(int index) //+ int forces movement speed;
        {
            int row = index / localWarMap.Columns + 1;
            int col = index - localWarMap.Columns * (row - 1) + 1;

            int range = 0;

            if (selectedArmy.ArmySiegegunCount > 0)
            {
                range = (int)ForcesEnum.Siege.Range;
            }
            
            if (selectedArmy.ArmyArchersCount > 0)
            {
                range = (int)ForcesEnum.Archers.Range;
            }


            saveRange = range;
            //????????????????????????????????????????????????????????
            for (int i = row - range; i <= row + range; i++)
            {
                for (int j = col - range; j <= col + range; j++)
                {
                    if ((i < 1) || (j < 1) || (j > localWarMap.Columns) || (i > localWarMap.Rows)) continue;  //
                    int ind = ReturnNumberOfCell(i, j);
                    localWarMap.Children.RemoveAt(ind);
                    //Image availableTileForMoving = new Image { Source = new BitmapImage(new Uri("/Pictures/tile-test-red.jpg", UriKind.Relative)) };
                    Image availableTileToShoot = new Image();
                    availableTileToShoot = AddSourceForTile(availableTileToShoot, "shoot", 1, i, j);
                    localWarMap.Children.Insert(ind, availableTileToShoot);
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
            for (int i = row - saveMovement; i <= row + saveMovement; i++)
            {
                for (int j = col - saveMovement; j <= col + saveMovement; j++)
                {
                    if ((i < 1) || (j < 1) || (j > localWarMap.Columns) || (i > localWarMap.Rows)) continue;  //
                    int ind = ReturnNumberOfCell(i, j);
                    localWarMap.Children.RemoveAt(ind);
                    //Image availableTileForMoving = new Image { Source = new BitmapImage(new Uri("/Pictures/test-tile.jpg", UriKind.Relative)) };
                    Image availableTileForMoving = new Image();
                    availableTileForMoving = AddSourceForTile(availableTileForMoving, "move", 0, i, j);
                    //availableTileForMoving.MouseRightButtonDown += tile_MouseRightButtonDown;
                    localWarMap.Children.Insert(ind, availableTileForMoving);
                }
            }
        }

        public void HideAvailableTilesToShoot(int index) //+ int forces movement speed;
        {

            int row = index / localWarMap.Columns + 1;
            int col = index - localWarMap.Columns * (row - 1) + 1;

            for (int i = row - saveRange; i <= row + saveRange; i++)
            {
                for (int j = col - saveRange; j <= col + saveRange; j++)
                {
                    if ((i < 1) || (j < 1) || (j > localWarMap.Columns) || (i > localWarMap.Rows)) continue;
                    int ind = ReturnNumberOfCell(i, j);
                    localWarMap.Children.RemoveAt(ind);
                    Image availableTileForMoving = new Image();
                    availableTileForMoving = AddSourceForTile(availableTileForMoving, "move", 0, i, j);
                    localWarMap.Children.Insert(ind, availableTileForMoving);
                }
            }
        }

        public void ShowInfoAboutArmies(int index)
        {
            SelectionCounter = 0;
            selectedArmiesForUnion = new List<bool>();
            armyInBattlesInCurrentTile = new List<ArmyInBattle>();
            armyInBattlesInCurrentTile = BattleModel.GetArmiesInfoInCurrentTile(armyInBattlesInCurrentTile, war, index);
            for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++)
            {
                selectedArmiesForUnion.Add(false);
            }


            ShowInfoAboutArmy();

            for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++)
            {
                if (armyInBattlesInCurrentTile[i].PlayerId == player.PlayerId)
                {
                    yourArmiesInCurrentTile.Add(armyInBattlesInCurrentTile[i]);
                }
            }
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            HideAvailableTilesToMove(index);
            SplitArmyDialog dialogWindow = new SplitArmyDialog(returnPlayerArmy(), war);
            dialogWindow.Show();
        }

        private void btnAttack_Click(object sender, RoutedEventArgs e) //select army for attack
        {
            shoot = !shoot;

            if (findPlayerArmyCanShoot())
            {
                if (shoot)
                    ShowAvailableTilesToShoot(index);
                else
                {
                    HideAvailableTilesToShoot(index);
                    localWarMap.Children.RemoveAt(index);
                    localWarMap.Children.Insert(index, new Image { Source = new BitmapImage(new Uri("/Pictures/Tiles/y1.jpg", UriKind.Relative)) });
                }
            }
        }

        private void ImgWar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ArmyInBattle leftSide = new ArmyInBattle();
            ArmyInBattle rightSide = new ArmyInBattle();
            int index = gridForArmies.Children.IndexOf((Image)sender);

            List<ArmyInBattle> armyInBattlesInCurrentTile = new List<ArmyInBattle>();
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

                btnSelectToUnite.IsEnabled = true;
                btnUnite.IsEnabled = true;
                btnSplit.IsEnabled = true;

                if (findPlayerArmyCanMove())
                {
                    ShowAvailableTilesToMove(index);
                }

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

                btnSelectToUnite.IsEnabled = false;
                btnUnite.IsEnabled = false;
                btnSplit.IsEnabled = false;
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

        public Image AddSourceForTile(Image tile, string operation, int tileColor, int Row, int Column)
        {
            //tileColor = 0 ? green
            //tileColor = 1 ? red

            if (operation == "move")
            {
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
                        tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/bj2.jpg", UriKind.Relative)); //bj2
                    }
                    else tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/br1.jpg", UriKind.Relative));
                }
            }
            else if (operation == "shoot")
            {
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
                        tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/g2.jpg", UriKind.Relative));
                    }
                    else tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/gra1.jpg", UriKind.Relative));
                }
            }

            return tile;
        }



        private void SwitchArmyTypeOfDefenders(int armyType, Image armyImage)
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

        private void SwitchArmyTypeOfAttackers(int armyType, Image armyImage)
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
        private void buttonCollapse_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState = WindowState.Minimized;
        }

        public bool CheckDistanceBetweenTwoArmies(int index, int INDEX)
        {
            int rowI1 = ReturnRowByIndex(index);
            int rowI2 = ReturnRowByIndex(INDEX);
            int colI1 = ReturnColumnByIndex(index);
            int colI2 = ReturnColumnByIndex(INDEX);

            if ((Math.Abs(rowI1 - rowI2) <= saveMovement) && ((Math.Abs(colI1 - colI2) <= saveMovement)))
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

            ShowArmiesOnMap();

            moveCounter++;
            if (moveCounter % 2 == playerSide)
            {
                searchPlayerArmies();
                unlockAllPlayerArmies();
            }

            setCurrentMoveColor();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            
            ShowArmiesOnMap();

            moveCounter++;
            if (moveCounter % 2 == playerSide)
            {
                searchPlayerArmies();
                unlockAllPlayerArmies();
            }
            else
            {
                lockAllPlayerArmies();
                HideAvailableTilesToMove(index);
                HideAvailableTilesToShoot(index);
            }

            setCurrentMoveColor();

            timerValue = 30;
        }

        public void searchPlayerArmies()
        {
            playerArmies.Clear();
            playerArmies = BattleModel.GetPlayerArmiesInfo(playerArmies, war, player);

        }
        public void lockAllPlayerArmies()
        {
            for (int i = 0; i < playerArmies.Count; i++)
            {
                playerArmies[i].CanMove = false;
                playerArmies[i].CanShoot = false;
            }
        }

        public void unlockAllPlayerArmies()
        {
            for (int i = 0; i < playerArmies.Count; i++)
            {
                playerArmies[i].CanMove = true;
                playerArmies[i].CanShoot = true;
            }
        }

        public void calculateMoveCounter()
        {
            //DateTime warLength = war.WarDateTimeStart;
            TimeSpan warLength = DateTime.UtcNow.Subtract(war.WarDateTimeStart);
            double currentwarLengthInSeconds = warLength.Hours * 3600 + warLength.Minutes * 60 + warLength.Seconds;
            moveCounter = Convert.ToInt32(Math.Floor(currentwarLengthInSeconds / 30));
            moveCounterLbl.Content = Convert.ToString(moveCounter);
            setCurrentMoveColor();
        }

        public void lockSelectedArmy()
        {
            for (int i = 0; i < playerArmies.Count; i++)
            {
                if (playerArmies[i].ArmyId == selectedArmy.ArmyId)
                {
                    playerArmies[i].CanMove = false;
                    playerArmies[i].CanShoot = false;
                }
            }
        }

        public bool findPlayerArmyCanMove()
        {
            for (int i = 0; i < playerArmies.Count; i++)
            {
                if (playerArmies[i].ArmyId == selectedArmy.ArmyId)
                    return playerArmies[i].CanMove;
            }
            return false;
        }

        public bool findPlayerArmyCanShoot()
        {
            for (int i = 0; i < playerArmies.Count; i++)
            {
                if (playerArmies[i].ArmyId == selectedArmy.ArmyId)
                    return playerArmies[i].CanShoot;
            }
            return false;
        }

        private void mainWarWinGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        public void lockArmyShoot()
        {
            for (int i = 0; i < playerArmies.Count; i++)
            {
                if (playerArmies[i].ArmyId == selectedArmy.ArmyId)
                {
                    playerArmies[i].CanShoot = false;
                    break;
                }
            }
        }

        public ArmyInBattle returnPlayerArmy()
        {
            for (int i = 0; i < playerArmies.Count; i++)
            {
                if (playerArmies[i].ArmyId == selectedArmy.ArmyId)
                {
                    return playerArmies[i];
                }
            }
            return null;
        }

        public void deleteImagesOnWarGrid()
        {
            for (int i = 0; i < localWarMap.Rows * localWarMap.Columns; i++)
            {
                gridForArmies.Children.RemoveAt(0);
            }
        }

        public void addEmptyImagesOnWarMap()
        {
            for (int i = 0; i < localWarMap.Rows * localWarMap.Columns; i++)
            {
                gridForArmies.Children.Add(new Image());
            }
        }

        public void setCurrentMoveColor()
        {
            if (moveCounter % 2 == 0)
                thisMoveIndicator.Fill = new SolidColorBrush(Colors.DarkBlue);
            else
                thisMoveIndicator.Fill = new SolidColorBrush(Colors.DarkRed);
        }

        public void ShowGarrisonInfo(int _locationId)
        {
            List<Garrison> garrisons;
            if (_locationId == castleDefenderLocalLandId)
            {
                garrisons = GarrisonModel.GetGarrisonInfo(war.LandDefenderId);
            }
            else
            {
                garrisons = GarrisonModel.GetGarrisonInfo(war.LandAttackerId);
            }

            Garrison fullGarrison = new Garrison();

            for (int i = 0; i < garrisons.Count; i++)
            {
                fullGarrison.ArmyInfantryCount += garrisons[i].ArmyInfantryCount;
                fullGarrison.ArmyArchersCount += garrisons[i].ArmyArchersCount;
                fullGarrison.ArmyHorsemanCount += garrisons[i].ArmyHorsemanCount;
                fullGarrison.ArmySiegegunCount += garrisons[i].ArmySiegegunCount;
                fullGarrison.ArmySizeCurrent += garrisons[i].ArmySizeCurrent;
            }

            GwarriorsAll.Content = fullGarrison.ArmySizeCurrent;
            GwarriorsInfantry.Content = fullGarrison.ArmyInfantryCount;
            GwarriorsArchers.Content = fullGarrison.ArmyArchersCount;
            GwarriorsKnights.Content = fullGarrison.ArmyHorsemanCount;
            GwarriorsSiege.Content = fullGarrison.ArmySiegegunCount;

            garrisonInfoGrid.Visibility = Visibility.Visible;
        }

        private void closeGInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            garrisonInfoGrid.Visibility = Visibility.Hidden;
        }

        public void subscribeToCastleEvents()
        {
            elementsWarMap.Children[castleAttackerLocalLandId].MouseLeftButtonDown += RedCastle_MouseLeftButtonDown;
            elementsWarMap.Children[castleDefenderLocalLandId].MouseLeftButtonDown += BlueCastle_MouseLeftButtonDown;
            elementsWarMap.Children[castleAttackerLocalLandId].MouseRightButtonDown += RedCastle_MouseRightButtonDown;// (gridForArmies.Children[castleAttackerLocalLandId], new MouseButtonEventArgs());
            elementsWarMap.Children[castleDefenderLocalLandId].MouseRightButtonDown += BlueCastle_MouseRightButtonDown;
            elementsWarMap.Children[castleAttackerLocalLandId].MouseEnter += Castle_MouseEnter;
            elementsWarMap.Children[castleDefenderLocalLandId].MouseEnter += Castle_MouseEnter;
            elementsWarMap.Children[castleAttackerLocalLandId].MouseLeave += Castle_MouseLeave;
            elementsWarMap.Children[castleDefenderLocalLandId].MouseLeave += Castle_MouseLeave;
            

        }

        private void RedCastle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowGarrisonInfo(castleAttackerLocalLandId);
            garrisonInfoGrid.Visibility = Visibility.Visible;
        }

        private void BlueCastle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowGarrisonInfo(castleDefenderLocalLandId);
            garrisonInfoGrid.Visibility = Visibility.Visible;
        }

        private void RedCastle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImgArmy_MouseRightButtonDown(gridForArmies.Children[castleAttackerLocalLandId], e);
        }

        private void BlueCastle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImgArmy_MouseRightButtonDown(gridForArmies.Children[castleDefenderLocalLandId], e);
        }

        private void Castle_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Castle_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }

}


