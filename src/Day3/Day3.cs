using System.Numerics;

namespace AoC2025.Day3;

public class Day3
{
    public void Part1()
    {
        var input = File.ReadAllLines("Day3/input.txt").Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList());
        int sum = 0;

        foreach (var line in input)
        {
            int currentMaxFromLeft = 0;
            int indexOfCurrentMaxFromLeft = -1;

            for (int i = 0; i < line.Count - 1; i++)
            {
                if (line[i] > currentMaxFromLeft)
                {
                    currentMaxFromLeft = line[i];
                    indexOfCurrentMaxFromLeft = i;
                }
            }

            int currentMaxFromRight = 0;
            int indexOfCurrentMaxFromRight = -1;

            for (int i = line.Count - 1; i > 0; i--)
            {
                if (i == indexOfCurrentMaxFromLeft)
                {
                    break;
                }

                if (line[i] > currentMaxFromRight)
                {
                    currentMaxFromRight = line[i];
                    indexOfCurrentMaxFromRight = i;
                }
            }

            sum += int.Parse($"{currentMaxFromLeft}{currentMaxFromRight}");
        }

        Console.WriteLine(sum);
    }

    public void Part2()
    {
        var input = File.ReadAllLines("Day3/input.txt").Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList());
        BigInteger sum = 0;

        foreach (var line in input)
        {
            List<(string jolt, int index)> jolts = [];

            for (int count = 0; count < 12; count++)
            {
                int endIndex = line.Count - 11;
                endIndex += jolts.Count;
                if (endIndex >= line.Count)
                {
                    endIndex = line.Count;
                }

                int startIndex = 0;

                if (jolts.Count != 0)
                {
                    startIndex = jolts.Last().index + 1;
                }
                

                var subset = line[startIndex..endIndex];

                int currentMaxFromLeft = 0;
                int indexOfCurrentMaxFromLeft = -1;

                for (int i = 0; i < subset.Count - 1; i++)
                {
                    if (subset[i] > currentMaxFromLeft)
                    {
                        currentMaxFromLeft = subset[i];
                        indexOfCurrentMaxFromLeft = i;
                    }
                }

                // int currentMaxFromRight = 0;
                // int indexOfCurrentMaxFromRight = -1;
                // int minAvailableIndex = line.Count - 1;
                // minAvailableIndex -= minAvailableIndex - 12;
                // minAvailableIndex -= jolts.Count;

                // for (int i = line.Count - 1; i >= 11 - jolts.Count; i--)
                // {
                //     if (jolts.Any(x => x.index == i))
                //     {
                //         continue;
                //     }

                //     if (line[i] > currentMaxFromRight)
                //     {
                //         currentMaxFromRight = line[i];
                //         indexOfCurrentMaxFromRight = i;
                //     }
                // }

                jolts.Add((currentMaxFromLeft.ToString(), indexOfCurrentMaxFromLeft));

                // int currentMaxFromLeft = 0;
                // int indexOfCurrentMaxFromLeft = -1;

                // for (int i = 0; i < line.Count - 1; i++)
                // {
                //     if (line[i] > currentMaxFromLeft)
                //     {
                //         currentMaxFromLeft = line[i];
                //         indexOfCurrentMaxFromLeft = i;
                //     }
                // }
            }

            jolts.Reverse();
            var asd = string.Join("", jolts.Select(x => x.jolt));
            sum += BigInteger.Parse($"{asd}");
        }

        Console.WriteLine(sum);
    }
}

// 987654321111 + 811111111119 + 434234234278 + 888911112111 = 3121910778619