using System.Numerics;

namespace AoC2025.Day06;

public class Day06
{
    public void Part1()
    {
        var input = File.ReadAllLines("Day06/input.txt").Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
        List<(List<BigInteger> operands, string operation)> operations = [];
        BigInteger sum = 0;

        for (int i = 0; i < input.Count - 1; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (i == 0)
                {
                    operations.Add(([], ""));
                }

                operations[j].operands.Add(BigInteger.Parse(input[i][j]));
            }
        }

        for (int i = 0; i < input[input.Count - 1].Length; i++)
        {
            operations[i] = (operations[i].operands, input[input.Count - 1][i]);
        }

        foreach (var calculation in operations)
        {
            if (calculation.operation == "+")
            {
                sum += calculation.operands.Aggregate(new BigInteger(0), (acc, n) => n + acc);
            }
            else if (calculation.operation == "*")
            {
                sum += calculation.operands.Aggregate(new BigInteger(1), (acc, n) => n * acc);
            }
        }

        Console.WriteLine(sum);
    }

    public void Part2()
    {
        var input = File.ReadAllLines("Day06/input.txt").ToList();
        List<(List<string> operands, string operation)> operations = [];
        BigInteger sum = 0;

        string first = "";
        string second = "";
        string third = "";
        string fourth = "";

        // Sorry for that hardcoded shit, it's Saturday morning 06:00 am... I should be asleep
        for (int i = 0; i < input[0].Length; i++)
        {
            if (input[0][i] == ' ' && input[1][i] == ' ' && input[2][i] == ' ' && input[3][i] == ' ')
            {
                operations.Add((new List<string>() { first.Replace(" ", "0"), second.Replace(" ", "0"), third.Replace(" ", "0"), fourth.Replace(" ", "0") }, ""));
                first = "";
                second = "";
                third = "";
                fourth = "";
            }

            if (i == input[0].Length - 1)
            {
                first += input[0][i];
                second += input[1][i];
                third += input[2][i];
                fourth += input[3][i];
                operations.Add((new List<string>() { first.Replace(" ", "0"), second.Replace(" ", "0"), third.Replace(" ", "0"), fourth.Replace(" ", "0") }, ""));
                first = "";
                second = "";
                third = "";
                fourth = "";
                continue;
            }

            first += input[0][i];
            second += input[1][i];
            third += input[2][i];
            fourth += input[3][i];
        }

        var operands = input[4].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        for (int i = 0; i < operands.Count; i++)
        {
            operations[i] = (operations[i].operands, operands[i]);
        }

        foreach (var calculation in operations)
        {
            var constructedNumbers = new List<BigInteger>();
            var filled = calculation.operands.Select(x => x.ToString().PadLeft(calculation.operands.Max(x => x.ToString().Length), '0')).ToList();

            for (int x = filled[0].Length - 1; x >= 0; x--)
            {
                string number = "";

                for (int y = 0; y < 4; y++)
                {
                    number += filled[y][x];
                }

                if (string.IsNullOrWhiteSpace(number.TrimEnd('0')))
                    continue;

                constructedNumbers.Add(BigInteger.Parse(number.TrimEnd('0')));
            }

            if (calculation.operation == "+")
            {
                sum += constructedNumbers.Aggregate(new BigInteger(0), (acc, n) => n + acc);
            }
            else if (calculation.operation == "*")
            {
                sum += constructedNumbers.Aggregate(new BigInteger(1), (acc, n) => n * acc);
            }
        }

        Console.WriteLine(sum);
    }
}