using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Enums;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LandConquest.Logic
{
    public class ConsumptionLogic
    {
        public static int CountFunction(Player player, int hoursToCount)
        {
            Army army = new Army();
            Peasants peasants = new Peasants();
            army = ArmyModel.GetArmyInfo(player, army);
            peasants = PeasantModel.GetPeasantsInfo(player, peasants);

            int totalConsumption = (peasants.PeasantsCount * (int)ConsumptionEnum.Peasants.Consumption +
                                   army.ArmyArchersCount * (int)ConsumptionEnum.Archers.Consumption +
                                  army.ArmyHorsemanCount * (int)ConsumptionEnum.Knights.Consumption +
                                  army.ArmyInfantryCount * (int)ConsumptionEnum.Infantry.Consumption +
                                  army.ArmySiegegunCount * (int)ConsumptionEnum.Siege.Consumption) * hoursToCount;
            return totalConsumption;
        }

        public static void ConsumptionCount(Player player, PlayerStorage storage)
        {
            var lastEntrance = PlayerEntranceModel.GetLastEntrance(player);
            if (lastEntrance == DateTime.MinValue)
            {
                PlayerEntranceModel.SetFirstEntrance(player);
            }
            else
            {
                int entranceDifference = (DateTime.UtcNow - lastEntrance).Hours;
                if (entranceDifference >= 1)
                {
                    PlayerEntranceModel.UpdateLastEntrance(player);
                    int consumption = CountFunction(player, entranceDifference);
                    var consumptionDifference = storage.PlayerFood - consumption;
                    if (consumptionDifference > 0)
                    {
                        storage.PlayerFood = consumptionDifference;
                        StorageModel.UpdateStorage(player, storage);
                    }
                    else
                    {
                        storage.PlayerFood = 0;
                        StorageModel.UpdateStorage(player, storage);
                        ArmyDesert(player, 4);
                        WarningDialogWindow.CallWarningDialogNoResult("Milord, part of your army deserted due to lack of provisions. Please, check your storage.");
                    }
                }
            }
        }

        public static void ArmyDesert(Player player, int part)
        {
            Army playerArmy = new Army();
            playerArmy = ArmyModel.GetArmyInfo(player, playerArmy);

            //List<ArmyInBattle> armiesInBatle = new List<ArmyInBattle>();
            //armiesInBatle = BattleModel.GetAllPlayerArmiesInfo(armiesInBatle, player);

            playerArmy.ArmyArchersCount = playerArmy.ArmyArchersCount / part;
            playerArmy.ArmyHorsemanCount = playerArmy.ArmyHorsemanCount / part;
            playerArmy.ArmyInfantryCount = playerArmy.ArmyInfantryCount / part;
            playerArmy.ArmySiegegunCount = playerArmy.ArmySiegegunCount / part;
            playerArmy.ArmySizeCurrent = playerArmy.ArmySizeCurrent / part;

            //foreach (var army in armiesInBatle)
            //{
            //    army.ArmyArchersCount = army.ArmyArchersCount / part;
            //    army.ArmyHorsemanCount = army.ArmyHorsemanCount / part;
            //    army.ArmyInfantryCount = army.ArmyInfantryCount / part;
            //    army.ArmySiegegunCount = army.ArmySiegegunCount / part;
            //    army.ArmySizeCurrent = army.ArmySizeCurrent / part;
            //}

            ArmyModel.UpdateArmy(playerArmy);
            BattleModel.DesertAllArmies(player, part);
        }

        public static async void ConsumptionCountAsync(Player player, PlayerStorage storage)
        {
            while (true)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(60 - DateTime.UtcNow.Hour).Milliseconds);
                    Task consumptionTask = new Task(() => ConsumptionCount(player, storage));
                    consumptionTask.Start();
                }
                catch { }
             }
        }
    }
}
