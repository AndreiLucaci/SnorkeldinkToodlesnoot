﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SnorkeldinkToodlesnoot.Field;
using SnorkeldinkToodlesnoot.Helper;
using SnorkeldinkToodlesnoot.Move;

namespace SnorkeldinkToodlesnoot.Bot
{
    public class AwesomeBot
    {
        private DateTime _starTime;
        private int _maxTimePerRound;

        public Move.Move DoMove(BotState state)
        {
            var depth = 1;

            var move = MoveType.None;

            _starTime = DateTime.Now;
            _maxTimePerRound = state.TimePerMove;

            try
            {
                while (true)
                {
                    var tuple = AlphaBeta(state.Field, depth, decimal.MinValue, decimal.MaxValue, state.Field.MyId,
                        move);
                    depth++;
                    move = tuple.Item2;
                }
            }
            catch (ElapsedTimeException)
            {
                var moves = GetAvailableMoves(state.Field.MyPosition, state.Field);

                var enumerable = moves as Tuple<Point, MoveType>[] ?? moves.ToArray();
                if (move != MoveType.None)
                {
                    return new Move.Move(move);
                }

                var el = enumerable.FirstOrDefault();
                if (el != null)
                {
                    return new Move.Move(el.Item2);
                }
                return new Move.Move(MoveType.Pass);
            }
            catch (Exception)
            {
                return new Move.Move(MoveType.Pass);
            }
        }

        private void CheckTime()
        {
            var now = DateTime.Now;
            var elapsed = now.Subtract(_starTime);
            if ( _maxTimePerRound * 0.90 - elapsed.TotalMilliseconds < 0)
            {
                throw new ElapsedTimeException();
            }
        }

        public int Dist(Point me, Point en)
        {
            return Math.Abs(me.X - en.X) + Math.Abs(me.Y - en.Y);
        }

        public bool AreConnected(Field.Field field, Point st, Point en)
        {
            var closedset = new HashSet<Point>();
            var openset = new List<Point> {en};

            var gScore = new Dictionary<Point, int>
            {
                [st] = 0
            };

            var hScore = new Dictionary<Point, int>{ [st] = Dist(st, en)};
            var fScore = new Dictionary<Point, int>{[st] = hScore[st]};

            while (openset.Count > 0)
            {
                openset.Sort((x, y) => LowestF(x, y, fScore));
                var pointX = openset.Pop();

                if (Equals(pointX, en)) return true;
                closedset.Add(pointX);

                foreach (var pointY in GetNeighbourNodes(field, pointX))
                {
                    if (closedset.Contains(pointY)) continue;

                    var tentativeGScore = gScore[pointX] + 1;
                    var tentativeIsBetter = false;
                    if (!openset.Contains(pointY))
                    {
                        openset.Add(pointY);
                        tentativeIsBetter = true;
                    }
                    else if (tentativeGScore < gScore[pointY])
                    {
                        tentativeIsBetter = true;
                    }

                    if (tentativeIsBetter)
                    {
                        gScore[pointY] = tentativeGScore;
                        hScore[pointY] = Dist(pointY, en);
                        fScore[pointY] = gScore[pointY] + hScore[pointY];
                    }
                }
            }

            return false;
        }

        public HashSet<Point> FillForm(Field.Field field, Point me, int maxi = 200)
        {
            var old = new HashSet<Point>();
            var newS = new HashSet<Point> {me};
            while (newS.Count > 0 && old.Count < maxi)
            {
                var t = newS.Pop();
                old.Add(t);

                foreach (var i in GetNeighbourNodes(field, t))
                {
                    if (field.Passable(i) || old.Contains(i)) continue;
                    else newS.Add(i);
                }

            }
            return old;
        }

        public int Evaluate(Field.Field field, Point player)
        {
            var meValidMoves = field.Moves(field.MyId);
            var enValidMobes = field.Moves(field.EnId);

            var playersAdjacent = field.Adjacent(field.MyPosition).Contains(field.EnemyPosition);

            int result = 0;

            if (meValidMoves.Count == 0 || enValidMobes.Count == 0)
            {
                if (meValidMoves.Count > 0)
                {
                    result = playersAdjacent ? -11 : 100;
                }
                else if (enValidMobes.Count > 0)
                {
                    result = playersAdjacent ? -11 : -100;
                }
                else
                {
                    result = -11;
                }
            }
            else
            {
                if (!AreConnected(field, field.MyPosition, field.EnemyPosition))
                {
                    var mine = FillForm(field, field.MyPosition);
                    var their = FillForm(field, field.EnemyPosition);

                    result = 12 + Decimal.ToInt32(Convert.ToDecimal(Math.Abs(mine.Count - their.Count)) /
                                                  Convert.ToDecimal(Math.Max(mine.Count, their.Count) * 86));

                    if (their.Count > mine.Count)
                    {
                        result = -result;
                    }
                }
                else
                {
                    result = 0;
                }
            }

            return result * ComputePlayerValue(field, player);
        }

