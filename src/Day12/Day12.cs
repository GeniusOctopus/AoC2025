namespace AoC2025.Day12;

public class Day12
{
    public void Part1()
    {
        var input = File.ReadAllText("Day12/input.txt").Split('@', StringSplitOptions.RemoveEmptyEntries);
        var presents = new List<Present>();
        var regions = new List<Region>();
        var fitCount = 0;

        foreach (var present in input[..^1])
        {
            var p = present.Split(':', StringSplitOptions.RemoveEmptyEntries);
            presents.Add(new Present(int.Parse(p[0]), p[1]));
        }

        foreach (var region in input[^1].Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var s = region.Split(':', StringSplitOptions.RemoveEmptyEntries);
            regions.Add(new Region(s[0], s[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()));
        }

        // I thought, hmmm ... let's be smart and filter out the areas, where the presents already fit without twisting and turning them
        for (int i = regions.Count - 1; i >= 0; i--)
        {
            if (regions[i].Map.GetLongLength(0) * regions[i].Map.GetLongLength(1) >= regions[i].NeededPresents.Sum() * 9)
            {
                fitCount++;
                regions.RemoveAt(i);
            }
        }

        // Luckily I decided to plug the result of the filteraboo in the textbox and ... that was it haha
        // But I realized it after I wrote the mess down there, that I thought could come in handy, when I have to try to make the presents fit
        // Buuut I will not delete the unneccesary code, look at it... and laugh with me :D
        // P.S. Yay, AoC 2025 done - It was soo much fun!
        Console.WriteLine(fitCount);
    }

    public void Part2()
    {
        Console.WriteLine(@"\(^.^)/");
    }
}

public class Present
{
    public int Id { get; set; }
    public List<Matrix3x3> Sprites { get; set; }

    public Present(int id, string sprite)
    {
        Id = id;
        Sprites = [];

        var s = sprite.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Matrix3x3 initalState = new();

        for (int i = 0; i < s.Length; i++)
        {
            for (int j = 0; j < s[i].Length; j++)
            {
                initalState.M[i, j] = s[i][j] == '#' ? id : -1;
            }
        }

        Sprites.Add(initalState);
        Sprites.Add(Matrix3x3.Rotate90(initalState));
        Sprites.Add(Matrix3x3.Rotate180(initalState));
        Sprites.Add(Matrix3x3.Rotate270(initalState));

        Deduplicate();
    }

    private void Deduplicate()
    {
        Sprites = Sprites.Distinct().ToList();
    }
}

public class Region
{
    public int[,] Map { get; set; }
    public List<int> NeededPresents { get; set; }

    public Region(string gridDefinition, List<int> neededPresents)
    {
        var i = int.Parse(gridDefinition.Split('x', StringSplitOptions.RemoveEmptyEntries)[1]);
        var j = int.Parse(gridDefinition.Split('x', StringSplitOptions.RemoveEmptyEntries)[0]);

        Map = new int[i,j];

        for (int idx = 0; idx < i; idx++)
        {
            for (int jdx = 0; jdx < j; jdx++)
            {
                Map[idx,jdx] = -1;
            }
        }

        NeededPresents = neededPresents;
    }
}


public class Matrix3x3 : IEquatable<Matrix3x3>
{
    private readonly static Matrix3x3 s_rverseRowCol = new(new int[,]
    {
        {0,0,1},
        {0,1,0},
        {1,0,0}
    });

    public int[,] M { get; private set; }

    public Matrix3x3()
    {
        M = new int[,]
        {
            {-1,-1,-1},
            {-1,-1,-1},
            {-1,-1,-1}
        };
    }

    public Matrix3x3(int[,] matrix)
    {
        M = new int[,]
        {
            {-1,-1,-1},
            {-1,-1,-1},
            {-1,-1,-1}
        };

        (M[0, 0], M[0, 1], M[0, 2]) = (matrix[0, 0], matrix[0, 1], matrix[0, 2]);
        (M[1, 0], M[1, 1], M[1, 2]) = (matrix[1, 0], matrix[1, 1], matrix[1, 2]);
        (M[2, 0], M[2, 1], M[2, 2]) = (matrix[2, 0], matrix[2, 1], matrix[2, 2]);
    }

