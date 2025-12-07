using System.Drawing;

var map = File.ReadAllLines("in.txt").Select(l => l.ToArray()).ToArray();
int totalSplit = 0;

Dictionary<(int,int), long> memo = new();

for (int i = 1; i < map.Length; i++)
{
  for (int j = 0; j < map[i].Length; j++)
  {
    if (map[i - 1][j] == 'S' || map[i - 1][j] == '|')
    {
      if (map[i][j] == '.' || map[i][j] == '|')
      { 
        map[i][j] = '|';
        memo[(i,j)] = GetVal((i,j)) + GetVal((i-1, j));

        if(memo[(i, j)] == 0) memo[(i,j)] = 1; // start
      }

      if (map[i][j] == '^')
      {
        totalSplit++;
        memo[(i, j - 1)] = GetVal((i, j - 1)) + GetVal((i - 1, j));
        memo[(i, j + 1)] = GetVal((i, j + 1)) +  GetVal((i - 1, j));

        map[i][j - 1] = '|';         
        map[i][j + 1] = '|';
      }
    }
  }
}

Console.WriteLine(totalSplit);
Console.WriteLine(memo.Where(m => m.Key.Item1 == map.Length-1).Sum(m => m.Value));

long GetVal((int,int) point)
{
  long ret = 0;
  memo.TryGetValue(point, out ret);
  return ret;
}