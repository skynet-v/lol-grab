using System;
using System.Linq;
using System.Threading;
using LOLApiAdapter.CommonDefinitions.Enums;

namespace LoLApiParser
{
    public static class Program
    {
        private static readonly LoLApiParserNode[] Nodes = {
            new LoLApiParserNode(LolApiServerRegion.RU),
            new LoLApiParserNode(LolApiServerRegion.EUN1),
            new LoLApiParserNode(LolApiServerRegion.EUW1),
            new LoLApiParserNode(LolApiServerRegion.TR1),

        };

        public static void Main(string[] args)
        {
            foreach (var loLApiParserNode in Nodes)
            {
                try
                {
                    loLApiParserNode.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            const string exitCommand = "exit";
            Console.Write(@">> ");
            var line = Console.ReadLine();
            do
            {
                Console.Write(@">> ");
                line = Console.ReadLine();

                switch (line)
                {
                    case "exit":
                            Finish();
                        break;

                    default: continue;
                }

            } while (Nodes.All(x => x.MatchParserIsWorking ||
                                    x.MatchResultsParserIsWorking) ||
                                    line != null && !line.Equals(exitCommand));
        }

        private static void Finish()
        {
            foreach (var loLApiParserNode in Nodes)
                loLApiParserNode.Stop();
        }
    }
}
