
Console.SetBufferSize(1000, 1000);

var tiles = File.ReadAllLines("in.txt").Select(l => l.Split(",")).Select(l => (long.Parse(l[0]), long.Parse(l[1]))).ToArray();

long maxArea = 0;
long maxArea2 = 0;

HashSet<(long,long)> sides = new HashSet<(long, long)>();
for (int i = 0; i < tiles.Length; i++)
{
  for (int j = i + 1; j < tiles.Length; j++)
  {
    if (tiles[i].Item1 == tiles[j].Item1)
      for(long k = Math.Min(tiles[i].Item2, tiles[j].Item2); k <= Math.Max(tiles[i].Item2, tiles[j].Item2); k++)
        sides.Add((tiles[i].Item1, k));

    if (tiles[i].Item2 == tiles[j].Item2)
      for (long k = Math.Min(tiles[i].Item1, tiles[j].Item1); k <= Math.Max(tiles[i].Item1, tiles[j].Item1); k++)
        sides.Add((k, tiles[i].Item2));
  }
}

for (int i = 0; i < tiles.Length; i++)
{
  for (int j = i + 1; j < tiles.Length; j++)
  {
    long l = Math.Min(tiles[i].Item1, tiles[j].Item1);
    long r = Math.Max(tiles[i].Item1, tiles[j].Item1);
    long t = Math.Min(tiles[i].Item2, tiles[j].Item2);
    long b = Math.Max(tiles[i].Item2, tiles[j].Item2);

    long area = (r - l + 1) * (b - t + 1);
    if (area > maxArea)
      maxArea = area;

    if(area < maxArea2)
      continue;

    bool hasTileInside = sides.Any(m => IsInside(m, l, r, t, b));

    if (!hasTileInside)
    {
      if (area > maxArea2)
        maxArea2 = area;
    }
  }
}

Console.WriteLine(maxArea);
Console.WriteLine(maxArea2);

bool IsBetween(long val, long l, long r)
{
  return val > l && val < r;
}

bool IsInside((long, long) point, long l, long r, long top, long bot)
{
  return IsBetween(point.Item1, l, r) && IsBetween(point.Item2, top, bot);
}
