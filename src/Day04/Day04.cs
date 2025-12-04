namespace AoC2025.Day04;

public class Day04
{
    public void Part1()
    {
        var input = File.ReadAllLines("Day04/input.txt");
        var sum = 0;

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] != '@')
                {
                    continue;
                }

                var countOfAdj = 0;

                // top
                if (y > 0)
                {
                    countOfAdj += input[y - 1][x] == '@' ? 1 : 0;
                }

                // topleft
                if (y > 0 && x > 0)
                {
                    countOfAdj += input[y - 1][x - 1] == '@' ? 1 : 0;
                }

                // topright
                if (y > 0 && x < input[y].Length - 1)
                {
                    countOfAdj += input[y - 1][x + 1] == '@' ? 1 : 0;
                }

                // left
                if (x > 0)
                {
                    countOfAdj += input[y][x - 1] == '@' ? 1 : 0;
                }

                // right
                if (x < input[y].Length - 1)
                {
                    countOfAdj += input[y][x + 1] == '@' ? 1 : 0;
                }

                // down
                if (y < input.Length - 1)
                {
                    countOfAdj += input[y + 1][x] == '@' ? 1 : 0;
                }

                // downleft
                if (y < input.Length - 1 && x > 0)
                {
                    countOfAdj += input[y + 1][x - 1] == '@' ? 1 : 0;
                }

                // downright
                if (y < input.Length - 1 && x < input[y].Length - 1)
                {
                    countOfAdj += input[y + 1][x + 1] == '@' ? 1 : 0;
                }

                if (countOfAdj < 4)
                {
                    sum++;
                }
            }

        }

        Console.WriteLine(sum);
    }

    public void Part2()
    {
        var input = File.ReadAllLines("Day04/input.txt");
        var sum = 0;

        while (true)
        {
            // ugly shitfuck
            List<List<bool>> map = [];

            for (int y = 0; y < input.Length; y++)
            {
                map.Add(new List<bool>());

                for (int x = 0; x < input[y].Length; x++)
                {
                    map[y].Add(false);
                }
            }

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] != '@')
                    {
                        continue;
                    }

                    var countOfAdj = 0;

                    // top
                    if (y > 0)
                    {
                        countOfAdj += input[y - 1][x] == '@' ? 1 : 0;
                    }

                    // topleft
                    if (y > 0 && x > 0)
                    {
                        countOfAdj += input[y - 1][x - 1] == '@' ? 1 : 0;
                    }

                    // topright
                    if (y > 0 && x < input[y].Length - 1)
                    {
                        countOfAdj += input[y - 1][x + 1] == '@' ? 1 : 0;
                    }

                    // left
                    if (x > 0)
                    {
                        countOfAdj += input[y][x - 1] == '@' ? 1 : 0;
                    }

                    // right
                    if (x < input[y].Length - 1)
                    {
                        countOfAdj += input[y][x + 1] == '@' ? 1 : 0;
                    }

                    // down
                    if (y < input.Length - 1)
                    {
                        countOfAdj += input[y + 1][x] == '@' ? 1 : 0;
                    }

                    // downleft
                    if (y < input.Length - 1 && x > 0)
                    {
                        countOfAdj += input[y + 1][x - 1] == '@' ? 1 : 0;
                    }

                    // downright
                    if (y < input.Length - 1 && x < input[y].Length - 1)
                    {
                        countOfAdj += input[y + 1][x + 1] == '@' ? 1 : 0;
                    }

                    if (countOfAdj < 4)
                    {
                        sum++;
                        map[y][x] = true;
                    }
                }
            }

            if (!map.SelectMany(x => x).Any(x => x == true))
            {
                break;
            }

            for(int i = input.Length - 1; i >= 0; i--)
            {
                for (int j = input[i].Length - 1; j >= 0; j--)
                {
                    if (map[i][j] == true)
                    {
                        var s = input[i].ToCharArray();
                        s[j] = 'x';
                        input[i] = new string(s);
                    }
                }
            }
        }

        Console.WriteLine(sum);
    }
}