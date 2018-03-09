using System.Collections.Generic;

namespace SnorkeldinkToodlesnoot.Bot
{
    public class BotState
    {
        public int MaxTimebank { get; set; }
        public int TimePerMove { get; set; }
        public int MaxRounds { get; set; }
        public int RoundNumber { get; set; }
        public int Timebank { get; set; }
        public string MyName { get; set; }

        public Dictionary<string, Player.Player> Players { get; set; }
        public Field.Field Field { get; set; }

        public BotState()
        {
            Field = new Field.Field();
            Players = new Dictionary<string, Player.Player>();
        }
    }
}
