using System.Collections.Generic;
using System.Linq;
using LightRidersBot.Move;

namespace LightRidersBot.Bot
{
    public class LightBot
    {
        public Move.Move DoMove(BotState state)
        {
            MoveType moveType;

            if (state.RoundNumber == 1)
            { 
                moveType = MoveHelper.GetRandomExcluding(new List<MoveType> {MoveType.Pass});
            }
            else
            {
                MoveType opposite = MoveHelper.GetOpposite();
                moveType = MoveHelper.GetRandomExcluding(new List<MoveType> {MoveType.Pass, opposite});
            }

            return new Move.Move(moveType);
        }
    }
}
