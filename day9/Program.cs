
Console.SetBufferSize(1000, 1000);

var tiles = File.ReadAllLines("in.txt").Select(l => l.Split(",")).Select(l => (long.Parse(l[0]), long.Parse(l[1]))).ToArray();

long maxArea = 0;
long maxArea2 = 0;

List<((long,long), (long, long))> sides = new List<((long, long), (long, long))>();
for (int i = 0; i < tiles.Length; i++)
{
  for (int j = i + 1; j < tiles.Length; j++)
  {
    if (tiles[i].Item1 == tiles[j].Item1 || tiles[i].Item2 == tiles[j].Item2)
      sides.Add((tiles[i], tiles[j]));
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

    bool hasTileInside = sides.Any(m => SideIsInside(m, l, r, t, b));

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

bool SideIsInside(((long, long), (long, long)) side, long l, long r, long top, long bot)
{
  bool isInside = IsInside(side.Item1, l,r,top,bot) || IsInside(side.Item2, l, r, top, bot);
  if (isInside)
    return true;

  bool crossesVertically = side.Item1.Item1 == side.Item2.Item1;
  if(crossesVertically)
    return IsBetween(side.Item1.Item1, l, r) 
        && Math.Min(side.Item1.Item2, side.Item2.Item2) <= top 
        && Math.Max(side.Item1.Item2, side.Item2.Item2) >= bot;

  bool crossesHorizontally = side.Item1.Item2 == side.Item2.Item2;
  if(crossesHorizontally)
    return IsBetween(side.Item1.Item2, top, bot) 
        && Math.Min(side.Item1.Item1, side.Item2.Item1) <= l 
        && Math.Max(side.Item1.Item1, side.Item2.Item1) >= r;

  return false;
}

