using System.Reflection.Metadata.Ecma335;
var input = File.ReadAllLines("in.txt");
var lines = input.Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToArray();

long total = 0;
for (int i = 0; i < lines[0].Length; i++)
{
  var column = lines.SkipLast(1).Select(l => l[i]);
  var columnNumbers = column.Select(long.Parse).ToArray();
  total += GetResult(columnNumbers, lines.Last()[i]);
}

long total2 = 0;
var currentNumbers = new List<string>();
for (int i = input[0].Length-1; i >= 0; i--)
{
  var column = input.Select(l => l[i]).ToArray();
  currentNumbers.Add(new string(column.SkipLast(1).ToArray()));
  if (column.Last() != ' ')
  { 
    total2 += GetResult(currentNumbers.Select(long.Parse), column.Last().ToString());
    currentNumbers.Clear();
    i--;
  }    
}

long GetResult(IEnumerable<long> values, string current_operator)
{
  bool multiply = current_operator == "*";
  return values.Aggregate(multiply ? 1l : 0l, (acc, next) => multiply ? acc * next : acc + next);
}

Console.WriteLine(total);
Console.WriteLine(total2);