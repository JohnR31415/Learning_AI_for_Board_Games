using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class Die
    {
        public Random random;

        public Die()
        {
            random = new Random();
        }
        public int rollDie()
        {
            return random.Next(6) + 1;
        }

        public List<int> rollMultipleDice(int numOfDice)
        {
            List<int> rolls = new List<int>();
            for (int i = 0; i < numOfDice; i++)
            {
                rolls.Add(rollDie());
            }
            return rolls;
        }
    }
}
