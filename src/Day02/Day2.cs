using System.Numerics;

namespace AoC2025.Day02;

public class Day02
{
    public void Part1()
    {
        var idRanges = File.ReadAllText("Day02/input.txt").Split(',').Select(x => new { firstId = x.Split('-')[0], lastId = x.Split('-')[1] });
        // Don't know if the sum could be bigger than ulong.MaxValue, so jsut use BigInteger
        BigInteger sumOfInvalidIds = 0;

        foreach (var idRange in idRanges)
        {
            BigInteger currentId = BigInteger.Parse(idRange.firstId);
            BigInteger lastId = BigInteger.Parse(idRange.lastId);
            // Bit ugly but yeah
            string currentIdStr = idRange.firstId;

            while (currentId <= lastId)
            {
                // Only ids with an even count of numbers can have a matching left and right sequence
                if (currentIdStr.Length % 2 != 0)
                {
                    currentId++;
                    currentIdStr = currentId.ToString();
                    continue;
                }

                List<string> sequences = SplitIntoSequences(currentIdStr, currentIdStr.Length / 2);

                if (sequences.All(x => x == sequences.First()))
                {
                    sumOfInvalidIds += currentId;
                }

                currentId++;
                currentIdStr = currentId.ToString();
            }
        }

        Console.WriteLine(sumOfInvalidIds);
    }

    public void Part2()
    {
        var idRanges = File.ReadAllText("Day02/input.txt").Split(',').Select(x => new { firstId = x.Split('-')[0], lastId = x.Split('-')[1] });
        BigInteger sumOfInvaidIds = 0;

        foreach (var idRange in idRanges)
        {
            BigInteger currentId = BigInteger.Parse(idRange.firstId);
            BigInteger lastId = BigInteger.Parse(idRange.lastId);
            string currentIdStr = currentId.ToString();

            while (currentId <= lastId)
            {
                var sequenceRepeats = false;

                for (int i = 1; i < currentIdStr.Length; i++)
                {
                    List<string> sequences = SplitIntoSequences(currentIdStr, i);

                    if (sequences.All(x => x == sequences.First()))
                    {
                        sequenceRepeats = true;
                        break;
                    }
                }

                if (sequenceRepeats)
                {
                    sumOfInvaidIds += currentId;
                }

                currentId++;
                currentIdStr = currentId.ToString();
            }
        }

        Console.WriteLine(sumOfInvaidIds);
    }

    private List<string> SplitIntoSequences(string input, int sequenceSize)
    {
        List<string> sequences = [];

        for (int i = 0; i < input.Length; i += sequenceSize)
        {
            sequences.Add(input.Substring(i, Math.Min(sequenceSize, input[i..].Length)));
        }

        return sequences;
    }
}