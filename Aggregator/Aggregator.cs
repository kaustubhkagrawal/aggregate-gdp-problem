using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aggregator
{
    public class GdpPopulation
    {

        private static string path = $"..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }data{ Path.DirectorySeparatorChar }datafile.csv";
        private static string dbPath = $"..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }data{ Path.DirectorySeparatorChar }db.json";
        private static string outputPath = $"..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }data{ Path.DirectorySeparatorChar }output.json";


        private static Dictionary<string, string> DbClean()
        {

            var cleanedData = new Dictionary<string, string>();

            if (File.Exists(dbPath))
            {
                JToken o;
                using (StreamReader file = File.OpenText(dbPath))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    o = JToken.ReadFrom(reader);
                }

                foreach (var obj in o)
                {
                    cleanedData[(string)obj["country"]] = (string)obj["continent"];
                }
            } else
            {
                // Throw Exception
            }

            return cleanedData;
        }

        public static void Aggregate()
        {
            /*List<string> dataFirstSplit = new List<string>();*/
            var dataSecondSplit = new List<String[]>();
            string separator = ",";

            /*String[] dataFirstSplit = */


            
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Datafile.csv not found");
            }
            else
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s;
                    string[] columns;

                    var output = new Dictionary<string, Dictionary<string, decimal>>();

                    if ((s = sr.ReadLine()) != null)
                    {
                        s = s.Replace("\"", "");
                        columns = (s.Split(separator));
                    }

                    while ((s = sr.ReadLine()) != null)
                    {
                        s = s.Replace("\"","");
                        dataSecondSplit.Add(s.Split(separator));
                    }
                    /*var dataSecondSplit = dataFirstSplit.Skip(1);*/
                    string continent = "";
                    var db = DbClean();

                    foreach (var country in dataSecondSplit)
                    {

                        /*Console.WriteLine(country[columns[0]]);*/
                        if(db.ContainsKey(country[0]))
                        {
                            continent = db[country[0]];

                            if (!output.ContainsKey(continent))
                            {
                                var localDictionary = new Dictionary<string, decimal>();
                                localDictionary.Add("GDP_2012", 0);
                                localDictionary.Add("POPULATION_2012", 0);
                                output.Add(continent, localDictionary);
                            }
                            output[continent]["GDP_2012"] += Decimal.Parse(country[7]);
                            output[continent]["POPULATION_2012"] += Decimal.Parse(country[4]);
                            
                        }
                        
                    }

                    File.WriteAllText(outputPath, JsonConvert.SerializeObject(output));
                }
            }



            


            /*foreach (var items in dataSecondSplit)
            {
                foreach(var item in items)
                {
                    Console.WriteLine(item);
                }
            }*/

            /*Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);*/
        }




        static void Main(string[] args)
        {
            Aggregate();
        }
    }
}
