namespace AoC2025.Day07;

public class Day07
{
    Dictionary<(int x, int y), long> _beams = [];

    public void Part1()
    {
        var input = File.ReadAllLines("Day07/input.txt");
        var splitters = new List<(int x, int y)>();
        var splits = 0;

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '^')
                {
                    splitters.Add((x, y));
                }
            }
        }

        splitters.Reverse();

        for (int i = 0; i < splitters.Count; i++)
        {
            var splitter = splitters[i];
            (int x, int y) radj = (-1, -1);
            (int x, int y) ladj = (-1, -1);
            (int x, int y) block = (-1, -1);

            for (int j = i + 1; j < splitters.Count; j++)
            {
                var adj = splitters[j];
                if (adj.y < splitter.y && adj.x == splitter.x + 1)
                {
                    if (radj.x == -1 && radj.y == -1)
                        radj = adj;
                }
                else if (adj.y < splitter.y && adj.x == splitter.x - 1)
                {
                    if (ladj.x == -1 && ladj.y == -1)
                        ladj = adj;
                }
                else if (adj.y < splitter.y && adj.x == splitter.x)
                {
                    if (block.x == -1 && block.y == -1)
                        block = adj;
                }

                if (radj.x != -1 && radj.y != -1 && ladj.x != -1 && ladj.y != -1 && block.x != -1 && block.y != -1)
                    break;
            }

            if (block.x != -1 && block.y != -1)
            {
                if (radj.y > block.y || ladj.y > block.y)
                {
                    splits++;
                }
            }
            else if (radj.y != -1 || ladj.y != -1)
            {
                splits++;
            }
        }

        Console.WriteLine(splits + 1);
    }

    public void Part2()
    {
        var input = File.ReadAllLines("Day07/input.txt");
        var splitters = new List<(int x, int y)>();
        (int x, int y) start = (0, 0);

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '^')
                {
                    splitters.Add((x, y));
                }
                if (input[y][x] == 'S')
                {
                    start = (x, y);
                }
            }
        }

        Console.WriteLine(CalcTimelines(start, splitters));
    }

    private long CalcTimelines((int x, int y) beam, List<(int x, int y)> splitters)
    {
        var nextSplitter = splitters.FirstOrDefault(splitter => splitter.x == beam.x && splitter.y > beam.y);

        if (nextSplitter is (0, 0))
        {
            return 1;
        }
        if (_beams.TryGetValue(beam, out long timelines))
        {
            return timelines;
        }
        long timeline = CalcTimelines((beam.x - 1, nextSplitter.y), splitters) + CalcTimelines((beam.x + 1, nextSplitter.y), splitters);
        _beams.Add(beam, timeline);
        return timeline;
    }
}