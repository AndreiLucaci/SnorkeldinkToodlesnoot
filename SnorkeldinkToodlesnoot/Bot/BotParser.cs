using System;
using LightRidersBot.Move;

namespace LightRidersBot.Bot
{
    public class BotParser
    {
        private readonly LightBot _bot;
        private readonly BotState _currentState;
        public BotParser(LightBot bot)
        {
            _bot = bot;
            _currentState = new BotState();
        }

        public void Run()
        {
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                var parts = line.Split(' ');
                switch (parts[0])
                {
                    case "settings":
                        ParseSettings(parts[1], parts[2]);
                        break;
                    case "update":
                        if (parts[1].Equals("game"))
                        {
                            ParseGameData(parts[2], parts[3]);
                        }
                        break;
                    case "action":
                        if (parts[1].Equals("move"))
                        {
                            var move = _bot.DoMove(_currentState);
                            Console.WriteLine(move?.ToString() ?? MoveType.Pass.ToString());
                        }
                        break;
                    default:
                        Console.Error.WriteLine("Unknown command");
                        break;
                }
            }
        }

        private void ParseSettings(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case "timebank":
                        int time = int.Parse(value);
                        _currentState.MaxTimebank = time;
                        _currentState.Timebank = time;
                        break;
                    case "time_per_move":
                        _currentState.TimePerMove = int.Parse(value);
                        break;
                    case "player_names":
                        var playerNames = value.Split(',');
                        foreach (var playerName in playerNames)
                            _currentState.Players.Add(playerName, new Player.Player(playerName));
                        break;
                    case "your_bot":
                        _currentState.MyName = value;
                        break;
                    case "your_botid":
                        int myId = int.Parse(value);
                        _currentState.Field.MyId = myId;
                        break;
                    case "field_width":
                        _currentState.Field.Width = int.Parse(value);
                        break;
                    case "field_height":
                        _currentState.Field.Height = int.Parse(value);
                        break;
                    case "max_rounds":
                        _currentState.MaxRounds = int.Parse(value);
                        break;

                    default:
                        Console.Error.Write($"Cannot parse settings input with key '{key}'");
                        break;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Cannot parse settings value '{value}' for key '{key}'");
            }
        }

        private void ParseGameData(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case "round":
                        _currentState.RoundNumber = int.Parse(value);
                        break;
                    case "field":
                        _currentState.Field.InitField();
                        _currentState.Field.ParseFromString(value);
                        break;
                    default:
                        Console.Error.WriteLine($"Cannot parse game data input with key '{key}'");
                        break;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Cannot parse game data value '{value}' for key '{key}'");
            }
        }
    }
}
