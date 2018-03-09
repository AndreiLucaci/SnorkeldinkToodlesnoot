namespace LightRidersBot.Player
{
    public class Player
    {
        public string Name { get; private set; }

        public Player(string playerName)
        {
            Name = playerName;
        }
    }
}
