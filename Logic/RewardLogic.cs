using System;
using System.Collections.Generic;
using System.Linq;
using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Models;

namespace LandConquest.Logic
{
    public class RewardLogic
    {
        public static void GiveDailyBonus(Player player)
        {
            var resources = new List<string> { "wood", "stone", "food", "iron", "gold_ore", "copper", "gems", "leather" };
            var random = new Random();

            var resource = resources.OrderBy(s => Guid.NewGuid()).First();

            int amount = random.Next(500, 1000);
            DailyBonusModel.GiveDailyBonusReward(player.PlayerId, resource, amount);

            if(resource == "gold_ore")
            {
                resource = "gold ore";
            }
            WarningDialogWindow.CallInfoDialogNoResult("Reward: " + resource + " of amount " + amount);
        }
    }
}
