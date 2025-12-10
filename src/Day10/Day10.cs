namespace AoC2025.Day10;

public class Day10
{
    public void Part1()
    {
        var machines = File.ReadAllLines("Day10/input.txt").Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).Select(x => new Machine
        {
            StartupPattern = x[0].TrimStart("[").TrimEnd("]").ToString(),
            Instructions = x[1..^1].Select(inst => new Instruction { Levers = inst.TrimStart("(").TrimEnd(")").ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)).ToList() }).ToList(),
            Joltage = x[^1]
        });

        var buttonPresses = 0;

        foreach (var machine in machines)
        {
            int minPresses = GetMinNumberOfPresses(machine);

            buttonPresses += minPresses;
        }

        Console.WriteLine(buttonPresses);
    }

    static int GetMinNumberOfPresses(Machine machine)
    {
        // Lever states are just 0s and 1s -> use XOR logic

        int targetMask = 0;
        for (int i = 0; i < machine.StartupPattern.Length; i++)
        {
            if (machine.StartupPattern[i] == '#')
            {
                targetMask |= 1 << i;
            }
        }

        List<int> buttonMasks = [];

        foreach (var instruction in machine.Instructions)
        {
            int buttonMask = 0;

            foreach (var lever in instruction.Levers)
            {
                buttonMask |= 1 << lever;
            }

            buttonMasks.Add(buttonMask);
        }

        // Using BFS, hoping there are not that many lights lol

        Queue<(int state, int presses)> nextStates = [];
        HashSet<int> visitedStates = [];

        int startState = 0;
        nextStates.Enqueue((startState, 0));
        visitedStates.Add(startState);

        while (nextStates.Count > 0)
        {
            var (current, depth) = nextStates.Dequeue();

            if (current == targetMask)
            {
                return depth;
            }

            foreach (int button in buttonMasks)
            {
                int nextState = current ^ button;

                if (!visitedStates.Contains(nextState))
                {
                    visitedStates.Add(nextState);
                    nextStates.Enqueue((nextState, depth + 1));
                }
            }
        }

        return -1;
    }
}

public class Machine
{
    public string StartupPattern { get; set; }
    public List<Instruction> Instructions { get; set; }
    public string Joltage { get; set; }
}

public class Instruction
{
    public List<int> Levers { get; set; }

    public override string ToString()
    {
        return string.Join(",", Levers);
    }
}
