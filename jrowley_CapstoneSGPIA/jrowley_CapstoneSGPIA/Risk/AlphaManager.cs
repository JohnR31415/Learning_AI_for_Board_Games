using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jrowley_CapstoneSGPIA.Risk
{
    public class AlphaManager
    {
        public static Random rand;
        public const int ALPHACOUNT = 16;
        public string PROJECT_PATH = Path.GetDirectoryName(
                             Path.GetDirectoryName(
                             System.IO.Path.GetDirectoryName(
                             System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase))) +
                             "\\Risk\\AlphaValues";
        public AlphaManager()
        {
            rand = new Random();
        }

        public List<float> getAlphaValues(int counter)
        {
            List<float> alpha = new List<float>();
            string counterPath = new Uri(PROJECT_PATH + "\\AI" + counter + ".csv").LocalPath;
            if (File.Exists(counterPath))
            {
                alpha = grabFile(counter, counterPath);
            }
            else
            {
                createFile(alpha, counter, counterPath);
            }
            return alpha;
        }

        public List<float> grabFile(int counter, string path)
        {
            List<float> alpha = new List<float>();
            string values = "";
            using (StreamReader reader = new StreamReader(path))
            {
                values = reader.ReadLine();

            }
            string[] parts = values.Split(',');
            foreach (string part in parts)
            {
                alpha.Add(float.Parse(part));
            }
            return alpha;
        }

        public void createFile(List<float> alpha, int counter, string path)
        {
            
            File.Create(path).Close();
            for (int i = 0; i < ALPHACOUNT; i++)
            {
                alpha.Add((float)rand.NextDouble() * 2);
                //System.Threading.Thread.Sleep(rand.Next(1,10));
            }
                
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                for (int i = 0; i < ALPHACOUNT - 1; i++)
                {
                    writer.Write(alpha[i] + ",");
                }
                writer.Write(alpha[alpha.Count - 1]);
                writer.Write('\n');
            }
        }

        public List<float> createRandomAlpha(string path)
        {

            List<float> alpha = new List<float>();
            for (int i = 0; i < ALPHACOUNT; i++)
                alpha.Add((float)rand.NextDouble() * 2);
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                for (int i = 0; i < ALPHACOUNT - 1; i++)
                {
                    writer.Write(alpha[i] + ",");
                }
                writer.Write(alpha[alpha.Count - 1]);
            }
            return alpha;
        }

        public void replaceFile(List<float> alpha, int counter, string path)
        {
            System.IO.File.WriteAllText(path, string.Empty);
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                
                for (int i = 0; i < ALPHACOUNT - 1; i++)
                {
                    writer.Write(alpha[i] + ",");
                }
                writer.Write(alpha[alpha.Count - 1]);
            }
        }

        public void storeData(List<float> alpha, int counter, string path)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {

                for (int i = 0; i < ALPHACOUNT - 1; i++)
                {
                    writer.Write(alpha[i] + ",");
                }
                writer.Write(alpha[alpha.Count - 1]);
                writer.Write('\n');
            }
        }

        public List<float> breedAlphas(int a1, int a2)
        {
            List<float> alpha1 = getAlphaValues(a1);
            List<float> alpha2 = getAlphaValues(a2);
            List<float> alphaChild = new List<float>();

            for (int i = 0; i < alpha1.Count; i++)
            {
                alphaChild.Add((alpha1[i] + alpha2[i]) / 2);
            }

            return alphaChild;
        }

        public List<float> mutateAlpha(List<float> alpha, int counter)
        {
            int mutations = rand.Next(1, alpha.Count/4);
            for (int i = 0; i < mutations; i++)
            {
                int location = rand.Next(0, alpha.Count - 1);
                alpha[location] = -1 + (float)rand.NextDouble() * 2;
            }
            return alpha;
        }

        public void replaceAllFiles(List<float> alpha)
        {
            List<float> cloneAlpha = new List<float>();
            foreach (float f in alpha)
                cloneAlpha.Add(f);
            for (int i = 0; i < 3; i++)
            {
                string counterPath = new Uri(PROJECT_PATH + "\\AI" + i + ".csv").LocalPath;
                string savePath = new Uri(PROJECT_PATH + "\\HistoryAI" + i + ".csv").LocalPath;
                if (File.Exists(counterPath))
                {
                    if (!File.Exists(savePath))
                    {
                        createFile(alpha, i, savePath);
                    }
                    else
                    {
                        storeData(alpha, i, savePath);
                    }
                    replaceFile(mutateAlpha(cloneAlpha, i), i, counterPath);
                    cloneAlpha.Clear();
                    foreach (float f in alpha)
                        cloneAlpha.Add(f);
                }
            }
            for (int i = 3; i < 6; i++)
            {
                string counterPath = new Uri(PROJECT_PATH + "\\AI" + i + ".csv").LocalPath;
                if (File.Exists(counterPath))
                {
                    File.Delete(counterPath);
                }
            }
        }
    }
}
