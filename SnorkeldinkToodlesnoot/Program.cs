using LightRidersBot.Bot;

namespace LightRidersBot
{
    class Program
    {
        static void Main(string[] args)
        {
            BotParser parser = new BotParser(new LightBot());
            parser.Run();
        }
    }
}
