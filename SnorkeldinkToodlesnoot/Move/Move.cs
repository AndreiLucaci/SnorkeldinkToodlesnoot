namespace LightRidersBot.Move
{
    public class Move
    {
        private readonly MoveType _moveType;
        public Move(MoveType moveType)
        {
            _moveType = moveType;
        }

        public override string ToString()
        {
            return _moveType.ToString();
        }
    }
}
