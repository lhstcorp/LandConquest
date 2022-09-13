using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public static class CommonModel
    {
        public static Random random;
        public static string GenerateId()
        {
            System.Threading.Thread.Sleep(15);
            random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static TimeSpan StripMilliseconds(this TimeSpan time)
        {
            return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
        }
    }
}
