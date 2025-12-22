using Microsoft.Z3;
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
  Context z3 = new();
  var solver = z3.MkOptimize();
  var ctx = solver.Context;

  List<ArithExpr> btnVars = new();
  for (int i = 0; i < buttons2.Count; i++)
  { 
    btnVars.Add(ctx.MkConst("btn" + i, ctx.MkIntSort()) as ArithExpr);
    solver.Assert(btnVars.Last() >= 0);
  }

  var sumExpr = ctx.MkAdd(btnVars);

  for (int ji = 0; ji < joltages.Length; ji++)
  {
    List<ArithExpr> eqCoefs = new();
    for (int i = 0; i < buttons2.Count; i++)
    {
      if (buttons2[i].Contains(ji))
        eqCoefs.Add(ctx.MkAdd(btnVars[i]));
    }

    solver.Assert(ctx.MkEq(ctx.MkAdd(eqCoefs), z3.MkInt(joltages[ji])));
  }

  var minSum = solver.MkMinimize(sumExpr);
  var status = solver.Check();

  var result = solver.Model.Evaluate(sumExpr);

  
  sum2 += ((IntNum)result).Int;
}

Console.WriteLine(sum);
Console.WriteLine(sum2);

// 20644 too high
// 20684