    public Matrix3x3(Matrix3x3 matrix)
    {
        M = new int[,]
        {
            {-1,-1,-1},
            {-1,-1,-1},
            {-1,-1,-1}
        };

        (M[0, 0], M[0, 1], M[0, 2]) = (matrix.M[0, 0], matrix.M[0, 1], matrix.M[0, 2]);
        (M[1, 0], M[1, 1], M[1, 2]) = (matrix.M[1, 0], matrix.M[1, 1], matrix.M[1, 2]);
        (M[2, 0], M[2, 1], M[2, 2]) = (matrix.M[2, 0], matrix.M[2, 1], matrix.M[2, 2]);
    }

    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        Matrix3x3 c = new(new int[,]
        {{-1,-1,-1},
        {-1,-1,-1},
        {-1,-1,-1}});

        for (int i = 0; i < a.M.GetLength(1); i++)
        {
            for (int j = 0; j < a.M.GetLength(0); j++)
            {
                c.M[i, j] = a.M[i, 0] * b.M[0, j] + a.M[i, 1] * b.M[1, j] + a.M[i, 2] * b.M[2, j];
            }
        }

        return c;
    }

    public static bool operator ==(Matrix3x3 a, Matrix3x3 b)
    {
        var equal = true;

        for (int i = 0; i < a.M.GetLength(1); i++)
        {
            for (int j = 0; j < a.M.GetLength(0); j++)
            {
                if (a.M[i, j] != b.M[i, j])
                {
                    equal = false;
                }
            }
        }

        return equal;
    }

    public static bool operator !=(Matrix3x3 a, Matrix3x3 b)
    {
        var notEqual = false;

        for (int i = 0; i < a.M.GetLength(1); i++)
        {
            for (int j = 0; j < a.M.GetLength(0); j++)
            {
                if (a.M[i, j] != b.M[i, j])
                {
                    notEqual = true;
                    break;
                }
            }
            if (notEqual)
                break;
        }

        return notEqual;
    }

    public static void Print(Matrix3x3 matrix)
    {
        for (int i = 0; i < matrix.M.GetLength(1); i++)
        {
            for (int j = 0; j < matrix.M.GetLength(0); j++)
            {
                Console.Write($"{matrix.M[i, j]}\t");
            }
            Console.WriteLine();
        }
    }

    public static Matrix3x3 Transpose(Matrix3x3 a)
    {
        Matrix3x3 b = new(a);

        (b.M[0, 1], b.M[1, 0]) = (a.M[1, 0], a.M[0, 1]);
        (b.M[0, 2], b.M[2, 0]) = (a.M[2, 0], a.M[0, 2]);
        (b.M[1, 2], b.M[2, 1]) = (a.M[2, 1], a.M[1, 2]);

        return b;
    }

    public static Matrix3x3 Rotate90(Matrix3x3 a)
    {
        Matrix3x3 b = new(a);

        return Transpose(b) * s_rverseRowCol;
    }

    public static Matrix3x3 Rotate180(Matrix3x3 a)
    {
        Matrix3x3 b = Rotate90(a);
        
        return Rotate90(b);
    }

    public static Matrix3x3 Rotate270(Matrix3x3 a)
    {
        Matrix3x3 b = new(a);

        return s_rverseRowCol * Transpose(b);
    }

    public bool Equals(Matrix3x3? other)
    {
        if (other is null)
            return false;

        return this == other;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Matrix3x3 other)
        {
            return this == other;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine($"{M[0,0]}{M[0,1]}{M[0,2]}",$"{M[1,0]}{M[1,1]}{M[1,2]}",$"{M[2,0]}{M[2,1]}{M[2,2]}");
    }
}
