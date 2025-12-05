using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;

var input = File.ReadAllText("in.txt").Split("\r\n\r\n");
var ranges = input[0].Split("\r\n").Select(line =>
{
  var parts = line.Split('-').Select(long.Parse).ToArray();
  return (parts[0], parts[1]);
}).Distinct().OrderBy(r => r.Item1).ToList();

var ids = input[1].Split("\r\n").Select(long.Parse);

Console.WriteLine(ids.Count(id => ranges.Any(r => id >= r.Item1 && id <= r.Item2)));

for(int i = 0; i < ranges.Count - 1; i++)
{
  var reunion = Reunion(ranges[i], ranges[i + 1]);
  if (reunion != (0, 0))
  {
    ranges[i] = reunion;
    ranges.RemoveAt(i + 1);
    i--;
  }
}

Console.WriteLine(ranges.Sum(r => r.Item2 - r.Item1 + 1));


(long, long) Reunion((long, long) r1, (long, long) r2)
{
  return r1.Item2 < r2.Item1 || r2.Item2 < r1.Item1
    ? (0, 0)
    : (Math.Min(r1.Item1, r2.Item1), Math.Max(r1.Item2, r2.Item2));
}
