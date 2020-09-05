using LandConquest.DialogWIndows;
using LandConquest.Entities;
using LandConquest.Forces;
using LandConquest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для WarWindow.xaml
    /// </summary>
    public partial class WarWindow : Window
    {
        System.Windows.Controls.Primitives.UniformGrid localWarMap = new System.Windows.Controls.Primitives.UniformGrid();
        Image imgArmySelected;
        Boolean f_armySelected = false;
        Boolean f_canMoveArmy = false;
        int INDEX;

        ArmyModel armyModel = new ArmyModel();
        BattleModel battleModel = new BattleModel();

        SqlConnection connection;
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

        //Canvas localWarArmyLayer = new Canvas();
        public WarWindow(SqlConnection _connection, Player _player, ArmyInBattle _army, List<ArmyInBattle> _armies, War _war)
        {
            connection = _connection;
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
            localWarMap.VerticalAlignment = VerticalAlignment.Center;
            gridForArmies.HorizontalAlignment = HorizontalAlignment.Left;
            gridForArmies.VerticalAlignment = VerticalAlignment.Center;
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
        }

        public void ShowArmiesOnMap()
        {
            //emptyImage.Source = new BitmapImage(new Uri("", UriKind.Relative));
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

                switch (armies[i].ArmyType)
                {
                    case 1:
                        {
                            imgArmy.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
                            break;
                        }
                    case 2:
                        {
                            imgArmy.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                            break;
                        }
                    case 3:
                        {
                            imgArmy.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                            break;
                        }
                    case 4:
                        {
                            imgArmy.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                            break;
                        }
                    case 5:
                        {
                            imgArmy.Source = new BitmapImage(new Uri("/Pictures/peasants_total.png", UriKind.Relative));
                            break;
                        }
                }
                armyImages.Add(imgArmy);

                if (((Image)gridForArmies.Children[armies[i].LocalLandId]).Source == emptyImage.Source)
                {
                    gridForArmies.Children.RemoveAt(armies[i].LocalLandId);
                }
                else
                {
                    if (((Image)gridForArmies.Children[armies[i].LocalLandId]).Source != imgArmy.Source)
                    {
                        gridForArmies.Children.RemoveAt(armies[i].LocalLandId);
                        imgArmy.Source = new BitmapImage(new Uri("/Pictures/peasants_total.png", UriKind.Relative));
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

                for (int i = 0; i < battleModel.SelectLastIdOfArmiesInCurrentTile(connection, INDEX, war); i++)
                {
                    armyInBattlesInCurrentTile.Add(new ArmyInBattle());
                }

                armyInBattlesInCurrentTile = battleModel.GetArmiesInfoInCurrentTile(connection, armyInBattlesInCurrentTile, war, INDEX);
                //armyInBattlesInCurrentTile.Add(selectedArmy);

                Image imgArmyThatStay = new Image();
                imgArmyThatStay.MouseLeftButtonDown += ImgArmy_MouseLeftButtonDown;
                imgArmyThatStay.MouseEnter += ImgArmy_MouseEnter;
                imgArmyThatStay.MouseLeave += ImgArmy_MouseLeave;
                imgArmyThatStay.MouseRightButtonDown += ImgArmy_MouseRightButtonDown;
                imgArmyThatStay.Width = 40;
                imgArmyThatStay.Height = 40;

                if (armyInBattlesInCurrentTile.Count > 1)
                {
                    Console.WriteLine("armyInBattlesInCurrentTile.Count " + armyInBattlesInCurrentTile.Count);
                    for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++)
                    {
                        if (armyInBattlesInCurrentTile[i].PlayerId == player.PlayerId)
                        {
                            //selectedArmy = armyInBattlesInCurrentTile[i];
                            //Console.WriteLine()
                            armyInBattlesInCurrentTile.Remove(armyInBattlesInCurrentTile[i]);
                            switch (selectedArmy.ArmyType)
                            {
                                case 1:
                                    {
                                        imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
                                        break;
                                    }
                                case 2:
                                    {
                                        imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                                        break;
                                    }
                                case 3:
                                    {
                                        imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                                        break;
                                    }
                                case 4:
                                    {
                                        imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                                        break;
                                    }
                                case 5:
                                    {
                                        imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/peasants_total.png", UriKind.Relative));
                                        break;
                                    }
                            }
                        }
                    }

                    int typeOfUniteArmy = battleModel.ReturnTypeOfArmy(armyInBattlesInCurrentTile);

                    switch (typeOfUniteArmy)
                    {
                        case 1:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
                                break;
                            }
                        case 2:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                                break;
                            }
                        case 3:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                                break;
                            }
                        case 4:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                                break;
                            }
                        case 5:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/peasants_total.png", UriKind.Relative));
                                break;
                            }
                    }


                    int index = localWarMap.Children.IndexOf((Image)sender);
                    gridForArmies.Children.RemoveAt(INDEX);
                    //((Image)gridForArmies.Children[INDEX]).Source = imgArmyThatStay.Source;
                    gridForArmies.Children.Insert(INDEX, imgArmyThatStay);
                    //gridForArmies.Children.Insert(INDEX, new Image());
                    gridForArmies.Children.RemoveAt(index); //?
                    gridForArmies.Children.Insert(index, imgArmySelected);

                    HideAvailableTilesToMove(INDEX);
                    battleModel.UpdateLocalLandOfArmy(connection, selectedArmy, index);
                }
                else
                {
                    Console.WriteLine("InDeX::::" + INDEX);
                    //Console.WriteLine("armyInBattlesInCurrentTile.Count " + armyInBattlesInCurrentTile.Count);
                    int index = localWarMap.Children.IndexOf((Image)sender);
                    gridForArmies.Children.RemoveAt(INDEX);
                    gridForArmies.Children.Insert(INDEX, new Image());
                    gridForArmies.Children.RemoveAt(index);
                    gridForArmies.Children.Insert(index, imgArmySelected);

                    HideAvailableTilesToMove(INDEX);
                    battleModel.UpdateLocalLandOfArmy(connection, selectedArmy, index);
                }
            }
        }

        private void ImgArmy_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (f_armySelected)
            {
                f_armySelected = false;
                int index = gridForArmies.Children.IndexOf((Image)sender);

                gridForArmies.Children.RemoveAt(INDEX);
                gridForArmies.Children.Insert(INDEX, new Image());

                gridForArmies.Children.RemoveAt(index);
                // call method retype;

                armyInBattlesInCurrentTile = new List<ArmyInBattle>();

                for (int i = 0; i < battleModel.SelectLastIdOfArmiesInCurrentTile(connection, index, war); i++)
                {
                    armyInBattlesInCurrentTile.Add(new ArmyInBattle());
                }

                armyInBattlesInCurrentTile = battleModel.GetArmiesInfoInCurrentTile(connection, armyInBattlesInCurrentTile, war, index);
                armyInBattlesInCurrentTile.Add(selectedArmy);

                int typeOfUniteArmy = battleModel.ReturnTypeOfArmy(armyInBattlesInCurrentTile);

                switch (typeOfUniteArmy)
                {
                    case 1:
                        {
                            imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
                            break;
                        }
                    case 2:
                        {
                            imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                            break;
                        }
                    case 3:
                        {
                            imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                            break;
                        }
                    case 4:
                        {
                            imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                            break;
                        }
                    case 5:
                        {
                            imgArmySelected.Source = new BitmapImage(new Uri("/Pictures/peasants_total.png", UriKind.Relative));
                            break;
                        }
                }
                //armyImages.Add(imgArmySelected);

                gridForArmies.Children.Insert(index, imgArmySelected);
                HideAvailableTilesToMove(INDEX);

                battleModel.UpdateLocalLandOfArmy(connection, selectedArmy, index);



                //Console.WriteLine("COUNT = " + armyInBattlesInCurrentTile.Count);

                //перезаписываем в этот лист армии что остались
                armyInBattlesInCurrentTile.Clear();

                for (int i = 0; i < battleModel.SelectLastIdOfArmiesInCurrentTile(connection, INDEX, war); i++)
                {
                    armyInBattlesInCurrentTile.Add(new ArmyInBattle());
                }

                armyInBattlesInCurrentTile = battleModel.GetArmiesInfoInCurrentTile(connection, armyInBattlesInCurrentTile, war, INDEX);

                if (armyInBattlesInCurrentTile.Count >= 1)
                {
                    Image imgArmyThatStay = new Image();
                    imgArmyThatStay.MouseLeftButtonDown += ImgArmy_MouseLeftButtonDown;
                    imgArmyThatStay.MouseEnter += ImgArmy_MouseEnter;
                    imgArmyThatStay.MouseLeave += ImgArmy_MouseLeave;
                    imgArmyThatStay.MouseRightButtonDown += ImgArmy_MouseRightButtonDown;
                    imgArmyThatStay.Width = 40;
                    imgArmyThatStay.Height = 40;

                    int typeOfUniteArmy2 = battleModel.ReturnTypeOfArmy(armyInBattlesInCurrentTile);

                    switch (typeOfUniteArmy2)
                    {
                        case 1:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
                                break;
                            }
                        case 2:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/archer.png", UriKind.Relative));
                                break;
                            }
                        case 3:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/hourseman.png", UriKind.Relative));
                                break;
                            }
                        case 4:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/catapult.png", UriKind.Relative));
                                break;
                            }
                        case 5:
                            {
                                imgArmyThatStay.Source = new BitmapImage(new Uri("/Pictures/peasants_total.png", UriKind.Relative));
                                break;
                            }
                    }


                    gridForArmies.Children.RemoveAt(INDEX);
                    gridForArmies.Children.Insert(INDEX, imgArmyThatStay);
                }
                //ShowAvailableTilesToMove(index);
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

            for (int i = 0; i < battleModel.SelectLastIdOfArmiesInCurrentTile(connection, index, war); i++)
            {
                armyInBattlesInCurrentTile.Add(new ArmyInBattle());
                selectedArmiesForUnion.Add(false);
            }

            armyInBattlesInCurrentTile = battleModel.GetArmiesInfoInCurrentTile(connection, armyInBattlesInCurrentTile, war, index);

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
            SplitArmyDialog dialogWindow = new SplitArmyDialog(connection, armyInBattlesInCurrentTile[0], war);
            dialogWindow.Show();
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
                            battleModel.DeleteArmyById(connection, armyInBattlesInCurrentTile[i]);
                        }
                    }
                }

                unionArmy.ArmyType = battleModel.ReturnTypeOfUnionArmy(unionArmy);
                battleModel.UpdateArmyInBattle(connection, unionArmy);
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
                    tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/g1.jpg", UriKind.Relative));
                    Colorrr = "w";
                }
                else {
                    Colorrr = "d";
                    tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/g2.jpg", UriKind.Relative)); 
                }
            }
            else
            {
                if (((Column % 2 == 0) && (Row % 2 == 0)) || ((Column % 2 == 1) && (Row % 2 == 1)))
                {
                    tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/r1.jpg", UriKind.Relative));
                }
                else tile.Source = new BitmapImage(new Uri("/Pictures/Tiles/r2.jpg", UriKind.Relative));
            }
            return tile;
        }

        private void btnWarWindowClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
