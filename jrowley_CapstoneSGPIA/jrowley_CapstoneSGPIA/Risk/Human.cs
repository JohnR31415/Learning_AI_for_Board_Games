using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class Human : Player
    {
        public Human(string name)
            : base(name)
        {
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
            //Asks for which territory they want to place on.
            //Console.WriteLine("Choose a territory to place 1 army.");
            //Console.WriteLine("Remaining reinforcements: {0}", reinforcements);

            //Shows all territories
            for (int i = 0; i < territories.Count; i++)
            {
                //Console.WriteLine((i + 1) + ". {0}, armies: {1}", territories[i].name, territories[i].armies);
            }

            //Gets input and adds one army to that territory
            int input = int.Parse(Console.ReadLine());
            territories[(input - 1)].armies++;
            reinforcements--;

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
            //Repeats until all reinforcements are used
            while (reinforcements > 0)
            {
                //Asks for which territory they want to place on.
                //Console.WriteLine("Choose a territory to place armies on.");
                //Console.WriteLine("Remaining reinforcements: {0}", reinforcements);

                //Shows all territories
                for (int i = 0; i < territories.Count; i++)
                {
                    //Console.WriteLine((i + 1) + ". {0}, armies: {1}", territories[i].name, territories[i].armies);
                }

                //Gets input and grabs the appropriate territory
                int input = int.Parse(Console.ReadLine());
                Territory toPlaceOn = territories[(input - 1)];

                input = -1;
                //Asks how many armies to place on the territory
                while (input > reinforcements || input < 0)
                {
                    //Console.WriteLine("How many armies do you want to place on {0}?", toPlaceOn.name);
                    input = int.Parse(Console.ReadLine());
                }
                //Places the armies
                toPlaceOn.armies += input;
                reinforcements -= input;

            }
        }

        public override bool AttackPhase(Board board, List<Player> players, List<Player> dead)
        {
            int input = -1;
            List<Territory> canAttack;
            List<Territory> attackableTerritories;
            Territory attacker;
            Territory defender;
            int attackDice = 0;
            int defendDice = 0;
            string output;
            int attackerLoss = 0;
            int defenderLoss = 0;

            //Sees if the player wants to attack
            while (input > 2 || input < 0)
            {
                //Console.WriteLine("Do you want to attack?\n1. Yes\n2. No");
                input = int.Parse(Console.ReadLine());
            }

            if (input == 1)
            {
                //Displays all territories that the player can attack from
                canAttack = territories.Where(x => x.armies > 1).ToList();
                input = -1;
                while (input > canAttack.Count || input < 0)
                {
                    //Console.WriteLine("Choose a territory to attack from?");
                    for (int i = 0; i < canAttack.Count; i++)
                    {
                        //Console.WriteLine("{0}. {1}: {2}", i + 1, canAttack[i].name, canAttack[i].armies);
                    }
                    input = int.Parse(Console.ReadLine());
                }

                attacker = canAttack[(input - 1)];

                input = -1;

                //Displays all the territories that the player can attack from the selected territory
                attackableTerritories = attacker.adjacent.Where(x => x.owner != attacker.owner).ToList();
                while (input > attackableTerritories.Count || input < 0)
                {
                    //Console.WriteLine("Choose a territory to attack?");
                    for (int i = 0; i < attackableTerritories.Count; i++)
                    {
                        //Console.WriteLine("{0}. {1}: {2}", i + 1, attackableTerritories[i].name, attackableTerritories[i].armies);
                    }
                    input = int.Parse(Console.ReadLine());
                }

                defender = attackableTerritories[(input - 1)];

                input = -1;

                //The roll loop
                while (input != 2)
                {
                    //Sees if the player wants to roll or retreat
                    //Console.WriteLine("1. Roll 2. Retreat\n{0}: {1}\n{2}: {3}", attacker.name, attacker.armies, defender.name, defender.armies);
                    input = int.Parse(Console.ReadLine());

                    if (input == 2)
                    {
                        input = -1;
                        break;
                    }
                    else
                    {
                        //Resets the input
                        input = -1;

                        //Rolls the dice and sets dice counts to max
                        Die die = new Die();
                        switch (attacker.armies)
                        {
                            case 2:
                                attackDice = 1;
                                break;
                            case 3:
                                attackDice = 2;
                                break;
                            default:
                                attackDice = 3;
                                break;
                        }
                        defendDice = defender.armies == 1 ? 1 : 2;

                        List<int> attackRolls = die.rollMultipleDice(attackDice).OrderByDescending(x => x).ToList();
                        List<int> defendRolls = die.rollMultipleDice(defendDice).OrderByDescending(x => x).ToList();

                        //Displays the attackers rolls
                        output = "Attacker: ";
                        for (int i = 0; i < attackRolls.Count - 1; i++)
                        {
                            output += attackRolls[i] + ", ";
                        }
                        output += attackRolls[attackRolls.Count - 1];
                        //Console.WriteLine(output);

                        //Displays the defenders rolls
                        output = "Defender: ";
                        for (int i = 0; i < defendRolls.Count - 1; i++)
                        {
                            output += defendRolls[i] + ", ";
                        }
                        output += defendRolls[defendRolls.Count - 1];
                        //Console.WriteLine(output);

                        //Calculates losses
                        if (attackRolls[0] > defendRolls[0])
                        {
                            defender.armies--;
                            defenderLoss++;
                        }
                        else
                        {
                            attacker.armies--;
                            attackerLoss++;
                        }
                        if (attackRolls.Count > 1 && defendRolls.Count > 1)
                        {
                            if (attackRolls[1] > defendRolls[1])
                            {
                                defender.armies--;
                                defenderLoss++;
                            }
                            else
                            {
                                attacker.armies--;
                                attackerLoss++;
                            }
                        }

                        //Displays the results of the skirmish
                        //Console.WriteLine("Attacker lost {0} armies and Defender lost {1} armies.", attackerLoss, defenderLoss);
                        attackerLoss = 0;
                        defenderLoss = 0;
                        if (attacker.armies < 2)
                        {
                            //Console.WriteLine("Not enough armies to attack.");
                            break;
                        }
                        else if (defender.armies < 1)
                        {
                            //Displays that you have conquered the territory
                            //Console.WriteLine("You defeated {0} in {1}.", defender.owner.name, defender.name);
                            Player defeated = defender.owner;

                            //Takes ownership and transfers the attacking armies
                            TakeOwnership(this, defender);
                            defender.armies += attackRolls.Count;
                            attacker.armies -= attackRolls.Count;

                            input = -1;
                            //Asks for how many armies to move mover
                            while (input > attacker.armies - 1 || input < 0)
                            {
                                //Console.WriteLine("How many armies do you want to move over?");
                                //Console.WriteLine("Available armies: {0}", attacker.armies - 1);
                                input = int.Parse(Console.ReadLine());
                            }

                            //Moves the armies over
                            attacker.armies -= input;
                            defender.armies += input;

                            //Console.WriteLine("{0}: {1}\n{2}: {3}", attacker.name, attacker.armies, defender.name, defender.armies);

                            //Checks to see if the opponent was eliminated
                            if (defeated.territories.Count == 0)
                            {
                                foreach (Card card in defeated.cards)
                                    cards.Add(card);
                                if (cards.Count > 4)
                                    TradeCards(board);
                                //Console.WriteLine("{0} defeated!", defeated.name);
                                int toNull = players.IndexOf(defeated);
                                dead.Add((Player)players[toNull].Clone());
                                players[toNull] = null;
                            }
                            input = 2;
                        }
                    }
                }
                return true;
            }
            else
                return false;
        }

        public override void FortifyPhase(Board board)
        {
            Territory toMoveFrom;
            Territory toMoveTo;
            int numArmiesToMove = 0;
            int input = -1;

            while (input > 2 || input < 0)
            {
                //Console.WriteLine("Do you want to fortify a territory?\n1. Yes\n2. No");
                input = int.Parse(Console.ReadLine());
            }

            if (input == 1)
            {
                input = -1;
                while (input > territories.Count || input < 0)
                {
                    //Asks for which territory they want to move to
                    //Console.WriteLine("Choose a territory to move armies from.");

                    //Shows all territories
                    for (int i = 0; i < territories.Count; i++)
                    {
                        //Console.WriteLine("{0}. {1}, available armies: {2}", i + 1, territories[i].name, territories[i].armies);
                    }
                    input = int.Parse(Console.ReadLine());
                }


                //Gets input and grabs the appropriate territory
                toMoveFrom = territories[(input - 1)];

                input = -1;
                //Asks how many armies to move to the territory
                while (input > toMoveFrom.adjacent.Count || input < 0)
                {
                    //Console.WriteLine("Choose a territory to move the armies to.");
                    for (int i = 0; i < toMoveFrom.adjacent.Count; i++)
                    {
                        //Console.WriteLine("{0}. {1}, armies: {2}", i + 1, toMoveFrom.adjacent[i].name, toMoveFrom.adjacent[i].armies);
                    }
                    input = int.Parse(Console.ReadLine());
                }
                toMoveTo = toMoveFrom.adjacent[(input - 1)];

                input = -1;
                while (input > toMoveFrom.armies - 1 || input < 0)
                {
                    //Console.WriteLine("How many armies do you want to move over?\n{0}-{1}", 0, toMoveFrom.armies - 1);
                    input = int.Parse(Console.ReadLine());
                }
                numArmiesToMove = input;

                //moves the armies
                toMoveFrom.armies -= numArmiesToMove;
                toMoveTo.armies += numArmiesToMove;
            }
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
            Human ret = new Human(name);
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
            return ret;
        }
    }
}
