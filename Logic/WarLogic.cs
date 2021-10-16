using LandConquest.DialogWIndows;
using LandConquest.Forms;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace LandConquest.Logic
{
    public class WarLogic
    {
        public static int ReturnNumberOfCell(int row, int column)
        {
            int index = (row - 1) * 30 + column - 1;
            return index;
        }

        // WAR ENTER BLOCK                              -- January/07/2021 -- greendend
        public static void EnterInWar(War _war, Player player, ArmyInBattle _armyInBattle)
        {
            War war = new War();

            war = WarModel.GetWarById(_war);

            if (_armyInBattle.ArmySizeCurrent > 0)
            {   // PARTICIPATION                            
                WarWindow window;

                if (player.PlayerCurrentRegion == war.LandAttackerId)
                {
                    _armyInBattle.LocalLandId = 270;
                    _armyInBattle.ArmySide = 1;

                    BattleModel.InsertArmyIntoBattleTable(_armyInBattle, war);

                    List<ArmyInBattle> armiesInBattle = new List<ArmyInBattle>();

                    armiesInBattle = BattleModel.GetArmiesInfo(armiesInBattle, war);

                    WarModel.addPlayerArmyToWarScoreHistory(_armyInBattle, war);

                    window = new WarWindow(player, 1, _armyInBattle, armiesInBattle, war);
                    window.Show();
                }
                else if (player.PlayerCurrentRegion == war.LandDefenderId)
                {
                    _armyInBattle.LocalLandId = 299;
                    _armyInBattle.ArmySide = 0; // hueta

                    BattleModel.InsertArmyIntoBattleTable(_armyInBattle, war);

                    List<ArmyInBattle> armiesInBattle = new List<ArmyInBattle>();
                    for (int i = 0; i < BattleModel.SelectLastIdOfArmies(war); i++)
                    {
                        armiesInBattle.Add(new ArmyInBattle());
                    }

                    armiesInBattle = BattleModel.GetArmiesInfo(armiesInBattle, war);

                    WarModel.addPlayerArmyToWarScoreHistory(_armyInBattle, war);

                    window = new WarWindow(player, 0, _armyInBattle, armiesInBattle, war);
                    window.Show();
                }
                else WarningDialogWindow.CallWarningDialogNoResult("You are not in any lands of war.\nPlease change your position!");

            }
            else
            {   // VIEW
                if ((player.PlayerCurrentRegion == war.LandDefenderId) || (player.PlayerCurrentRegion == war.LandAttackerId))
                {
                    List<ArmyInBattle> armiesInBattle = new List<ArmyInBattle>();
                    for (int i = 0; i < BattleModel.SelectLastIdOfArmies(war); i++)
                    {
                        armiesInBattle.Add(new ArmyInBattle());
                    }

                    armiesInBattle = BattleModel.GetArmiesInfo(armiesInBattle, war);

                    WarWindow window = new WarWindow(player, _armyInBattle.ArmySide, _armyInBattle, armiesInBattle, war);
                    window.Show();
                }
                else MessageBox.Show("You are not in any lands of war.\nPlease change your position!");
            }
        }

        public static ArmyInBattle CheckFreeArmies(Army army, Player player)
        {
            ArmyInBattle freePlayerArmy = new ArmyInBattle();

            List<ArmyInBattle> armies = new List<ArmyInBattle>();
            armies = BattleModel.GetAllPlayerArmiesInfo(armies, player);

            freePlayerArmy.PlayerId = army.PlayerId;
            freePlayerArmy.ArmyId = army.ArmyId;
            freePlayerArmy.ArmySizeCurrent = army.ArmySizeCurrent;
            freePlayerArmy.ArmyType = army.ArmyType;
            freePlayerArmy.ArmyArchersCount = army.ArmyArchersCount;
            freePlayerArmy.ArmyInfantryCount = army.ArmyInfantryCount;
            freePlayerArmy.ArmySiegegunCount = army.ArmySiegegunCount;
            freePlayerArmy.ArmyHorsemanCount = army.ArmyHorsemanCount;

            for (int i = 0; i < armies.Count; i++)
            {
                freePlayerArmy.ArmySizeCurrent -= armies[i].ArmySizeCurrent;
                freePlayerArmy.ArmyArchersCount -= armies[i].ArmyArchersCount;
                freePlayerArmy.ArmyInfantryCount -= armies[i].ArmyInfantryCount;
                freePlayerArmy.ArmySiegegunCount -= armies[i].ArmySiegegunCount;
                freePlayerArmy.ArmyHorsemanCount -= armies[i].ArmyHorsemanCount;
            }

            return freePlayerArmy;
        }
    }
}
