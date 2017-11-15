using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth
{

    enum State
    {
        Empty,
        Wall,
        Visited
    };
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var other = (Point)obj;
            return (X == other.X) && (Y == other.Y);
        }
        public override int GetHashCode()
        {
            return X * 2 + Y * 3;
        }
        public override string ToString()
        {
            return X + " " + Y;
        }

    }

    public class Program
    {
        static List<string> Around(Point beginPoint, Point endPoint, State[,] map)
        {
            Point nextPoint;
            var result = new List<string>();
            var stack = new Stack<Point>();
            stack.Push(beginPoint);

            bool flagx = false;
            bool flagy = false;

            while (stack.Count != 0)
            {
                var point = stack.Pop();
                if (point.X < 0 || point.X >= map.GetLength(0) ||
                    point.Y < 0 || point.Y >= map.GetLength(1))
                    continue;

                map[point.X, point.Y] = State.Visited;

                

                for (var dy = 1; dy >= -1; dy -= 2)
                {
                    nextPoint = new Point { X = point.X + 0, Y = point.Y + dy };
                    if (map[nextPoint.X, nextPoint.Y] != State.Empty)
                    {
                        flagy = true;
                        continue;
                    }
                    stack.Push(nextPoint);
                }
                for (var dx = 1; dx >= -1; dx -= 2)
                {
                    nextPoint = new Point { X = point.X + dx, Y = point.Y + 0 };
                    if (map[nextPoint.X, nextPoint.Y] != State.Empty)
                    {
                        flagx = true;
                        continue;
                    }
                    stack.Push(nextPoint);
                }
                if (flagy & flagx & !point.Equals(endPoint))
                    continue;
                else
                    result.Add((new Point { X = point.X + 1, Y = point.Y + 1 }).ToString());

                if (point.Equals(endPoint)) break;

            }
            var index = result.FindLastIndex(x => x.Equals((new Point { X = beginPoint.X + 2, Y = beginPoint.Y+1 }).ToString()));
            List<string> res = new List<string>();
            res.Add("Y");
            res.Add((new Point { X = beginPoint.X + 1, Y = beginPoint.Y + 1 }).ToString());
            res.AddRange(result.GetRange(index , result.Count - index ));
            return res;
        }

        static void Main()
        {
            string[] str = File.ReadAllLines("in.txt");

            var hight = Convert.ToInt32(str[0]);
            var waight = Convert.ToInt32(str[1]);

            var p = str[str.Length - 2].Split(' ').Where(s => s != "").ToList();
            var first = new Point
            {
                X = Convert.ToInt32(p[0]) - 1,
                Y = Convert.ToInt32(p[1]) - 1
            };

            p = str[str.Length - 1].Split(' ').Where(s => s != "").ToList();
            var second = new Point
            {
                X = Convert.ToInt32(p[0]) - 1,
                Y = Convert.ToInt32(p[1]) - 1
            };

            var map = new State[hight, waight];
            for (int y = 0; y < hight; y++)
            {
                var l = str[y + 2].Split(' ');
                var lab = l.Where(s => s != "").ToList();
                for (int x = 0; x < waight; x++)
                {
                    if (lab[x] == "0")
                        map[y, x] = State.Empty;
                    else
                        map[y, x] = State.Wall;
                }
            }
            var res = Around(first, second, map);

            if (!res.Last().Equals((new Point { X = second.X + 1, Y = second.Y + 1 }).ToString()))
                File.WriteAllText("out.txt", "N");
            else
            {
                File.WriteAllText("out.txt", "Y");
                File.WriteAllLines("out.txt", res);

            }
        }
    }




}
