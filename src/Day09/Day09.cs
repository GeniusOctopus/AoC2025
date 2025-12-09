using System.Drawing;

namespace AoC2025.Day09;

public class Day09
{
    public void Part1()
    {
        var points = File.ReadAllLines("Day09/input.txt").Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(x => new Point(int.Parse(x[0]), int.Parse(x[1]))).ToList();
        Dictionary<(Point, Point), long> areas = [];

        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                areas.Add((points[i], points[j]), (Math.Abs((long)points[i].X - points[j].X) + 1) * (Math.Abs(points[i].Y - (long)points[j].Y) + 1));
            }
        }

        areas = areas.OrderByDescending(x => x.Value).ToDictionary();
        Console.WriteLine(areas.First().Value);
    }

    public void Part2()
    {
        var polygon = File.ReadAllLines("Day09/input.txt").Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(x => new Point(int.Parse(x[0]), int.Parse(x[1]))).ToList();
        Dictionary<(Point, Point), long> areas = [];
        Dictionary<(Point, Point), long> possibleAreas = [];

        for (int i = 0; i < polygon.Count; i++)
        {
            for (int j = i + 1; j < polygon.Count; j++)
            {
                areas.Add((polygon[i], polygon[j]), (Math.Abs((long)polygon[i].X - polygon[j].X) + 1) * (Math.Abs(polygon[i].Y - (long)polygon[j].Y) + 1));
            }
        }

        var crossingLines = areas.OrderByDescending(x => x.Key.Item1.Y == x.Key.Item2.Y).ThenByDescending(x => Math.Abs(x.Key.Item1.X - x.Key.Item2.X)).Select(x => x.Key.Item1.Y).Take(2).ToList();
        var lowerBound = crossingLines.OrderByDescending(x => x).First();
        var upperBound = crossingLines.OrderBy(x => x).First();

        foreach (var area in areas)
        {
            if (IsRectInPolygon(area.Key, polygon, lowerBound, upperBound))
            {
                possibleAreas.Add(area.Key, area.Value);
            }
        }

        possibleAreas = possibleAreas.OrderByDescending(x => x.Value).ToDictionary();
        Console.WriteLine(possibleAreas.First().Value);
    }

    private bool IsRectInPolygon((Point, Point) rect, List<Point> polygon, int lowerBound, int upperBound)
    {
        var checkP1 = new Point(rect.Item1.X, rect.Item2.Y);
        if (!IsPointInPolygon(checkP1, polygon))
        {
            return false;
        }

        var checkP2 = new Point(rect.Item2.X, rect.Item1.Y);
        if (!IsPointInPolygon(checkP2, polygon))
        {
            return false;
        }

        // Using the topology of the input, which seems to always contain two edges, that almost separate the polygon
        if (!(Math.Min(rect.Item1.Y, rect.Item2.Y) >= lowerBound && Math.Max(rect.Item1.Y, rect.Item2.Y) >= lowerBound
            || Math.Min(rect.Item1.Y, rect.Item2.Y) <= upperBound && Math.Max(rect.Item1.Y, rect.Item2.Y) <= upperBound))
        {
            return false;
        }

        return true;
    }

    private bool IsPointInPolygon(Point p, List<Point> polygon)
    {
        int numVertices = polygon.Count;

        double x = p.X, y = p.Y;
        bool inside = false;

        Point p1 = polygon[numVertices - 1];

        for (int i = 0; i < numVertices; p1 = polygon[i++])
        {
            Point p2 = polygon[i];

            if (((double)p2.Y - p1.Y) * (x - p1.X) == ((double)p2.X - p1.X) * (y - p1.Y))
            {
                if (x >= Math.Min(p1.X, p2.X) && x <= Math.Max(p1.X, p2.X) &&
                    y >= Math.Min(p1.Y, p2.Y) && y <= Math.Max(p1.Y, p2.Y))
                {
                    return true;
                }
            }

            double p1y = p1.Y, p2y = p2.Y;
            double p1x = p1.X, p2x = p2.X;

            if (((p1y <= y) && (p2y > y)) || ((p2y <= y) && (p1y > y)))
            {
                double intersectX = (y - p1y) * (p2x - p1x) / (p2y - p1y) + p1x;

                if (x < intersectX)
                {
                    inside = !inside;
                }
            }
        }

        return inside;
    }
}