        private int ComputePlayerValue(Field.Field field, string playerId)
        {
            var player = field.GetByPlayerId(playerId);
            return ComputePlayerValue(field, player);
        }

        private int ComputePlayerValue(Field.Field field, Point player) => (player.Equals(field.MyPosition) ? 1 : 2);


        public Tuple<decimal, MoveType> AlphaBeta(Field.Field field, int depth, decimal alpha, decimal beta, string player, MoveType bestMovetype = MoveType.None)
        {
            var moves = field.Moves(player).Select(i => i.Key).ToList();

            //Console.Error.WriteLine($"update Curent-depth-{depth}");

            if (depth == 0 || moves.Count == 0)
            {
                alpha = Evaluate(field, field.GetByPlayerId(player));
                return Tuple.Create(alpha, MoveType.None);
            }

            moves = OrderByClosness(field, new Point(field.Height / 2, field.Width / 2), moves);
            moves = OrderByClosness(field, field.EnemyPosition, moves);

            if (bestMovetype != MoveType.None)
            {
                moves.Remove(bestMovetype);
                moves.Insert(0, bestMovetype);
            }

            var bestMove = moves.Count > 0 ? moves[0] : MoveType.None;

            foreach (var move in moves)
            {
                CheckTime();

                field.MoveForth(move, player);
                var val = -AlphaBeta(field, depth - 1, -beta, -alpha, field.EnId).Item1;
                field.MoveBack(move, player);

                if (val > alpha)
                {
                    bestMove = move;
                    alpha = val;
                    if (alpha >= beta)
                    {
                        return Tuple.Create(alpha, bestMove);
                    }
                }
            }

            decimal suicideVal = -11 * (ComputePlayerValue(field, player) + 1);
            if (suicideVal > alpha)
            {
                foreach (var direction in field.Directions)
                {
                    if (player == field.MyId || player == field.EnId)
                    {
                        return Tuple.Create(suicideVal, direction);
                    }
                }
            }

            return Tuple.Create(alpha, bestMove);
        }

        public int Order(MoveType a, MoveType b, int dx, int dy)
        {
            if (dy > 0)
            {
                if (a == MoveType.Down) return 1;
                if (b == MoveType.Down) return -1;
                if (a == MoveType.Up) return -1;
                if (b == MoveType.Up) return 1;
            }
            if (dy < 0)
            {
                if (a == MoveType.Down) return -1;
                if (b == MoveType.Down) return 1;
                if (a == MoveType.Up) return 1;
                if (b == MoveType.Up) return -1;
            }
            if (dx > 0)
            {
                if (a == MoveType.Right) return 1;
                if (b == MoveType.Right) return -1;
                if (a == MoveType.Left) return -1;
                if (b == MoveType.Left) return 1;
            }
            if (dx < 0)
            {
                if (a == MoveType.Right) return -1;
                if (b == MoveType.Right) return 1;
                if (a == MoveType.Left) return 1;
                if (b == MoveType.Left) return -1;
            }
            return 0;
        }

        public List<MoveType> OrderByClosness(Field.Field field, Point to,
            List<MoveType> moves)
        {
            var me = field.MyPosition;

            var dx = me.X - to.X;
            var dy = me.Y - to.Y;

            moves.Sort(((a, b) => Order(a, b, dx, dy)));
            return moves;
        }

        public int LowestF(Point x, Point y, Dictionary<Point, int> f) => f[x] > f[y] ? -1 : (f[x] == f[y] ? 0 : 1);

        public IEnumerable<Point> GetNeighbourNodes(Field.Field field, Point x)
        {
            return GetAvailableMoves(x, field).Select(i => i.Item1);
        }

        public IEnumerable<Tuple<Point, MoveType>> GetAvailableMoves(Point point, Field.Field field)
        {
            var validMoves = DetermineMoves(point, field.Width, field.Height);

            return validMoves.Where(i => field.GetValue(i.Item1).Free());
        }

        private IEnumerable<Tuple<Point, MoveType>> DetermineMoves(Point point, int width, int height)
        {
            if (point.X - 1 >= 0)
            {
                yield return Tuple.Create(new Point(point.X -1, point.Y), MoveType.Left);
            }

            if (point.Y-1 >= 0)
            {
                yield return Tuple.Create(new Point(point.X, point.Y - 1), MoveType.Up);
            }

            if (point.X + 1 < width)
            {
                yield return Tuple.Create(new Point(point.X + 1, point.Y), MoveType.Right);
            }

            if (point.Y + 1 < height)
            {
                yield return Tuple.Create(new Point(point.X, point.Y + 1), MoveType.Down);
            }
        }
    }
}
