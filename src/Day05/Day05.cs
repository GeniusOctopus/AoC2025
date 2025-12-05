using System.Numerics;

namespace AoC2025.Day05;

public class Day05
{
    public void Part1()
    {
        var input = File.ReadAllLines("Day05/input.txt");
        var ranges = input.Where(x => x.Contains('-')).Select(x => new { LowerBound = ulong.Parse(x[..x.IndexOf('-')]), UpperBound = ulong.Parse(x[(x.IndexOf('-') + 1)..]) });
        var ids = input.Where(x => ulong.TryParse(x, out ulong id)).Select(x => ulong.Parse(x));
        ulong sum = 0;

        sum = ids.Aggregate(0UL, (acc, n) => acc += ranges.Any(x => n <= x.UpperBound && n >= x.LowerBound) ? 1UL : 0UL);

        Console.WriteLine(sum);
    }

    public void Part2()
    {
        var input = File.ReadAllLines("Day05/input.txt");
        var ranges = input.Where(x => x.Contains('-')).Select(x => new { LowerBound = BigInteger.Parse(x[..x.IndexOf('-')]), UpperBound = BigInteger.Parse(x[(x.IndexOf('-') + 1)..]) });
        BigInteger sum = 0;
        Node? root = null;

        foreach (var range in ranges)
            root = AddRecursive(root, range.LowerBound, range.UpperBound);

        Console.WriteLine(CalcNodeCount(root));
    }

    public Node AddRecursive(Node? node, BigInteger lowerBound, BigInteger upperBound)
    {
        if (node == null)
            return new Node(lowerBound, upperBound);

        if (lowerBound < node.LowerBound && upperBound > node.UpperBound)
        {
            var calculatedUpper = upperBound >= node.LowerBound ? node.LowerBound - 1 : upperBound;
            node.Left = AddRecursive(node.Left, lowerBound, calculatedUpper);
            var calculatedLower = lowerBound <= node.UpperBound ? node.UpperBound + 1 : lowerBound;
            node.Right = AddRecursive(node.Right, calculatedLower, upperBound);
        }
        else if (lowerBound < node.LowerBound)
        {
            var calculatedUpper = upperBound >= node.LowerBound ? node.LowerBound - 1 : upperBound;
            node.Left = AddRecursive(node.Left, lowerBound, calculatedUpper);
        }
        else if (upperBound > node.UpperBound)
        {
            var calculatedLower = lowerBound <= node.UpperBound ? node.UpperBound + 1 : lowerBound;
            node.Right = AddRecursive(node.Right, calculatedLower, upperBound);
        }

        return node;
    }

    public BigInteger CalcNodeCount(Node? root)
    {
        if (root is null)
            return 0;

        BigInteger l = CalcNodeCount(root.Left);
        BigInteger r = CalcNodeCount(root.Right);

        return root.UpperBound - root.LowerBound + 1 + l + r;
    }

    public class Node(BigInteger lowerBound, BigInteger upperBound)
    {
        public BigInteger LowerBound { get; set; } = lowerBound;
        public BigInteger UpperBound { get; set; } = upperBound;
        public Node? Left { get; set; }
        public Node? Right { get; set; }
    }
}