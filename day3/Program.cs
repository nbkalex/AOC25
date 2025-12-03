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

Console.WriteLine(jolatege);

Console.WriteLine("----------------------------------------------------------------------");

// Part 2 - 12 batteries per bank
long jolatege2 = 0;
foreach (var bank in banks)
{
  var orderedBank = bank.OrderByDescending(b => b).ToList();
  
  long jolts = 0;
  int lastIndex = 0;
  for (long i = 12; i > 0; i--)
  {
    int found = orderedBank.First(battery => bank.IndexOf(battery, lastIndex) != -1 && bank.Count - bank.IndexOf(battery, lastIndex) >= i);
    jolts = jolts * 10 + found;
    orderedBank.Remove(found);
    lastIndex = bank.IndexOf(found, lastIndex)+1;
  }

  Console.WriteLine(jolts);

  jolatege2 += jolts;
}

Console.WriteLine(jolatege2);

// 174263868434233 TOO HIGH