using System.Numerics;

namespace AoC2025.Day3;

public class Day3
{
    public void Part1()
    {
        var batteryBanks = File.ReadAllLines("Day3/input.txt").Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList());
        BigInteger totalJoltageOutput = 0;
        var batteryCount = 2;

        foreach (var batteryBank in batteryBanks)
        {
            List<(string joltage, int battery)> highJoltageBatteries = [];

            for (int i = 0; i < batteryCount; i++)
            {
                highJoltageBatteries.Add(GetMaxBatteryJoltage(batteryBank, GetSlidingWindowBoundaries(batteryBank, highJoltageBatteries, batteryCount)));
            }

            totalJoltageOutput += BigInteger.Parse(string.Join("", highJoltageBatteries.Select(x => x.joltage)));
        }

        Console.WriteLine(totalJoltageOutput);
    }

    public void Part2()
    {
        var batteryBanks = File.ReadAllLines("Day3/input.txt").Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList());
        BigInteger totalJoltageOutput = 0;
        var batteryCount = 12;

        foreach (var batteryBank in batteryBanks)
        {
            List<(string joltage, int battery)> highJoltageBatteries = [];

            for (int i = 0; i < batteryCount; i++)
            {
                highJoltageBatteries.Add(GetMaxBatteryJoltage(batteryBank, GetSlidingWindowBoundaries(batteryBank, highJoltageBatteries, batteryCount)));
            }

            totalJoltageOutput += BigInteger.Parse(string.Join("", highJoltageBatteries.Select(x => x.joltage)));
        }

        Console.WriteLine(totalJoltageOutput);
    }

    private (int windowStartIndex, int windowEndIndex) GetSlidingWindowBoundaries(List<int> batteryBank, List<(string joltage, int battery)> highJoltageBatteries, int batteryCount)
    {
        int windowStartIndex = highJoltageBatteries.Count != 0 ? highJoltageBatteries.Last().battery + 1 : 0;
        int windowEndIndex = batteryBank.Count - (batteryCount - 1);
        windowEndIndex += highJoltageBatteries.Count;
        if (windowEndIndex >= batteryBank.Count)
        {
            windowEndIndex = batteryBank.Count;
        }

        return (windowStartIndex, windowEndIndex);
    }

    private (string joltage, int battery) GetMaxBatteryJoltage(List<int> line, (int startIndex, int endIndex) slidingWindowBoundaries)
    {
        var subset = line[slidingWindowBoundaries.startIndex..slidingWindowBoundaries.endIndex];

        int maxJoltage = 0;
        int maxJoltageBattery = -1;

        for (int i = 0; i < subset.Count; i++)
        {
            if (subset[i] > maxJoltage)
            {
                maxJoltage = subset[i];
                maxJoltageBattery = i + slidingWindowBoundaries.startIndex;
            }
        }

        return (maxJoltage.ToString(), maxJoltageBattery);
    }
}
