using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class Person
    {
        public string PlayerId { get; set; }
        public string PersonId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool MaleFemale { get; set; }
        public int Prestige { get; set; }
        public string Title { get; set; }
        public int Lvl { get; set; }
        public int Exp { get; set; }
        public int Power { get; set; }
        public int Health { get; set; }
        public int Intellect { get; set; }
        public int Agility { get; set; }
        public Person() { }
    }
}
