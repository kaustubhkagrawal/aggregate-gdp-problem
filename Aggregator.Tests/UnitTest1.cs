using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;
using Aggregator;

namespace Aggregator.Tests
{
    public class UnitTest1
    {


        private static string expectedOutputFilePath = $"..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }data{ Path.DirectorySeparatorChar }expected-output.json";
        private static string actualOutputFilePath = $"..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }..{ Path.DirectorySeparatorChar }data{ Path.DirectorySeparatorChar }output.json";

        private static JToken ReadJsonFile(string path)
        {
            JToken o;
            if (File.Exists(path))
            {
                using (StreamReader file = File.OpenText(path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    o = JToken.ReadFrom(reader);
                    return o;
                }
            }
            else
            {
                return null;
            }

        }

        /*private string ReadJsonFile(string path)
        {
            JToken o;
            if (File.Exists(path))
            {
                string s;
                using (StreamReader sr = File.OpenText(path))
                {
                    if ((s = sr.ReadToEnd()) != null)
                    {
                        return s;
                    } else
                    {
                        return "";
                    }
                }
            }
            else
            {
                return null;
            }

        }*/



        [Fact]
        public static void IsOutputDeepEquals()
        {
            GdpPopulation.Aggregate();

            var expected = ReadJsonFile(expectedOutputFilePath);
            var actual = ReadJsonFile(actualOutputFilePath);

            Assert.True(JToken.DeepEquals(expected, actual));
            /*Assert.Equal(expected, actual);*/

        }

        [Fact]
        public static void IsActualOutputNull()
        {
            /*GdpPopulation.Aggregate();*/

            var expected = ReadJsonFile(expectedOutputFilePath);

            Assert.NotNull(expected);
        }

        [Theory]
        [InlineData("../../../../data/expected-output.json")]
        [InlineData("../../../../data/output.json")]
        [InlineData("../../../../data/datafile.csv")]
        [InlineData("../../../../data/db.json")]
        public static void CheckingForFileNotFoundException(string path)
        {
            if (!File.Exists(path))
            {
                Assert.Throws<FileNotFoundException>(() => IsOutputDeepEquals());
            } else
            {
                Assert.True(File.Exists(path));
            }
        }

    }
}
