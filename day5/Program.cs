using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;

var input = File.ReadAllText("in.txt").Split("\r\n\r\n");
var ranges = input[0].Split("\r\n").Select(line =>
{
  var parts = line.Split('-').Select(long.Parse).ToArray();
  return (parts[0], parts[1]);
}).Distinct().ToList();

var ids = input[1].Split("\r\n").Select(long.Parse);

Console.WriteLine(ids.Count(id => ranges.Any(r => id >= r.Item1 && id <= r.Item2)));


HashSet<(long, long)> mergedRanges = new(ranges);

while (true)
{
  HashSet<(long, long)> mergedRanges2 = new();
  HashSet<((long, long),(long,long))> visited = new();
  foreach (var r1 in mergedRanges)
  {
    foreach (var r2 in mergedRanges)
    {
      if(visited.Contains((r1,r2)))
        continue;

      visited.Add((r2, r1));

      var reunion = Reunion(r1, r2);
      mergedRanges2.Add(reunion.Item1);
    }
  }

  if(mergedRanges.SequenceEqual(mergedRanges2))
    break;

  mergedRanges = mergedRanges2;
}

Console.WriteLine(mergedRanges.Sum(r => r.Item2 - r.Item1 + 1));


(long, long) Intersect((long, long) r1, (long, long) r2)
{
  if (r1.Item2 < r2.Item1 || r2.Item2 < r1.Item1)
    return (0, 0);

  return (Math.Max(r1.Item1, r2.Item1), Math.Min(r1.Item2, r2.Item2));
}

((long, long), (long, long)) Reunion((long, long) r1, (long, long) r2)
{
  return Intersect(r1, r2) == (0, 0)
    ? (r1, r2)
    : ((Math.Min(r1.Item1, r2.Item1), Math.Max(r1.Item2, r2.Item2)), (0, 0));
}

//364736262280887 too high
//347083838819027 too low
