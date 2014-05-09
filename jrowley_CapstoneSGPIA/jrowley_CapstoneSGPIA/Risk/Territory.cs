using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class Territory : ICloneable
    {
        public string name;
        public int armies;
        public Player owner;
        public List<Territory> adjacent;

        public Territory(string name)
        {
            this.name = name;
            adjacent = new List<Territory>();
        }

        public void addAdjacent(Territory t)
        {
            adjacent.Add(t);
            t.addReverseAdjacent(this);
        }

        public void addReverseAdjacent(Territory t)
        {
            adjacent.Add(t);
        }

        public object Clone()
        {
            Territory ret = new Territory(name);
            ret.armies = armies;
            ret.owner = owner;
            ret.adjacent = adjacent;
            return ret;
        }
    }
}
