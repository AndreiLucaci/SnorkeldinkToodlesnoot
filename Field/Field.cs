using System;

namespace LightRidersBot.Field
{
    public class Field
    {
        public int MyId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string[][] FieldPositions { get; set; }
        public Point MyPosition { get; private set; }

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

        public void ParseFromString(string s)
        {
            ClearField();

            var split = s.Split(',');
            var x = 0;
            var y = 0;

            foreach (var value in split)
            {
                FieldPositions[x][y] = value;

                if (FieldPositions[x][y].Equals(MyId.ToString()))
                {
                    MyPosition = new Point(x, y);
                }

                if (++x == Width)
                {
                    x = 0;
                    y++;
                }
            }
        }
    }
}
