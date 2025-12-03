using System.Linq;

var banks = File.ReadAllLines("in.txt").Select(bank => bank.Select(battery => battery - '0').ToList());

// Part 1 - 2 batteries per bank
int jolatege = 0;
foreach (var bank in banks)
{
  int max = bank.Max();
  int jolts = max != bank.Last()
    ? max * 10 + bank.Skip(bank.IndexOf(max) + 1).Max()
    : bank.SkipLast(1).Max() * 10 + max;
  
  jolatege += jolts;
}

//Console.WriteLine(jolatege);

// Part 2 - 12 batteries per bank
long jolatege2 = 0;
foreach (var bank in banks)
{
  var orderedBank = bank.OrderByDescending(b => b).ToList();
  
  long jolts = 0;
  int cursorPos = 0;
  for (long i = 12; i > 0; i--)
  {
    int found = orderedBank.First(battery => bank.Count - bank.IndexOf(battery, cursorPos) >= i);
    int cursorNextPos = bank.IndexOf(found, cursorPos) + 1;

    jolts = jolts * 10 + found;

    // remove batteries between cursor positions
    foreach (var rem in bank.Skip(cursorPos).Take(cursorNextPos - cursorPos))
      orderedBank.Remove(rem);

    // move cursor
    cursorPos = cursorNextPos;
  }

  jolatege2 += jolts;
}


//Console.WriteLine(jolatege2);

Console.WriteLine(banks.Sum(bank => FindMax(0, 2, bank)));
Console.WriteLine(banks.Sum(bank => FindMax(0, 12, bank)));


long FindMax(int paddingLeft, int paddingRight, List<int> bank2)
{
  var bank = bank2.Skip(paddingLeft).SkipLast(paddingRight-2).ToList();

  // find left value
  int maxLeft = bank.SkipLast(1).Max();
  int maxLeftIndex = bank.IndexOf(maxLeft)+1;
  int maxRight = bank.Skip(maxLeftIndex).Max();
  int maxRightIndex= bank.IndexOf(maxRight, maxLeftIndex)+1;

  int jolts = maxLeft * 10 + maxRight;

  return paddingRight > 2 
    ? jolts * (long)Math.Pow(10, paddingRight - 2) + FindMax(paddingLeft + maxRightIndex, paddingRight - 2, bank2)
    : jolts;
}