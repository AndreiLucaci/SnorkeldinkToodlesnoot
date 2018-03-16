using System;
using System.Collections.Generic;
using System.Linq;
using SnorkeldinkToodlesnoot.Bot;
using SnorkeldinkToodlesnoot.Move;

namespace SnorkeldinkToodlesnoot.Field
{
    public class Field
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string[][] FieldPositions { get; set; }
        public Point MyPosition { get; private set; }
        public Point EnemyPosition { get; set; }

        public string MyId { get; set; }
        public string EnId => MyId == BotConstants.Player1 ? BotConstants.Player2 : BotConstants.Player1;

        public void InitField()
        {
            try
            {
                FieldPositions = new string[Width][];
                for (int i = 0; i < Width; i++)
                {
                    FieldPositions[i] = new string[Height];
                }
            }
            catch (Exception)
            {
                throw new Exception("Error: field settings have not been parsed. Cannot initalize field");
            }

            ClearField();
        }

        private void ClearField()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    FieldPositions[x][y] = ".";

            MyPosition = null;
        }

        public void MoveForth(MoveType move, string playerId)
        {
            var origin = GetByPlayerId(playerId);
            var to = Rel(move, origin);
            FieldPositions[origin.X][origin.Y] = BotConstants.Wall;

            if (playerId == MyId)
            {
                Set(to, MyId);
                MyPosition = to;
            }
            else
            {
                Set(to, EnId);
                EnemyPosition = to;
            }
        }

        public void MoveBack(MoveType move, string playerId)
        {
            var origin = Origin(playerId);
            var to = Rel(Mirror[move], origin);
            Set(origin, BotConstants.Floor);

            if (playerId == MyId)
            {
                Set(to, MyId);
                MyPosition = to;
            }
            else
            {
                Set(to, EnId);
                EnemyPosition = to;
            }
        }

        public bool Passable(Point point) => Get(point) == BotConstants.Floor;

        public Dictionary<MoveType, Point> Moves(string playerId)
        {
            var origin = Origin(playerId);

            var possible = Directions.ToDictionary(i => i, i => Rel(i, origin));
            return possible.Where(i => Passable(possible[i.Key])).ToDictionary(i => i.Key, i => i.Value);
        }

        public IEnumerable<Point> Adjacent(Point point = null)
        {
            return Directions.Select(i => Rel(i, point)).ToList();
        }

        public Point Rel(MoveType direction, Point origin = null)
        {
            if (origin == null)
            {
                origin = MyPosition;
            }
            if (direction == MoveType.Up)
            {
                return new Point(origin.X, origin.Y - 1);
            }
            if (direction == MoveType.Left)
            {
                return new Point(origin.X - 1, origin.Y);
            }
            if (direction == MoveType.Right)
            {
                return new Point(origin.X + 1, origin.Y);
            }
            if (direction == MoveType.Down)
            {
                return new Point(origin.X, origin.Y + 1);
            }
            //if (direction == MoveType.Down)
            //{
            //    return new Point(origin.X, origin.Y - 1);
            //}
            //if (direction == MoveType.Right)
            //{
            //    return new Point(origin.X + 1, origin.Y);
            //}
            //if (direction == MoveType.Left)
            //{
            //    return new Point(origin.X -1, origin.Y);
            //}
            //if (direction == MoveType.Up)
            //{
            //    return new Point(origin.X, origin.Y + 1);
            //}

            return origin;
            //throw new Exception("Invlid move");
        }

        public Point Origin(string id) => id == MyId ? MyPosition : EnemyPosition;

        public List<MoveType> Directions  = new List<MoveType>{ MoveType.Down, MoveType.Left, MoveType.Right, MoveType.Up};
        public Dictionary<MoveType, MoveType> Mirror  = new Dictionary<MoveType, MoveType>
        {
            {MoveType.Down, MoveType.Up},
            {MoveType.Up, MoveType.Down},
            {MoveType.Left, MoveType.Right},
            {MoveType.Right, MoveType.Left}
        };

        public string Get(Point coords) => !(0 <= coords.X && coords.X < Width) || !(0 <= coords.Y && coords.Y < Height)
            ? BotConstants.Wall
            : FieldPositions[coords.X][coords.Y];

        public void Set(Point point, string val)
        {
            FieldPositions[point.X][point.Y] = val;
        }

        public Point GetByPlayerId(string playerId) => playerId == MyId ? MyPosition : EnemyPosition;

        public void ParseFromString(string s)
        {
            ClearField();

            var split = s.Split(',');
            var x = 0;
            var y = 0;

            foreach (var value in split)
            {
                FieldPositions[x][y] = value;

                if (FieldPositions[x][y].Equals(MyId))
                {
                    MyPosition = new Point(x, y);
                }

                if (value != MyId && value != "x" && value != ".")
                {
                    EnemyPosition = new Point(x, y);
                }

                if (++x == Width)
                {
                    x = 0;
                    y++;
                }
            }
        }

        public string GetValue(Point point) => FieldPositions[point.X][point.Y];
    }
}
