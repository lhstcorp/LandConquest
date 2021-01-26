using LandConquestDB.Entities;
using LandConquestDB.Enums;

namespace LandConquestDB.Models
{
    public class ConsumptionModel
    {
        public static void CountConsumption(Player player)
        {
            Army army = new Army();
            Peasants peasants = new Peasants();
            army = ArmyModel.GetArmyInfo(player, army);
            peasants = PeasantModel.GetPeasantsInfo(player, peasants);

            int hourConsumption = peasants.PeasantsCount * (int)ConsumptionEnum.Peasants.Consumption + 
                                   army.ArmyArchersCount * (int)ConsumptionEnum.Archers.Consumption +
                                  army.ArmyHorsemanCount * (int)ConsumptionEnum.Knights.Consumption +
                                  army.ArmyInfantryCount * (int)ConsumptionEnum.Infantry.Consumption +
                                  army.ArmySiegegunCount * (int)ConsumptionEnum.Siege.Consumption;

        }
    }
}
