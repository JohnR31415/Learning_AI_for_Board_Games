using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class Continent : ICloneable
    {
        public string name;
        public List<Territory> territories;
        public int value;

        public Continent(string name, int value)
        {
            territories = new List<Territory>();
            this.name = name;
            this.value = value;
        }

        public void addTerritory(Territory territory)
        {
            territories.Add(territory);
        }

        public object Clone()
        {
            Continent ret = new Continent(name, value);
            List<Territory> cloneTerritories = new List<Territory>();
            foreach (Territory t in territories)
            {
                cloneTerritories.Add((Territory)t.Clone());
            }
            ret.territories = cloneTerritories;
            return ret;
        }
    }
}
