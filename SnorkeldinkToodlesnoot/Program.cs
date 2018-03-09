using System;
using System.IO;
using LightRidersBot.Bot;
using SnorkeldinkToodlesnoot.Bot;

namespace SnorkeldinkToodlesnoot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput(512)));
            BotParser parser = new BotParser(new AwesomeBot());
            parser.Run();
        }
    }
}
