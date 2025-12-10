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

  var buttons2Copy = buttons2.ToList();
  var orderedJoltages = joltages.OrderBy(j => j).ToArray();
  List<int[]> orderedButtons = new();

  foreach (var joltage in orderedJoltages)
  {
    int index = joltages.ToList().IndexOf(joltage);
    foreach (var button in buttons2)
    {
      if (button.Contains(index) && buttons2Copy.Contains(button))
      {
        orderedButtons.Add(button);
        buttons2Copy.Remove(button);
      }
    }
  }

  buttons2 = orderedButtons;

  // Part 2
  var q2 = new Stack<(List<int[]>, int[], List<int[]>)>();
  q2.Push((new List<int[]>(), new int[joltages.Length], new(buttons2)));
  var minSteps2 = int.MaxValue;
  while (q2.Any())
  {
    var (currentButtons, currentJoltages, availableButtons) = q2.Pop();
    if (currentButtons.Count > minSteps2)
      continue;

    if (currentJoltages.SequenceEqual(joltages))
    {
      minSteps2 = Math.Min(minSteps2, currentButtons.Count);
      continue;
    }

    foreach (int[] button in availableButtons)
    {
      if (currentButtons.Contains(button))
        continue;

      int multiplier = 1;
      List<(List<int[]>, int[], List<int[]>)> toAdd = new();
      while (true)
      {
        int[] newJoltages = currentJoltages.ToArray();

        List<int[]> newAvailableButtons = new(availableButtons);
        bool stop = false;
        foreach (var num in button)
        {
          newJoltages[num] = newJoltages[num] + multiplier;
          if (newJoltages[num] == joltages[num])
            newAvailableButtons.RemoveAll(b => b.Contains(num));

          if (newJoltages[num] > joltages[num])
          {
            newAvailableButtons.RemoveAll(b => b.Contains(num));
            stop = true;
            break;
          }
        }

        if (stop)
          break;

        List<int[]> newButtons = new(currentButtons);
        for (int i2 = 0; i2 < multiplier; i2++)
          newButtons.Add(button);

        toAdd.Add((newButtons, newJoltages, newAvailableButtons));

        multiplier++;
      }

      foreach (var item in toAdd)
        q2.Push(item);
    }
  }

  Console.WriteLine(minSteps2);

  sum2 += minSteps2;
}

Console.WriteLine(sum);
Console.WriteLine(sum2);
