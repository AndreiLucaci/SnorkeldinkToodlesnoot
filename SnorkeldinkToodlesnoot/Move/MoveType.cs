using System;
using System.Collections.Generic;

namespace SnorkeldinkToodlesnoot.Move
{
    public enum MoveType
    {
        Up,
        Down,
        Left,
        Right,
        Pass
    }

    public static class MoveHelper
    {
        public static MoveType LastMove { get; private set; }
        public static MoveType GetRandomExcluding(List<MoveType> moveTypes)
        {
            List<MoveType> valuesExcluding = new List<MoveType> { MoveType.Down,MoveType.Left, MoveType.Pass, MoveType.Right, MoveType.Up};

            foreach (var moveType in moveTypes)
                valuesExcluding.Remove(moveType);

            var rand = new Random();
            LastMove = valuesExcluding[rand.Next(valuesExcluding.Count)];
            return LastMove;
        }

        public static string ToString(this MoveType moveType)
        {
            return Enum.GetName(typeof(MoveType), moveType).ToUpper();
        }

        public static MoveType GetOpposite()
        {
            if (LastMove != MoveType.Pass)
                return LastMove == MoveType.Up
                    ? MoveType.Down
                    : LastMove == MoveType.Down
                        ? MoveType.Up
                        : LastMove == MoveType.Left
                            ? MoveType.Right
                            : MoveType.Left;

            return MoveType.Pass;
        }
    }
}
