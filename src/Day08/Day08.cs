namespace AoC2025.Day08;

public class Day08
{
    public void Part1And2(int maxIterations)
    {
        var input = File.ReadAllLines("Day08/input.txt").Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(x => new JunctionBox { X = int.Parse(x[0]), Y = int.Parse(x[1]), Z = int.Parse(x[2]) }).ToList();

        List<DistanceMap> distanceMaps = [];

        for (int i = 0; i < input.Count; i++)
        {
            for (int j = i + 1; j < input.Count; j++)
            {
                distanceMaps.Add(new DistanceMap
                {
                    Self = input[i],
                    Compare = input[j],
                    Distance = input[i].EuclidianDistance(input[j])
                });
            }
        }

        distanceMaps = distanceMaps.OrderBy(x => x.Distance).ToList();

        int numCon = 0;
        Dictionary<JunctionBox, List<JunctionBox>> circuits = [];
        DistanceMap? lastMerge = null;

        foreach (var map in distanceMaps)
        {
            if (numCon >= maxIterations)
            {
                break;
            }

            if (!circuits.TryGetValue(map.Self, out List<JunctionBox>? _) && !circuits.TryGetValue(map.Compare, out List<JunctionBox>? _))
            {
                List<JunctionBox> l = [map.Self, map.Compare];
                circuits.Add(map.Self, l);
                circuits.Add(map.Compare, l);
                lastMerge = map;
            }
            else if (circuits.TryGetValue(map.Self, out List<JunctionBox>? circuitSelf) && !circuits.TryGetValue(map.Compare, out List<JunctionBox>? _))
            {
                circuitSelf.Add(map.Compare);
                circuits.Add(map.Compare, circuitSelf);

                lastMerge = map;
            }
            else if (!circuits.TryGetValue(map.Self, out List<JunctionBox>? _) && circuits.TryGetValue(map.Compare, out List<JunctionBox>? circuitCompare))
            {
                circuitCompare.Add(map.Self);
                circuits.Add(map.Self, circuitCompare);

                lastMerge = map;
            }
            else if (circuits.TryGetValue(map.Self, out List<JunctionBox>? cS) && circuits.TryGetValue(map.Compare, out List<JunctionBox>? cP))
            {
                if (cS != cP)
                {
                    lastMerge = map;

                    cS.AddRange(cP);
                    var a = circuits.Where(x => x.Value == cP).ToList();
                    for (int x = 0; x < a.Count; x++)
                    {
                        circuits[a[x].Key] = cS;
                    }
                }
            }

            numCon++;
        }

        var largest = circuits.GroupBy(x => x.Value).Select(x => x.Select(y => y.Value.Count)).Select(x => x.Count()).OrderByDescending(x => x).Take(3).Aggregate(1, (acc, n) => acc * n);
        Console.WriteLine("Part1:" + largest);
        Console.WriteLine("Part2:" + lastMerge.Self.X * lastMerge.Compare.X);
    }
}

public class DistanceMap
{
    public JunctionBox Self { get; set; }
    public JunctionBox Compare { get; set; }
    public double Distance { get; set; }
}

public class JunctionBox
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public double EuclidianDistance(JunctionBox other)
    {
        return Math.Sqrt(Math.Pow((double)this.X - other.X, 2) + Math.Pow((double)this.Y - other.Y, 2) + Math.Pow((double)this.Z - other.Z, 2));
    }
}