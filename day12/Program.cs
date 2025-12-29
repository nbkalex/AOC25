using System.Drawing;

var input = File.ReadAllText("in.txt").Split("\r\n\r\n");

var presents = input.SkipLast(1).Select(chunk => chunk.Split("\r\n").Skip(1)).ToArray();

var regions = input.Last().Split("\r\n").Select(line =>
{
  var parts = line.Split(": ");
  var size = parts[0].Split("x").Select(int.Parse).ToArray();
  var presents = parts[1].Split(" ").Select(int.Parse).ToArray();

  return ((size[0], size[1]), presents);
}).ToArray();

Console.WriteLine(regions.Count(CanPlace));

bool CanPlace(((int width, int height) size, int[] presents) region)
{
  int w = region.size.width;
  int h = region.size.height;

  int presentArea = 0;
  for (int i = 0; i < region.presents.Length; i++)
    presentArea += presents[i].Sum(p => p.Count(c => c=='#')) * region.presents[i];

  return presentArea <= w * h;
}