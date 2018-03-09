using SnorkeldinkToodlesnoot.Move;

namespace SnorkeldinkToodlesnoot.Bot
{
    public class AwesomeBot
    {
        public Move.Move DoMove(BotState state)
        {
            var me = state.Field.MyPosition;
            var enemty = state.Field.EnemyPosition;

            return new Move.Move(MoveType.Pass);
        }
    }
}
