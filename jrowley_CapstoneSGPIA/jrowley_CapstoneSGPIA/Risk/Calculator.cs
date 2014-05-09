using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class Calculator
    {
        public List<float> alpha;
        public Calculator()
        {
            alpha = new List<float>();
        }

        public Territory bestReinforce(Board board, Player player)
        {
            float best = 0;
            float territoryValue = 0;
            List<Territory> bestTerritories = new List<Territory>();
            Territory bestTerritory = null;
            Territory biggest = null;
            Board cloneBoard = (Board)board.Clone();
            Player clonePlayer = (Player)player.Clone();

            foreach (Territory t in clonePlayer.territories)
            {
                foreach (Territory adj in t.adjacent)
                {
                    if (adj.owner.name != clonePlayer.name)
                    {
                        if (biggest == null || adj.armies > biggest.armies)
                            biggest = adj;
                        if (adj.armies < t.armies)
                            territoryValue += 1 + alpha[0];
                        territoryValue += 1 + alpha[1];
                        Continent adjParent = getParentContinent(board, adj);
                        int ownedTerInCont = continentTerritoriesOwned(adjParent, t.owner);
                        if ((ownedTerInCont + 1) == adjParent.territories.Count || (ownedTerInCont + 2) == adjParent.territories.Count)
                            territoryValue += 5 + alpha[2];
                        if (adj.owner.territories.Count <= 4)
                            territoryValue += 3 + alpha[3];
                    }
                }
                if (biggest == null)
                    territoryValue += (0 - t.armies) + alpha[4];
                else
                    territoryValue += (biggest.armies - t.armies) + alpha[5];
                //Adds the corrisponding alpha value of the territories parent continent
                Continent parent = getParentContinent(board, t);
                switch (parent.name)
                {
                    case "North America":
                        territoryValue += alpha[10];
                        break;
                    case "South America":
                        territoryValue += alpha[11];
                        break;
                    case "Europe":
                        territoryValue += alpha[12];
                        break;
                    case "Africa":
                        territoryValue += alpha[13];
                        break;
                    case "Asia":
                        territoryValue += alpha[14];
                        break;
                    case "Australia":
                        territoryValue += alpha[15];
                        break;
                }
                //Add value if the reinforcement is next to the needed territory to complete a continent
                //Add value if the reinforcement is next to a players last reinforcement
                if (territoryValue > best)
                {
                    best = territoryValue;
                    bestTerritories.Clear();
                    bestTerritories.Add(t);
                }
                else if (territoryValue == best)
                {
                    bestTerritories.Add(t);
                }
                territoryValue = 0;
                biggest = null;
            }
            if (bestTerritories.Count > 1)
            {
                Random rand = new Random();
                int select = rand.Next(bestTerritories.Count);
                bestTerritory = bestTerritories[select];
            }
            else if (bestTerritories.Count < 1)
            {
                bestTerritories.Add(null);
                bestTerritories.Add(null);
            }
            else
            {
                bestTerritory = bestTerritories[0];
            }
            return bestTerritory;
        }

        public List<Territory> bestAttack(Board board, Player player)
        {
            float best = 0;
            float attackValue = 0;
            List<Territory> bestAttack = new List<Territory>();
            List<Territory> bestAttacks = new List<Territory>();
            Territory biggest = null;
            Board cloneBoard = (Board)board.Clone();
            Player clonePlayer = (Player)player.Clone();

            foreach (Territory t in clonePlayer.territories)
            {
                foreach (Territory adj in t.adjacent)
                {
                    if ((biggest == null || adj.armies > biggest.armies) && adj.owner.name != player.name)
                        biggest = adj;
                }
                foreach (Territory adj in t.adjacent)
                {
                    if (adj.owner.name != clonePlayer.name && t.armies > 1)
                    {
                        attackValue = (t.armies - adj.armies) + alpha[6];
                        if (playerOwnsContinent(getParentContinent(board, adj), adj.owner))
                        {
                            attackValue += 5 + alpha[7];
                        }
                        int contTerrOwned = continentTerritoriesOwned(getParentContinent(board, adj), t.owner);
                        if ((contTerrOwned + 1) == getParentContinent(board, adj).territories.Count || (contTerrOwned + 2) == getParentContinent(board, adj).territories.Count)
                        {
                            attackValue += 5 + alpha[8];
                        }

                        attackValue += ((float)contTerrOwned / (float)getParentContinent(board, adj).territories.Count) + alpha[9];

                        Continent parent = getParentContinent(board, adj);
                        switch (parent.name)
                        {
                            case "North America":
                                attackValue += alpha[10];
                                break;
                            case "South America":
                                attackValue += alpha[11];
                                break;
                            case "Europe":
                                attackValue += alpha[12];
                                break;
                            case "Africa":
                                attackValue += alpha[13];
                                break;
                            case "Asia":
                                attackValue += alpha[14];
                                break;
                            case "Australia":
                                attackValue += alpha[15];
                                break;
                        }

                        //if adj.owner contains all territories in that continent,
                        //add a large value to attack value.
                        //add continent ownership percentage
                        //if t.owner is missing only one from that continent and adj is it,
                        //add a large value to the attack value.
                        if (attackValue > best)
                        {
                            best = attackValue;
                            bestAttacks.Clear();
                            bestAttacks.Add(t);
                            bestAttacks.Add(adj);
                        }
                        else if (attackValue == best)
                        {
                            bestAttacks.Add(t);
                            bestAttacks.Add(adj);
                        }
                    }
                }
                biggest = null;
            }
            if (bestAttacks.Count > 2)
            {
                Random rand = new Random();
                int select = rand.Next((bestAttacks.Count / 2));
                bestAttack.Add(bestAttacks[select * 2]);
                bestAttack.Add(bestAttacks[(select * 2) + 1]);
            }
            else if (best < 1)
            {
                bestAttack.Clear();
                bestAttack.Add(null);
                bestAttack.Add(null);
            }
            else
            {
                bestAttack.Add(bestAttacks[0]);
                bestAttack.Add(bestAttacks[1]);
            }
            return bestAttack;
        }

        public List<Territory> bestMove(Board board, Player player)
        {
            List<Territory> moveTerritories = new List<Territory>();
            Board cloneBoard = (Board)board.Clone();
            Player clonePlayer = (Player)player.Clone();
            List<Territory> isolatedTerritories = new List<Territory>();
            List<Territory> emptyIsolated = new List<Territory>();
            List<Territory> adjToIsolatedTerritories = new List<Territory>();
            bool isIsolated = true;

            List<Territory> bestTerritories = new List<Territory>();
            Territory bestTerritory = null;
            Territory bestIsolated = null;

            foreach (Territory t in clonePlayer.territories)
            {
                foreach (Territory adj in t.adjacent)
                {
                    if (adj.owner.name != clonePlayer.name)
                    {
                        isIsolated = false;
                    }
                }
                if (isIsolated && t.armies > 1)
                {
                    isolatedTerritories.Add(t);
                }
                else if (isIsolated && t.armies == 1)
                {
                    emptyIsolated.Add(t);
                }
                isIsolated = true;
            }

            if (isolatedTerritories.Count < 1)
            {
                moveTerritories.Add(null);
                moveTerritories.Add(null);
                return moveTerritories;
            }
            else
            {
                bestIsolated = null;
                foreach (Territory t in isolatedTerritories)
                {
                    if (bestIsolated == null || t.armies > bestIsolated.armies)
                        bestIsolated = t;
                    foreach (Territory adj in t.adjacent)
                    {
                        if (adj.owner.name == clonePlayer.name &&
                            !adjToIsolatedTerritories.Exists(x => x.name == adj.name) &&
                            !isolatedTerritories.Exists(x => x.name == adj.name))
                            adjToIsolatedTerritories.Add(adj);
                    }

                }
                moveTerritories.Clear();
                moveTerritories.Add(bestIsolated);

                foreach (Territory t in bestIsolated.adjacent)
                {
                    foreach (Territory adj in adjToIsolatedTerritories)
                    {
                        if (t.name == adj.name && !(emptyIsolated.Contains(adj)) && (bestTerritory == null || adj.armies < bestTerritory.armies))
                        {
                            bestTerritory = adj;
                        }
                    }
                }
                moveTerritories.Add(bestTerritory);
            }
            return moveTerritories;
        }

        public Territory biggestAdjacent(Territory territory, Player player)
        {
            Territory biggest = null;
            foreach (Territory adj in territory.adjacent)
                if (biggest == null || adj.armies > biggest.armies)
                    biggest = adj;
            return biggest;
        }

        public Continent getParentContinent(Board board, Territory territory)
        {
            Continent parent = null;
            foreach (Continent c in board.continents)
            {
                foreach (Territory t in c.territories)
                {
                    if (t.name == territory.name)
                    {
                        parent = c;
                        break;
                    }
                }
            }
            return parent;
        }

        public bool playerOwnsContinent(Continent continent, Player player)
        {
            int continentTerritoryCount = continent.territories.Count;
            int continentTerritoriesOwned = 0;
            foreach (Territory t in continent.territories)
            {
                if(player.territories.Contains(t))
                    continentTerritoriesOwned ++;
            }
            if (continentTerritoriesOwned == continentTerritoryCount)
                return true;
            return false;
        }
        public int continentTerritoriesOwned(Continent continent, Player player)
        {
            int territoriesOwned = 0;
            foreach (Territory t in continent.territories)
            {
                if(player.territories.Contains(t))
                    territoriesOwned++;
            }
            return territoriesOwned;
        }
    }
}
