namespace AoC2025.Day10;

public class Day10
{
    public void Part1()
    {
        var machines = File.ReadAllLines("Day10/input.txt").Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).Select(x => new Machine
        {
            StartupPattern = x[0].TrimStart("[").TrimEnd("]").ToString(),
            Buttons = x[1..^1].Select(inst => new Button { FlippedLevers = inst.TrimStart("(").TrimEnd(")").ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)).ToList() }).ToList()
        });

        var buttonPresses = 0;

        foreach (var machine in machines)
        {
            int minPresses = GetMinNumberOfPresses(machine);

            buttonPresses += minPresses;
        }

        Console.WriteLine(buttonPresses);
    }

    public void Part2()
    {

        var machines = File.ReadAllLines("Day10/input.txt").Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).Select(x => new Machine
        {
            StartupPattern = x[0].TrimStart("[").TrimEnd("]").ToString(),
            Buttons = x[1..^1].Select(inst => new Button { FlippedLevers = inst.TrimStart("(").TrimEnd(")").ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)).ToList() }).ToList(),
            JoltageLevels = x[^1].TrimStart("{").TrimEnd("}").ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList()
        });

        long totalMinPresses = 0;

        foreach (var machine in machines)
        {
            // 161 durch Probieren herausgefunden
            int minPresses = GetMinNumberOfPresses(machine, 161);

            if (minPresses != -1)
            {
                Console.WriteLine($"Lösung für LGS gefunden: {minPresses}");
                totalMinPresses += minPresses;
            }
            else
            {
                Console.WriteLine("Keine Lösung für LGS gefunden, Suchbereich wahrscheinlich zu klein");
            }
        }

        Console.WriteLine($"Sum total presses: {totalMinPresses}");
    }

    private int GetMinNumberOfPresses(Machine machine)
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

        foreach (var instruction in machine.Buttons)
        {
            int buttonMask = 0;

            foreach (var lever in instruction.FlippedLevers)
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

    // Dem Mathebuch sein Dank
    // Jetzt reichts mir, ich will mit all dem nichts mehr zu tun haben -.-
    private int GetMinNumberOfPresses(Machine machine, int koeffSuchbereich)
    {
        // Beim Testen war double.Epsilon (5e-324) für die Präzision zu klein
        // Durch Probieren hab ich eine viel größere Zahl gefunden, mit der es funktioniert hat
        const double PrecisionTolerance = 1e-10;

        var zeilen = machine.JoltageLevels.Count;
        var spalten = machine.Buttons.Count;

        // Erweiterte Koeffizientenmatrix für Gauß-Jordan Algorithmus erstllen - sorry for Denglish aber sonst komme ich nicht klar
        // Form (A|b)
        var erwKoeffizientenMatrix = new double[zeilen, spalten + 1];
        
        for (int i = 0; i < zeilen; i++)
        {
            erwKoeffizientenMatrix[i, spalten] = machine.JoltageLevels[i];

            for (int j = 0; j < spalten; j++)
            {
                if (machine.Buttons[j].FlippedLevers.Contains(i))
                {
                    erwKoeffizientenMatrix[i, j] = 1.0;
                }
            }
        }

        var idx = 0;
        // -1 steht später für Parameter
        int[] numGleichungFuerLoesungBzwParameter = [.. Enumerable.Repeat(-1, spalten)];

        // Gauß-Jordan Algorithmus -> Eliminationsverfahren
        // Beginne mit erster Gleichung und bringe das ganze Ding Zeilenstufenform
        for (int j = 0; j < spalten && idx < zeilen; j++)
        {
            // Wähle nächste Gleichung
            int zeile = idx;
            for (int i = idx + 1; i < zeilen; i++)
            {
                if (Math.Abs(erwKoeffizientenMatrix[i, j]) > Math.Abs(erwKoeffizientenMatrix[zeile, j]))
                    zeile = i;
            }

            if (Math.Abs(erwKoeffizientenMatrix[zeile, j]) < PrecisionTolerance) continue;

            // Zeilen vertauschen
            if (zeile != idx)
            {
                for (int k = 0; k <= spalten; k++)
                {
                    (erwKoeffizientenMatrix[idx, k], erwKoeffizientenMatrix[zeile, k]) = (erwKoeffizientenMatrix[zeile, k], erwKoeffizientenMatrix[idx, k]);
                }
            }

            double aji = erwKoeffizientenMatrix[idx, j];
            for (int k = j; k <= spalten; k++)
            {
                erwKoeffizientenMatrix[idx, k] /= aji;
            }

            numGleichungFuerLoesungBzwParameter[j] = idx;

            for (int i = 0; i < zeilen; i++)
            {
                // Aktuell gewählte Zeile selbst nicht bearbeiten
                if (i != idx)
                {
                    // Eliminieren
                    double factor = erwKoeffizientenMatrix[i, j];
                    for (int k = j; k <= spalten; k++)
                    {
                        erwKoeffizientenMatrix[i, k] -= factor * erwKoeffizientenMatrix[idx, k];
                    }
                }
            }
            idx++;
        }

        // Bestimmung der bereits gefundenen Lösungen und der Parameter
        var spezielleLoesung = new double[spalten];
        List<int> parameters = [];

        for (int col = 0; col < spalten; col++)
        {
            // -1 -> freie Variable, sonst Wert der bestimmten Variable ablesbar
            if (numGleichungFuerLoesungBzwParameter[col] != -1)
            {
                spezielleLoesung[col] = erwKoeffizientenMatrix[numGleichungFuerLoesungBzwParameter[col], spalten];
            }
            else
            {
                parameters.Add(col);
            }
        }

        int anzParameter = parameters.Count;
        var kern = new double[anzParameter, spalten];

        // Kern der Matrix bestimmen, um Parameter für die Minimierung nutzen zu können
        for (int k = 0; k < anzParameter; k++)
        {
            int parameter = parameters[k];
            kern[k, parameter] = 1.0;

            for (int j = 0; j < spalten; j++)
            {
                if (numGleichungFuerLoesungBzwParameter[j] != -1)
                {
                    int r = numGleichungFuerLoesungBzwParameter[j];
                    kern[k, j] = -erwKoeffizientenMatrix[r, parameter];
                }
            }
        }

        Console.WriteLine($"Anzahl Parameter: {anzParameter}");
        Console.WriteLine($"Koeffizienten im Bereich [{-koeffSuchbereich}, {koeffSuchbereich}] probieren");

        long minimaleAnzKnopfdruecke = long.MaxValue;

        long linearkombinationen = 1;
        for (int k = 0; k < anzParameter; k++) linearkombinationen *= 2 * koeffSuchbereich + 1;

        // Durchprobieren der Kombinationen aus einem selber (durch Probieren) bestimmten Bereich
        for (long i = 0; i < linearkombinationen; i++)
        {
            List<int> koeffizienten = [];
            long tempck = i;

            // Koeffizienten für jeweiligen Parameter herausfinden
            for (int k = 0; k < anzParameter; k++)
            {
                int koeffizient = (int)(tempck % (2 * koeffSuchbereich + 1)) - koeffSuchbereich;
                koeffizienten.Add(koeffizient);
                tempck /= 2 * koeffSuchbereich + 1;
            }

            long knopfdrueckeGesamt = 0;
            var zureichend = true;

            for (int j = 0; j < spalten; j++)
            {
                // Variable x0, x1 ... xj
                double xj = spezielleLoesung[j];

                // Durchprobieren der Koeffizienten für die Parameter
                for (int ck = 0; ck < anzParameter; ck++)
                {
                    xj += koeffizienten[ck] * kern[ck, j];
                }

                // Ergebnis muss Ganzzahlig sein
                if (Math.Abs(xj - Math.Round(xj)) > PrecisionTolerance)
                {
                    zureichend = false;
                    break;
                }

                long anzKnopfdruecke = (long)Math.Round(xj);

                // Ergebnis muss größer als 0 sein
                if (anzKnopfdruecke < 0)
                {
                    zureichend = false;
                    break;
                }

                knopfdrueckeGesamt += anzKnopfdruecke;
            }

            if (zureichend)
            {
                minimaleAnzKnopfdruecke = Math.Min(minimaleAnzKnopfdruecke, knopfdrueckeGesamt);
            }
        }

        return (minimaleAnzKnopfdruecke == long.MaxValue) ? -1 : (int)minimaleAnzKnopfdruecke;
    }
}

public class Machine
{
    public string StartupPattern { get; set; }
    public List<Button> Buttons { get; set; }
    public List<int> JoltageLevels { get; set; }
}

public class Button
{
    public List<int> FlippedLevers { get; set; }
}
