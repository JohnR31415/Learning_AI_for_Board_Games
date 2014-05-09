using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class AI : Player
    {
        public Calculator calculator;
        public List<float> alpha;
        public AlphaManager am;
        public bool learning;
        public const int ALPHA_COUNT = 16;
        public AI(string name, bool learning, AlphaManager am)
            : base(name)
        {
            this.am = am;
            this.learning = learning;
            calculator = new Calculator();
        }

        public void setupAlpha()
        {
            if (!learning)
            {
                alpha = new List<float>();
                for (int i = 0; i < ALPHA_COUNT; i++)
                    alpha.Add(1.0f);
            }
            else
            {
                alpha = am.getAlphaValues(int.Parse(name));
            }
            calculator.alpha = alpha;

        }

        public override void TradeCards(Board board)
        {
            if (cards.Count > 2)
            {
                if (cards.Count > 5)
                {
                    //force to trade
                }
                //ask to trade
            }
            //tell not enough cards
        }

        public override void PlaceReinforcement(Board board)
        {
            Territory toPlace = calculator.bestReinforce(board, this);
            if (toPlace == null)
            {
                Random rand = new Random();
                int select = rand.Next(territories.Count);
                toPlace = territories[select];
            }
            else
            {
                toPlace = territories.Where(x => x.name == toPlace.name).First();
            }
            toPlace.armies++;
            reinforcements--;
            //Select Territory
        }

        public override void GetReinforcements(Board board)
        {
            int min = 3;
            int total = territories.Count / 3;
            reinforcements += min > total ? min : total;

            foreach (Continent c in board.continents)
            {
                bool containsAll = true;
                foreach (Territory t in c.territories)
                {
                    if (!(territories.Contains(t)))
                    {
                        containsAll = false;
                        break;
                    }
                }
                if (containsAll)
                {
                    reinforcements += c.value;
                }
            }
        }

        public override void ReinforcementPhase(Board board)
        {
            while (reinforcements > 0)
            {
                Territory toPlace = calculator.bestReinforce(board, this);
                if (toPlace == null)
                {
                    Random rand = new Random();
                    int select = rand.Next(territories.Count);
                    toPlace = territories[select];
                }
                else
                {
                    toPlace = territories.Where(x => x.name == toPlace.name).First();
                }
                toPlace.armies++;
                reinforcements--;
            }
            //While reinforcements are left
            //Select territory
            //Place armies
        }

        public override bool AttackPhase(Board board, List<Player> players, List<Player> dead)
        {
            //Make attack?
            //Perform attack
            Die die = new Die();
            int attackDice = 0;
            int defendDice = 0;
            List<Territory> attackTerritories = new List<Territory>();
            List<Territory> cloneTerritories = new List<Territory>();
            List<Territory> lastAttack = new List<Territory>();
            attackTerritories.Add(null);
            attackTerritories.Add(null);
            cloneTerritories.Add(null);
            cloneTerritories.Add(null);
            lastAttack.Add(null);
            lastAttack.Add(null);
            List<int> attackRolls = new List<int>();
            List<int> defendRolls = new List<int>();
            bool started = false;
            while (((attackTerritories[0] != null && attackTerritories[1] != null) || !started) && territories.Count != board.territories.Count)
            {
                //Console.WriteLine("{0} is thinking...", this.name);
                started = true;
                cloneTerritories.Clear();
                cloneTerritories = calculator.bestAttack(board, this);
                attackTerritories.Clear();
                if (cloneTerritories[0] == null && cloneTerritories[1] == null)
                    break;
                attackTerritories.Add(board.territories.Where(x => x.name == cloneTerritories[0].name).First());
                attackTerritories.Add(board.territories.Where(x => x.name == cloneTerritories[1].name).First());
                if (lastAttack[0] != null && lastAttack[1] != null)
                    if (attackTerritories[0].name == lastAttack[0].name &&
                       attackTerritories[1].name == lastAttack[1].name)
                        break;
                bool attackDone = false;

                while (!attackDone &&
                    attackTerritories[0].armies > 1 &&
                    attackTerritories[1].armies > 0 &&
                    territories.Count != board.territories.Count)
                {
                    lastAttack[0] = (Territory)attackTerritories[0].Clone();
                    lastAttack[1] = (Territory)attackTerritories[1].Clone();
                    if (attackTerritories[0].armies < attackTerritories[1].armies)
                        break;
                    //Console.WriteLine("Attacking {0} from {1}", attackTerritories[1].name, attackTerritories[0].name);
                    //Console.WriteLine("{0}: {1}\n{2}: {3}", attackTerritories[0].name, attackTerritories[0].armies, attackTerritories[1].name, attackTerritories[1].armies);
                    System.Threading.Thread.Sleep(0);
                    switch (attackTerritories[0].armies)
                    {
                        case 2:
                            attackDice = 1;
                            break;
                        case 3:
                            attackDice = 2;
                            break;
                        case 4:
                            attackDice = 3;
                            break;
                        default:
                            attackDice = 3;
                            break;
                    }

                    defendDice = attackTerritories[1].armies == 1 ? 1 : 2;

                    attackRolls = die.rollMultipleDice(attackDice).OrderByDescending(x => x).ToList();
                    defendRolls = die.rollMultipleDice(defendDice).OrderByDescending(x => x).ToList();

                    if (attackRolls[0] > defendRolls[0])
                    {
                        attackTerritories[1].armies--;
                    }
                    else
                    {
                        attackTerritories[0].armies--;
                    }
                    if (attackRolls.Count > 1 && defendRolls.Count > 1)
                    {
                        if (attackRolls[1] > defendRolls[1])
                        {
                            attackTerritories[1].armies--;
                        }
                        else
                        {
                            attackTerritories[0].armies--;
                        }

                    }
                    manager.vUpdateMap();
                    System.Threading.Thread.Sleep(0);
                    if (attackTerritories[0].armies < 2)
                        break;
                    else if (attackTerritories[1].armies < 1)
                    {
                        Player defeated = attackTerritories[1].owner;
                        //Console.WriteLine("{0} defeated {1} in {2} from {3}", this.name, defeated.name, attackTerritories[1].name, attackTerritories[0].name);
                        TakeOwnership(this, attackTerritories[1]);
                        attackTerritories[1].armies += attackRolls.Count;
                        attackTerritories[0].armies -= attackRolls.Count;

                        int halfAttack = attackTerritories[0].armies / 2;
                        if (halfAttack == 0)
                            halfAttack = 1;
                        if (attackTerritories[0].armies - 1 != 0)
                        {
                            attackTerritories[1].armies += halfAttack;
                            attackTerritories[0].armies -= halfAttack;
                        }

                        if (attackTerritories[0].armies == 0 ||
                            attackTerritories[1].armies == 0)
                            //Console.WriteLine("Something bad happened.");

                        attackDone = true;
                        manager.vUpdateMap();
                        System.Threading.Thread.Sleep(0);
                        if (defeated.territories.Count == 0)
                        {
                            foreach (Card card in defeated.cards)
                                cards.Add(card);
                            if (cards.Count > 4)
                                TradeCards(board);
                            //Console.WriteLine("{0} defeated!", defeated.name);
                            int toNull = players.IndexOf(defeated);
                            dead.Add((Player)players[toNull]);
                            players[toNull] = null;
                            System.Threading.Thread.Sleep(0);
                        }
                    }
                }


            }
            //Occupied?
            //Move armies
            //Player eliminated?
            //Take Cards
            //Trade if > 4
            return false;
        }

        public override void FortifyPhase(Board board)
        {
            List<Territory> moveTerritories = calculator.bestMove(board, this);
            if (moveTerritories[0] != null && moveTerritories[1] != null)
            {
                moveTerritories[1].armies += moveTerritories[0].armies - 1;
                moveTerritories[0].armies = 1;
            }
            else
            {
                //Console.WriteLine("Making no move");
            }

            //Choose territory
            //Move armies
        }

        public override void TakeOwnership(Player player, Territory territory)
        {
            territory.owner.territories.Remove(territory);
            territory.owner = player;
            player.territories.Add(territory);
            territory.armies = 0;
        }

        public override object Clone()
        {
            AI ret = new AI(name, learning, am);
            ret.position = position;
            ret.reinforcements = reinforcements;
            List<Territory> cloneTerritories = new List<Territory>();
            foreach (Territory t in territories)
            {
                cloneTerritories.Add((Territory)t.Clone());
            }
            List<Card> cloneCard = new List<Card>();
            foreach (Card c in cards)
            {
                cloneCard.Add((Card)c.Clone());
            }
            ret.territories = cloneTerritories;
            ret.cards = cloneCard;
            ret.alpha = alpha;
            return ret;
        }
    }
}
