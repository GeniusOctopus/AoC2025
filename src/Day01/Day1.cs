namespace AoC2025.Day01;

public class Day01
{
    private const int InitialDialNumber = 50;
    private const int MinDialNumber = 0;
    private const int MaxDialNumber = 99;
    private const int DialNumberCount = 100;
    private const int RotateLeft = -1;
    private const int RotateRight = 1;

    public void Part1()
    {
        var document = File.ReadAllLines("Day01/input.txt");

        var dialNumber = InitialDialNumber;
        var countZeroVisited = 0;

        foreach (var instruction in document)
        {
            var rotationDirection = instruction[0] == 'L' ? RotateLeft : RotateRight;
            var clicks = int.Parse(instruction[1..]);

            // Transpose result to fit in the range 0..99
            dialNumber = (dialNumber + (rotationDirection * clicks)) % DialNumberCount;

            if (dialNumber == MinDialNumber)
            {
                countZeroVisited++;
            }
        }

        Console.WriteLine($"Final dial number: {dialNumber}");
        Console.WriteLine($"Count zero visited: {countZeroVisited}");
    }

    public void Part2()
    {
        var document = File.ReadAllLines("Day01/input.txt");

        var dialNumber = InitialDialNumber;
        var countZeroVisited = 0;

        foreach (var instruction in document)
        {
            var rotationDirection = instruction[0] == 'L' ? RotateLeft : RotateRight;
            var clicks = int.Parse(instruction[1..]);

            // full rotation always lead to the visit of 0
            // therefore calculate number of full rotations
            if (clicks > MaxDialNumber)
            {
                countZeroVisited += clicks / DialNumberCount;
                clicks %= DialNumberCount;
            }

            if (dialNumber != MinDialNumber && rotationDirection == RotateLeft && dialNumber - clicks <= MinDialNumber)
            {
                countZeroVisited++;
            }

            if (dialNumber != MinDialNumber && rotationDirection == RotateRight && dialNumber + clicks >= DialNumberCount)
            {
                countZeroVisited++;
            }

            // Transpose result to fit in the range 0..99
            dialNumber = (dialNumber + (rotationDirection * clicks)) % DialNumberCount;

            if (dialNumber < MinDialNumber)
            {
                dialNumber += DialNumberCount;
            }
        }

        Console.WriteLine($"Final dial number: {dialNumber}");
        Console.WriteLine($"Count zero visited: {countZeroVisited}");
    }
}