using System.Collections;
using System.Collections.Generic;

var input = File.ReadAllLines("in.txt");
List<(int, int[], List<int[]>, int[])> diagrams = new();
foreach (var line in input)
{
  var indicator_rest = line.Split("] (");
  string indicatorToken = indicator_rest[0].Trim('[');
  var len = indicatorToken.Length;

  int indicator = Convert.ToInt32(indicatorToken.Replace('.', '0').Replace('#', '1'), 2);

  var buttons_joltage = indicator_rest[1].Split(") {");
  string buttonsToken = buttons_joltage[0];
  List<int> buttons = new();
  List<int[]> buttons2 = new();
  foreach (var button in buttonsToken.Split(") ("))
  {
    int buttonVal = 0;
    int buttonVal2 = 0;
    foreach (var bt in button.Split(',').Select(int.Parse))
    {
      buttonVal |= 1 << len - bt - 1;
      buttonVal2 |= 1 << bt;
    }
    buttons.Add(buttonVal);
    buttons2.Add(button.Split(',').Select(int.Parse).ToArray());
  }

  int[] joltages = buttons_joltage[1].Trim('}').Split(',').Select(int.Parse).ToArray();
  diagrams.Add((indicator, buttons.ToArray(), buttons2, joltages));
}

foreach (var (indicator, buttons, buttons2, joltages) in diagrams)
{
  Console.Write(indicator);
  Console.Write(" - ");
  Console.Write(string.Join(",", buttons));
  Console.Write(" - ");
  Console.Write(string.Join(",", joltages));
  Console.WriteLine();
}

Console.WriteLine();

long sum = 0;
long sum2 = 0;

foreach (var diagram in diagrams)
{
  var (indicator, buttons, buttons2, joltages) = diagram;

  // Part 1
  Queue<(int, List<int>)> q1 = new();
  q1.Enqueue((0, new List<int>()));
  int minSteps1 = int.MaxValue;
  while (q1.Any())
  {
    var (currentVal, currentButtons) = q1.Dequeue();
    if (currentButtons.Count >= minSteps1)
      continue;

    if (currentVal == indicator)
    {
      minSteps1 = Math.Min(minSteps1, currentButtons.Count);
      continue;
    }

    for (int i = 0; i < buttons.Length; i++)
    {
      if (currentButtons.Any() && buttons[i] == currentButtons.Last())
        continue;

      int nextVal = currentVal ^ buttons[i];
      if (nextVal != currentVal)
      {
        var nextButtons = new List<int>(currentButtons);
        nextButtons.Add(buttons[i]);
        q1.Enqueue((nextVal, nextButtons));
      }
    }
  }

  sum += minSteps1;

  // Part 2  
  var ordered = joltages.OrderBy(j => j).ToArray();
  List<(int[], int)> joltagesConfig = new(){(new int[joltages.Length], 0) };
  int min = int.MaxValue;
  foreach (var joltage in ordered)
  {
    int index = joltages.ToList().IndexOf(joltage);
    var availableButtons = buttons2.Where(b => b.Contains(index)).ToList();
    if(!availableButtons.Any())
      continue;

    var newJoltagesConfig = joltagesConfig.ToList();
    Stack<(List<(int[], int)>, int)> q2 = new();
    q2.Push((joltagesConfig, 0));

    while (q2.Any())
    {
      var current = q2.Pop();

      var currentConfigs = current.Item1;      
      int index2 = current.Item2;
      if(index2 == availableButtons.Count)
        continue;

      List<(int[], int)> newConfigs = new();
      foreach (var cfg in currentConfigs)
      {
        for (int i = 0; i <= joltage - cfg.Item1[index]; i++)
        {
          int newIndex= cfg.Item2 + i;
            if(newIndex >= min)
            break;

          bool stop = false;
          int[] newCfg = cfg.Item1.ToArray();
          foreach(var n in availableButtons[index2])
          { 
            newCfg[n] = newCfg[n] + i;
            if(newCfg[n] > joltages[n])
            {
              stop =true;
              break;
            }
          }

          if(stop)
            break;

          if (newCfg[index] == joltage)
          {
            if(newCfg.SequenceEqual(joltages))
              min = Math.Min(min, newIndex);

            if(newCfg.Zip(joltages).Any(c => c.First > c.Second))
              break;

            newJoltagesConfig.Add((newCfg, newIndex));
            continue;
          }

          newConfigs.Add((newCfg, cfg.Item2 + i));
        }
      }

      q2.Push((newConfigs, index2 + 1));
    }

    foreach (var button in availableButtons)
      buttons2.Remove(button);

    joltagesConfig = newJoltagesConfig;

    if (!buttons2.Any())
      break;
  }

  Console.WriteLine(min);
  sum2 += min;
}

Console.WriteLine(sum);
Console.WriteLine(sum2);
