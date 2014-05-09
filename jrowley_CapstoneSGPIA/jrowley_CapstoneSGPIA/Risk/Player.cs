using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace jrowley_CapstoneSGPIA.Risk
{
    public abstract class Player : ICloneable
    {
        public string name;
        public int position;
        public int reinforcements;
        public List<Territory> territories;
        public List<Card> cards;
        public Color color;
        public string colorName;
        public Manager manager;

        public Player(string name)
        {
            this.name = name;
            territories = new List<Territory>();
            cards = new List<Card>();
        }

        public Player() { }

        public abstract void TradeCards(Board board);

        public abstract void PlaceReinforcement(Board board);

        public abstract void GetReinforcements(Board board);

        public abstract void ReinforcementPhase(Board board);

        public abstract bool AttackPhase(Board board, List<Player> players, List<Player> dead);

        public abstract void FortifyPhase(Board board);

        public abstract void TakeOwnership(Player player, Territory territory);

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
