namespace AoC2025.Day11;

public class Day11
{
    public void Part1()
    {
        var devices = File.ReadAllLines("Day11/input.txt").Select(x => x.Split(':', StringSplitOptions.RemoveEmptyEntries))
            .Select(x => new KeyValuePair<string, List<string>>(x[0], x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList())).ToDictionary();

        var queue = new Queue<string>();
        queue.Enqueue("you");
        int counter = 0;

        while (queue.Count > 0)
        {
            var nextDevice = queue.Dequeue();

            if (devices[nextDevice].Any(x => x == "out"))
            {
                counter++;
                continue;
            }

            foreach (var next in devices[nextDevice]) queue.Enqueue(next);
        }

        Console.WriteLine(counter);
    }

    public void Part2()
    {
        var devices = File.ReadAllLines("Day11/input.txt").Select(x => x.Split(':', StringSplitOptions.RemoveEmptyEntries))
            .Select(x => new KeyValuePair<string, List<string>>(x[0], x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList())).ToDictionary();
        Dictionary<string, ulong> cache = [];

        var fft = DFS(devices, cache, "svr", "fft");
        cache.Clear();
        var dac = DFS(devices, cache, "fft", "dac");
        cache.Clear();
        var output = DFS(devices, cache, "dac", "out");

        Console.WriteLine(fft * dac * output);
    }

    private ulong DFS(Dictionary<string, List<string>> graph, Dictionary<string, ulong> cache, string current, string target)
    {
        if (current == target)
            return 1;

        if (cache.ContainsKey(current))
            return cache[current];

        ulong pathCount = 0;

        if (graph.ContainsKey(current))
        {
            foreach (var neighbor in graph[current])
            {
                pathCount += DFS(graph, cache, neighbor, target);
            }
        }

        cache[current] = pathCount;
        return pathCount;
    }
}