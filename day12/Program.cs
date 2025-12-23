var input = File.ReadAllText("in.txt").Split("\r\n\r\n");

var presents = input.SkipLast(1).Select(chunk =>
{
  var chunkLines = chunk.Split("\r\n").Skip(1).ToArray();
  long[] presents = new long[chunkLines.Count()];
  for (int i = 0; i < chunkLines.Count(); i++)
  {
    int size = chunkLines[i].Length;
    for (int j = 0; j < size; j++)
      if (chunkLines[i][j] == '#')
        presents[i] += 1 << size - j - 1;
  }

  return presents;

}).ToArray();

var regions = input.Last().Split("\r\n").Select(line =>
{
  var parts = line.Split(": ");
  var size = parts[0].Split("x").Select(int.Parse).ToArray();
  var presents = parts[1].Split(" ").Select(int.Parse).ToArray();

  return ((size[0], size[1]), presents);
}).ToArray();

Console.WriteLine(regions.Count(CanPlace));


bool CanPlace(((int, int), int[]) region)
{
  long[] regionSurface = new long[region.Item1.Item2];
  int surfaceWidth = region.Item1.Item1;

  // rotation [0, 3]
  // present + rotation + position

  var presentsToArange = new List<long[]>(); // empty present

  for (int presentIndex = 0; presentIndex < region.Item2.Length; presentIndex++)
  {
    int presentCount = region.Item2[presentIndex];
    for (int i = 0; i < presentCount; i++)
      presentsToArange.Add(presents[presentIndex]);
  }

  // surface, presentIndex, x, y
  Stack<(long[], int)> stack = new();
  stack.Push((regionSurface, 0));

  HashSet<string> invalidConfigs = new HashSet<string>();

  while (stack.Any())
  {
    var current = stack.Pop();

    var currentPresent = presentsToArange[current.Item2];
    var currentSurface = current.Item1;

    //Print(currentSurface, surfaceWidth);

    //Print(currentSurface, surfaceWidth);

    //Console.WriteLine($"Placing present {current.Item2}");
    for (int ir = 0; ir < 4; ir++)
    {
      if(!invalidConfigs.Add(GetHashPresent(currentSurface, currentPresent)))
        continue;

      for (int i = 0; i < surfaceWidth-2; i++)
        for (int j = 0; j < regionSurface.Length-2; j++)
        {
          if (!IsPositionEmpty(currentSurface, surfaceWidth, i, j))
            continue;

          long[] newSurface = PlacePresent(currentSurface, surfaceWidth, currentPresent, i, j);
          
          if (newSurface != null)
          {
            stack.Push((newSurface, current.Item2 + 1));
            if (current.Item2 == presentsToArange.Count - 1)
            {
              Console.WriteLine("Found Solution");
              return true;
            }
          }
        }

      currentPresent = RotatePresent(currentPresent);
    }
  }

  Console.WriteLine("Not Found");
  return false;
}

long[]? PlacePresent(long[] regionSurface, int surfaceWidth, long[] present, int x, int y)
{
  var copy = regionSurface.ToArray();
  for (int i = y; i < y + 3; i++)
  {
    long presentLine = present[i - y] << (surfaceWidth - x - 3);
    copy[i] ^= presentLine;
    if ((copy[i] & presentLine) != presentLine)
      return null;
  }

  return copy;
}

string GetHash(long[] cfg)
{
  return cfg.Aggregate("", (acc, l) => acc+l);
}

string GetHashPresent(long[] surface, long[] present)
{
  return GetHash(surface) + GetHash(present);
}

void Print(long[] surface, int surfaceWidth)
{
  Console.ReadKey();
  Console.Clear();

  for (int i = 0; i < surface.Length; i++)
  {
    for (int j = surfaceWidth - 1; j >= 0; j--)
      Console.Write(((surface[i] >> j) & 1) == 1 ? '#' : '.');
    Console.WriteLine();
  }

  Console.WriteLine();
}

bool IsPositionEmpty(long[] surface, int surfaceWidth, int x, int y)
{
  return (surface[y] >> (surfaceWidth - x - 1) & 1) == 0;
}

long[] RotatePresent(long[] present)
{
  long[] rotated = new long[3];
  for (int i = 0; i < 3; i++)
  {
    for (int j = 0; j < 3; j++)
    {
      if (((present[i] >> j) & 1) == 1)
        rotated[j] |= 1 << (3 - i - 1);
    }
  }
  return rotated;
}