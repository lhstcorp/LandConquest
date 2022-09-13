using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;

namespace LandConquest.Logic
{
    public class IncomeCalculationLogic
    {
        private static string playerLvl;
        private enum Manufactures : int
        {
            wood = 0,
            clay = 1,
            coal = 2,
            fur = 3,
            wool = 4,
            soda = 5,
            lime = 6,
            leather = 7
        }
        
        // Todo: delete
        private static Player CheckLvlChange(Player player)
        {

            while (Math.Pow(player.PlayerLvl, 2) * 500 - player.PlayerExp <= 0)
            {
                player.PlayerLvl += 1;
                playerLvl = player.PlayerLvl.ToString();
            }

            return player;
        }
    }
}
