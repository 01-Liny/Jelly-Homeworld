using UnityEngine;
using System.Collections;
using System.Collections.Generic;//使用泛型的时候需要这个
using System.Linq;

//对 List<Point> 的一些扩展方法
public static class ListHelp
{
    public static bool Exists(this List<Point> points, Point point)
    {
        foreach (Point p in points)
            if ((p.X == point.X) && (p.Y == point.Y))
                return true;
        return false;
    }

    public static bool Exists(this List<Point> points, int x, int y)
    {
        foreach (Point p in points)
            if ((p.X == x) && (p.Y == y))
                return true;
        return false;
    }

    public static Point MinPoint(ref List<Point> points)
    {
        points = points.OrderBy(p => p.F).ToList();
        return points[0];
    }
    public static void Add(this List<Point> points, int x, int y)
    {
        Point point = new Point(x, y);
        points.Add(point);
    }

    public static Point Get(this List<Point> points, Point point)
    {
        foreach (Point p in points)
            if ((p.X == point.X) && (p.Y == point.Y))
                return p;
        return null;
    }

    public static void Remove(this List<Point> points, int x, int y)
    {
        foreach (Point point in points)
        {
            if (point.X == x && point.Y == y)
                points.Remove(point);
        }
    }
}
