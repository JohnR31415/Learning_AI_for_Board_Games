using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class Manager
    {
        public Board board;
        public List<Player> players;
        public List<Player> dead;
        public int current;
        public int turns;
        public const int MIN = 0, MAX = 6;
        public Random random;
        public Calculator calculator;
        public List<TerritoryToken> tokens;
        public List<TerritoryToken> shuffledTokens;
        public MainWindow mw;
        public AlphaManager am;
        public string PROJECT_PATH = Path.GetDirectoryName(
                             Path.GetDirectoryName(
                             System.IO.Path.GetDirectoryName(
                             System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase))) +
                             "\\Risk";

        public Manager()
        {
            board = new Board();
            players = new List<Player>();
            dead = new List<Player>();
            random = new Random();
            shuffledTokens = new List<TerritoryToken>();
            am = new AlphaManager();
        }

        public void vSetup()
        {
            int numPlayers = -1;
            int numHumans = -1;
            int numComputers = -1;
            int newMIN = 0;
            int numTroops = 40;
            string input;
            string name;

            //Adds the human players
            while (numHumans > MAX || numHumans < MIN)
            {
                //Console.WriteLine("How many human players?\n{0}-{1}", MIN, MAX);
                input = Console.ReadLine();
                numHumans = int.Parse(input);
            }

            //Sets the new minimum
            while ((newMIN + numHumans) < 2)
                newMIN++;

            //Adds the computer players
            while (numComputers > MAX - numHumans || numComputers < newMIN)
            {
                //Console.WriteLine("How many computer players?\n{0}-{1}", newMIN, MAX - numHumans);
                input = Console.ReadLine();
                numComputers = int.Parse(input);
            }

            //Determines total players
            numPlayers = numHumans + numComputers;

            //Asks for human names
            for (int i = 0; i < numHumans; i++)
            {
                //Console.WriteLine("Enter human player {0}'s name.", i + 1);
                name = Console.ReadLine();
                CreatePlayer(name);
            }

            //Asks for computer names
            for (int i = 0; i < numComputers; i++)
            {
                //Console.WriteLine("Enter computer player {0}'s name.", i + 1);
                name = Console.ReadLine();
                CreateLearningComputer(name);
            }

            //Determines how many troops to give each player
            for (int i = 2; i < numPlayers; i++)
            {
                numTroops -= 5;
            }

            //Gives each player reinforcements
            foreach (Player player in players)
            {
                player.reinforcements = numTroops;
                player.manager = this;
            }

            for (int i = 0; i < numPlayers; i++)
            {
                switch (i + 1)
                {
                    case 1:
                        players[i].color = Colors.Red;
                        break;
                    case 2:
                        players[i].color = Colors.Yellow;
                        break;
                    case 3:
                        players[i].color = Colors.Green;
                        break;
                    case 4:
                        players[i].color = Colors.Blue;
                        break;
                    case 5:
                        players[i].color = Colors.Purple;
                        break;
                    default:
                        players[i].color = Colors.Brown;
                        break;
                }
            }
        }

        public void fSetup()
        {
            int numPlayers = -1;
            int numHumans = -1;
            int numComputers = -1;
            int numTroops = 40;

            //Adds the human players
            numHumans = 0;

            //Adds the computer players
            numComputers = 6;

            //Determines total players
            numPlayers = numHumans + numComputers;

            //Asks for human names
            for (int i = 0; i < numHumans; i++)
            {
                CreatePlayer(i+"");
            }

            //Asks for computer names
            for (int i = 0; i < 3; i++)
            {
                CreateLearningComputer(i+"");
            }
            for (int i = 3; i < numComputers; i++)
            {
                CreateComputer(i + "");
            }

            //Determines how many troops to give each player
            for (int i = 2; i < numPlayers; i++)
            {
                numTroops -= 5;
            }

            //Gives each player reinforcements
            foreach (Player player in players)
            {
                player.reinforcements = numTroops;
                player.manager = this;
            }

            for (int i = 0; i < numPlayers; i++)
            {
                switch (i + 1)
                {
                    case 1:
                        players[i].color = Colors.Red;
                        players[i].colorName = "Red";
                        break;
                    case 2:
                        players[i].color = Colors.Yellow;
                        players[i].colorName = "Yellow";
                        break;
                    case 3:
                        players[i].color = Colors.Green;
                        players[i].colorName = "Green";
                        break;
                    case 4:
                        players[i].color = Colors.Blue;
                        players[i].colorName = "Blue";
                        break;
                    case 5:
                        players[i].color = Colors.Purple;
                        players[i].colorName = "Purple";
                        break;
                    default:
                        players[i].color = Colors.Gray;
                        players[i].colorName = "Gray";
                        break;
                }
            }
        }

        public void HvsLAISetup()
        {
            int numPlayers = -1;
            int numHumans = -1;
            int numComputers = -1;
            int numTroops = 40;

            //Adds the human players
            numHumans = 1;

            //Adds the computer players
            numComputers = 1;

            //Determines total players
            numPlayers = numHumans + numComputers;

            //Asks for human names
            for (int i = 0; i < numHumans; i++)
            {
                CreatePlayer("Player");
            }

            //Asks for computer names
            for (int i = 0; i < numComputers; i++)
            {
                CreateLearningComputer("" + i);
            }

            //Determines how many troops to give each player
            for (int i = 2; i < numPlayers; i++)
            {
                numTroops -= 5;
            }

            //Gives each player reinforcements
            foreach (Player player in players)
            {
                player.reinforcements = numTroops;
                player.manager = this;
            }

            for (int i = 0; i < numPlayers; i++)
            {
                switch (i + 1)
                {
                    case 1:
                        players[i].color = Colors.Red;
                        players[i].colorName = "Red";
                        break;
                    case 2:
                        players[i].color = Colors.Yellow;
                        players[i].colorName = "Yellow";
                        break;
                    case 3:
                        players[i].color = Colors.Green;
                        players[i].colorName = "Green";
                        break;
                    case 4:
                        players[i].color = Colors.Blue;
                        players[i].colorName = "Blue";
                        break;
                    case 5:
                        players[i].color = Colors.Purple;
                        players[i].colorName = "Purple";
                        break;
                    default:
                        players[i].color = Colors.Gray;
                        players[i].colorName = "Gray";
                        break;
                }
            }
        }



        public void vDistribute()
        {
            //Shuffles the territories and distributes them
            List<Territory> shuffledTerritories = board.territories.OrderBy(item => random.Next()).ToList();
            while (shuffledTerritories.Count > 0)
            {
                foreach (Player player in players)
                {
                    if (shuffledTerritories.Count == 0)
                        break;
                    player.territories.Add(shuffledTerritories.First());
                    shuffledTerritories.First().owner = player;
                    shuffledTerritories.First().armies++;
                    player.reinforcements--;
                    shuffledTerritories.Remove(shuffledTerritories.First());
                }
            }

            vUpdateMap();

            //Orders the players randomly
            List<Player> playerHolder = new List<Player>();
            int position = 1;
            while (players.Count > 0)
            {
                int rand = random.Next(players.Count);
                players[rand].position = position;
                position++;
                playerHolder.Add(players[rand]);
                players.Remove(players[rand]);
            }

            //Orders the players by position
            players = playerHolder.OrderBy(x => x.position).ToList();

            foreach (Player player in players)
            {
                if (player.GetType() == typeof(AI))
                    (player as AI).setupAlpha();
            }

            //Places one army on one territory for each of the players until no reinforcements remain
            while (players.Last().reinforcements > 0)
            {
                foreach (Player player in players)
                {
                    player.PlaceReinforcement(board);
                    vUpdateMap();
                }
            }
        }

        public void vUpdateMap()
        {
            //commented out for speed
            for (int i = 0; i < tokens.Count; i++)
            {
                Territory equiv = board.territories.Where(x => x.name.Replace(" ", string.Empty) == tokens[i].name.Name).First();
                tokens[i].ellipse.Fill = new SolidColorBrush(equiv.owner.color);
                tokens[i].armies.Content = equiv.armies;
                ExtensionMethods.Refresh(tokens[i].ellipse);
                ExtensionMethods.Refresh(tokens[i].armies);
            }
        }

        public void StartGame()
        {
            current = players.Count - 1;
            bool onePlayerLeft = false;
            while (!onePlayerLeft)
            {
                int playerCounter = 0;
                foreach (Player player in players)
                {
                    if (player != null)
                        playerCounter++;
                }
                if (playerCounter == 1)
                {
                    onePlayerLeft = true;
                    break;
                }
                current = current >= players.Count - 1 ? 0 : current + 1;
                TakeTurn();
            }
            Console.WriteLine("Game Over");
            Console.WriteLine("{0} turns.", turns);
            storeGameLength(turns);
            Player winner = null;
            foreach(Player p in players)
                if(p != null)
                    winner = p;

            List<Player> allPlayers = new List<Player>();
            allPlayers.Add(winner);

            foreach (Player p in dead)
                allPlayers.Add(p);

            int compCount = 0;
            foreach (Player p in allPlayers)
            {
                if(p.GetType() == typeof(AI))
                    compCount++;
            }
            if (compCount > 1)
            {
                AI b1 = null;
                AI b2 = null;

                if (winner.GetType() == typeof(AI))
                {
                    b1 = (AI)winner;
                    while (b2 == null)
                    {
                        if (dead.Last().GetType() == typeof(AI))
                            b2 = (AI)dead.Last();
                        else
                            dead.RemoveAt(dead.Count - 1);
                    }
                    
                }
                List<float> bredAlpha = am.breedAlphas(int.Parse(b1.name), int.Parse(b2.name));
                am.replaceAllFiles(bredAlpha);
            }
        }

        public void TakeTurn()
        {
            if (players[current] != null)
            {
                players[current].TradeCards(board);
                players[current].GetReinforcements(board);
                players[current].ReinforcementPhase(board);
                vUpdateMap();
                do { vUpdateMap(); }
                while (players[current].AttackPhase(board, players, dead));
                players[current].FortifyPhase(board);
                vUpdateMap();
                turns++;
            }
        }

        public void CreatePlayer(string name)
        {
            players.Add(new Human(name));
        }

        public void CreateComputer(string name)
        {
            players.Add(new AI(name, false, am));
        }

        public void CreateLearningComputer(string name)
        {
            players.Add(new AI(name, true, am));
        }

        public void createFile()
        {
            
        }

        public void storeGameLength(int turnValue)
        {
            string filePath = new Uri(PROJECT_PATH + "\\GameLength.csv").LocalPath;
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.Write(turnValue);
                writer.Write('\n');
            }
        }
    }
}
