using LandConquest.Entities;
using LandConquest.Forces;
using LandConquest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
            //imgArmy = new Image();
            //imgArmy.MouseDown += ImgArmy_MouseLeftButtonDown;
            //imgArmy.MouseEnter += ImgArmy_MouseEnter;
            //imgArmy.MouseLeave += ImgArmy_MouseLeave;
            //imgArmy.Width = 40;
            //imgArmy.Height = 40;
            //imgArmy.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));

            for (int x = 0; x < localWarMap.Columns; x++)
            {
                for (int y = 0; y < localWarMap.Rows; y++)
                {
                    Image tile = new Image();
                    tile.Source = new BitmapImage(new Uri("/Pictures/test-tile.jpg", UriKind.Relative));
                    localWarMap.Children.Add(tile);
                    gridForArmies.Children.Add(new Image());
                }
            }
            mainWarWinGrid.Children.Add(localWarMap);
            ShowArmiesOnMap();
            //test 
            //tileSelected.Source = "/Pictures/tile-test-red.jpg";

            ////ReturnNumberOfCell(20, army.LocalLandId);

            //int index = army.LocalLandId;  
            //gridForArmies.Children.RemoveAt(index);
            //gridForArmies.Children.Insert(index, imgArmy);
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
            HideAvailableTilesToMove(INDEX);
            imgArmySelected = (Image)sender;
            int index = gridForArmies.Children.IndexOf(imgArmySelected);
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
                            selectedArmy = armyInBattlesInCurrentTile[i];
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
                    Image availableTileForMoving = new Image { Source = new BitmapImage(new Uri("/Pictures/tile-test-red.jpg", UriKind.Relative)) };
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
                    Image availableTileForMoving = new Image { Source = new BitmapImage(new Uri("/Pictures/test-tile.jpg", UriKind.Relative)) };
                    //availableTileForMoving.MouseRightButtonDown += tile_MouseRightButtonDown;
                    localWarMap.Children.Insert(ind, availableTileForMoving);
                }
            }
        }

        public void ShowInfoAboutArmies(int index)
        {
            armyInBattlesInCurrentTile = new List<ArmyInBattle>();

            for (int i = 0; i < battleModel.SelectLastIdOfArmiesInCurrentTile(connection, index, war); i++)
            {
                armyInBattlesInCurrentTile.Add(new ArmyInBattle());
            }

            armyInBattlesInCurrentTile = battleModel.GetArmiesInfoInCurrentTile(connection, armyInBattlesInCurrentTile, war, index);

            warriorsInfantry.Content = armyInBattlesInCurrentTile[0].ArmyInfantryCount;
            warriorsArchers.Content = armyInBattlesInCurrentTile[0].ArmyArchersCount;
            warriorsKnights.Content = armyInBattlesInCurrentTile[0].ArmyHorsemanCount;
            warriorsSiege.Content = armyInBattlesInCurrentTile[0].ArmySiegegunCount;
            warriorsAll.Content = armyInBattlesInCurrentTile[0].ArmySizeCurrent;

            for (int i = 0; i < armyInBattlesInCurrentTile.Count; i++) 
            {
                if (player.PlayerId == armyInBattlesInCurrentTile[i].PlayerId)
                {
                    selectedArmy = armyInBattlesInCurrentTile[i];
                    f_canMoveArmy = true;
                }
            }
        }

        private void splitArmiesButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
