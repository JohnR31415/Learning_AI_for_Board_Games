using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class Board : ICloneable
    {
        public List<Continent> continents;
        public List<Territory> territories;
        public List<TerritoryToken> tokens;

        public Board()
        {
            continents = new List<Continent>();
            territories = new List<Territory>();
            createContinents();
            createTerritories();
            setAdjacency();
            addTerritories();
        }

        public void createContinents()
        {
            continents.Add(new Continent("North America", 5));
            continents.Add(new Continent("South America", 2));
            continents.Add(new Continent("Europe", 5));
            continents.Add(new Continent("Africa", 3));
            continents.Add(new Continent("Asia", 7));
            continents.Add(new Continent("Australia", 2));
        }
        public void createTerritories()
        {
            continents[0].addTerritory(new Territory("Alaska"));

            continents[0].addTerritory(new Territory("Alberta"));
            continents[0].addTerritory(new Territory("Central America"));
            continents[0].addTerritory(new Territory("Eastern United States"));
            continents[0].addTerritory(new Territory("Greenland"));
            continents[0].addTerritory(new Territory("Northwest Territory"));
            continents[0].addTerritory(new Territory("Ontario"));
            continents[0].addTerritory(new Territory("Quebec"));
            continents[0].addTerritory(new Territory("Western United States"));

            continents[1].addTerritory(new Territory("Argentina"));
            continents[1].addTerritory(new Territory("Brazil"));
            continents[1].addTerritory(new Territory("Peru"));
            continents[1].addTerritory(new Territory("Venezuela"));

            continents[2].addTerritory(new Territory("Great Britain"));
            continents[2].addTerritory(new Territory("Iceland"));
            continents[2].addTerritory(new Territory("Northern Europe"));
            continents[2].addTerritory(new Territory("Scandinavia"));
            continents[2].addTerritory(new Territory("Southern Europe"));
            continents[2].addTerritory(new Territory("Ukraine"));
            continents[2].addTerritory(new Territory("Western Europe"));

            continents[3].addTerritory(new Territory("Congo"));
            continents[3].addTerritory(new Territory("East Africa"));
            continents[3].addTerritory(new Territory("Egypt"));
            continents[3].addTerritory(new Territory("Madagascar"));
            continents[3].addTerritory(new Territory("North Africa"));
            continents[3].addTerritory(new Territory("South Africa"));

            continents[4].addTerritory(new Territory("Afghanistan"));
            continents[4].addTerritory(new Territory("China"));
            continents[4].addTerritory(new Territory("India"));
            continents[4].addTerritory(new Territory("Irkutsk"));
            continents[4].addTerritory(new Territory("Japan"));
            continents[4].addTerritory(new Territory("Kamchatka"));
            continents[4].addTerritory(new Territory("Middle East"));
            continents[4].addTerritory(new Territory("Mongolia"));
            continents[4].addTerritory(new Territory("Siam"));
            continents[4].addTerritory(new Territory("Siberia"));
            continents[4].addTerritory(new Territory("Ural"));
            continents[4].addTerritory(new Territory("Yakutsk"));

            continents[5].addTerritory(new Territory("Eastern Australia"));
            continents[5].addTerritory(new Territory("Indonesia"));
            continents[5].addTerritory(new Territory("New Guinea"));
            continents[5].addTerritory(new Territory("Western Australia"));
        }

        public void addTerritories()
        {
            foreach (Continent c in continents)
                foreach (Territory t in c.territories)
                    territories.Add(t);
        }

        public void setAdjacency()
        {
            List<Territory> allTerritories = new List<Territory>();
            foreach (Continent c in continents)
            {
                foreach (Territory t in c.territories)
                {
                    allTerritories.Add(t);
                }
            }
            //Alaska -> Alberta, Northwest Territory, Kamchatka
            allTerritories[0].addAdjacent(allTerritories[1]);
            allTerritories[0].addAdjacent(allTerritories[5]);
            allTerritories[0].addAdjacent(allTerritories[31]);

            //Alberta -> Northwest Territory, Ontario, Western US
            allTerritories[1].addAdjacent(allTerritories[5]);
            allTerritories[1].addAdjacent(allTerritories[6]);
            allTerritories[1].addAdjacent(allTerritories[8]);

            //Central America -> Eastern US, Western US, Venezuela
            allTerritories[2].addAdjacent(allTerritories[3]);
            allTerritories[2].addAdjacent(allTerritories[8]);
            allTerritories[2].addAdjacent(allTerritories[12]);

            //Eastern US -> Ontario, Quebec, Western US
            allTerritories[3].addAdjacent(allTerritories[6]);
            allTerritories[3].addAdjacent(allTerritories[7]);
            allTerritories[3].addAdjacent(allTerritories[8]);

            //Greenland -> Northwest Territory, Ontario, Quebec, Iceland
            allTerritories[4].addAdjacent(allTerritories[5]);
            allTerritories[4].addAdjacent(allTerritories[6]);
            allTerritories[4].addAdjacent(allTerritories[7]);
            allTerritories[4].addAdjacent(allTerritories[14]);

            //Northwest Territory -> Ontario
            allTerritories[5].addAdjacent(allTerritories[6]);

            //Ontario -> Quebec, Western US
            allTerritories[6].addAdjacent(allTerritories[7]);
            allTerritories[6].addAdjacent(allTerritories[8]);

            //Quebec -> All adjacency done

            //Western US -> All adjacency done

            //Argentina -> Brazil, Peru
            allTerritories[9].addAdjacent(allTerritories[10]);
            allTerritories[9].addAdjacent(allTerritories[11]);

            //Brazil -> Peru, Venezuela, North Africa
            allTerritories[10].addAdjacent(allTerritories[11]);
            allTerritories[10].addAdjacent(allTerritories[12]);
            allTerritories[10].addAdjacent(allTerritories[24]);

            //Peru -> Venezuela
            allTerritories[11].addAdjacent(allTerritories[12]);

            //Venezuela -> All adjacency done

            //Great Britain -> Iceland, Northern Europe, Scandinavia, Western Europe
            allTerritories[13].addAdjacent(allTerritories[14]);
            allTerritories[13].addAdjacent(allTerritories[15]);
            allTerritories[13].addAdjacent(allTerritories[16]);
            allTerritories[13].addAdjacent(allTerritories[19]);

            //Iceland -> Scandinavia
            allTerritories[14].addAdjacent(allTerritories[16]);

            //Northern Europe -> Scandinavia, Southern Europe, Ukraine, Western Europe
            allTerritories[15].addAdjacent(allTerritories[16]);
            allTerritories[15].addAdjacent(allTerritories[17]);
            allTerritories[15].addAdjacent(allTerritories[18]);
            allTerritories[15].addAdjacent(allTerritories[19]);

            //Scandinavia -> Ukraine
            allTerritories[16].addAdjacent(allTerritories[18]);

            //Southern Europe -> Ukraine, Western Europe, Egypt, North Africa, Middle East
            allTerritories[17].addAdjacent(allTerritories[18]);
            allTerritories[17].addAdjacent(allTerritories[19]);
            allTerritories[17].addAdjacent(allTerritories[22]);
            allTerritories[17].addAdjacent(allTerritories[24]);
            allTerritories[17].addAdjacent(allTerritories[32]);

            //Ukraine -> Afghanistan, Middle East, Ural
            allTerritories[18].addAdjacent(allTerritories[26]);
            allTerritories[18].addAdjacent(allTerritories[32]);
            allTerritories[18].addAdjacent(allTerritories[36]);

            //Western Europe -> North Africa
            allTerritories[19].addAdjacent(allTerritories[24]);

            //Congo -> East Africa, North Africa, South Africa
            allTerritories[20].addAdjacent(allTerritories[21]);
            allTerritories[20].addAdjacent(allTerritories[24]);
            allTerritories[20].addAdjacent(allTerritories[25]);

            //East Africa -> Egypt, Madagascar, North Africa, South Africa, Middle East
            allTerritories[21].addAdjacent(allTerritories[22]);
            allTerritories[21].addAdjacent(allTerritories[23]);
            allTerritories[21].addAdjacent(allTerritories[24]);
            allTerritories[21].addAdjacent(allTerritories[25]);
            allTerritories[21].addAdjacent(allTerritories[32]);

            //Egypt -> North Africa, Middle East
            allTerritories[22].addAdjacent(allTerritories[24]);
            allTerritories[22].addAdjacent(allTerritories[32]);

            //Madagascar -> South Africa
            allTerritories[23].addAdjacent(allTerritories[25]);

            //North Afriica -> All adjacency done

            //South Africa -> All adjacency done

            //Afghanistan -> China, India, Middle East, Ural
            allTerritories[26].addAdjacent(allTerritories[27]);
            allTerritories[26].addAdjacent(allTerritories[28]);
            allTerritories[26].addAdjacent(allTerritories[32]);
            allTerritories[26].addAdjacent(allTerritories[36]);

            //China -> India, Mongolia, Siam, Siberia, Ural
            allTerritories[27].addAdjacent(allTerritories[28]);
            allTerritories[27].addAdjacent(allTerritories[33]);
            allTerritories[27].addAdjacent(allTerritories[34]);
            allTerritories[27].addAdjacent(allTerritories[35]);
            allTerritories[27].addAdjacent(allTerritories[36]);

            //India -> Middle East, Siam
            allTerritories[28].addAdjacent(allTerritories[32]);
            allTerritories[28].addAdjacent(allTerritories[34]);

            //Irkutsk -> Kamchatka, Mongolia, Siberia, Yakutsk
            allTerritories[29].addAdjacent(allTerritories[31]);
            allTerritories[29].addAdjacent(allTerritories[33]);
            allTerritories[29].addAdjacent(allTerritories[35]);
            allTerritories[29].addAdjacent(allTerritories[37]);

            //Japan -> Kamchatka, Mongolia
            allTerritories[30].addAdjacent(allTerritories[31]);
            allTerritories[30].addAdjacent(allTerritories[33]);

            //Kamchatka -> Mongolia, Yakutsk
            allTerritories[31].addAdjacent(allTerritories[33]);
            allTerritories[31].addAdjacent(allTerritories[37]);

            //Middle East -> All adjacency done

            //Mongolia -> Siberia
            allTerritories[33].addAdjacent(allTerritories[35]);

            //Siam -> Indonesia
            allTerritories[34].addAdjacent(allTerritories[39]);

            //Siberia -> Ural, Yakutsk
            allTerritories[35].addAdjacent(allTerritories[36]);
            allTerritories[35].addAdjacent(allTerritories[37]);

            //Ural -> All adjacency done

            //Yakutsk -> All adjacency done

            //Eastern Australia -> New Guinea, Western Australia
            allTerritories[38].addAdjacent(allTerritories[40]);
            allTerritories[38].addAdjacent(allTerritories[41]);

            //Indonesia -> New Guinea, Western Australia
            allTerritories[39].addAdjacent(allTerritories[40]);
            allTerritories[39].addAdjacent(allTerritories[41]);

            //New Guinea -> Western Australia
            allTerritories[40].addAdjacent(allTerritories[41]);

            //Western Australia -> All adjacency done
            allTerritories[30].addAdjacent(allTerritories[31]);
        }

        public void addTokens()
        {
            foreach (Territory t in territories)
                tokens.Add(new TerritoryToken(new Ellipse(), new Label(), new Label()));
        }

        public object Clone()
        {
            Board ret = new Board();
            List<Continent> cloneContinents = new List<Continent>();
            foreach (Continent c in continents)
            {
                cloneContinents.Add((Continent)c.Clone());
            }
            List<Territory> cloneTerritories = new List<Territory>();
            foreach (Territory t in territories)
            {
                cloneTerritories.Add((Territory)t.Clone());
            }
            ret.continents = cloneContinents;
            ret.territories = cloneTerritories;
            return ret;
        }
    }
}
