using System;
using System.Collections.Generic;
using System.Text;

namespace LandConquestServer.Entities
{
    public class WarScore
    {
        public string PlayerId { get; set; }

        public int Score { get; set; }

        public int Prestige { get; set; }

        public string WarId { get; set; }
    }
}
