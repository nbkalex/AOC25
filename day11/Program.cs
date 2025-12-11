var input = File.ReadAllLines("in.txt").Select(l => l.Split(": "))
                                       .ToDictionary(k => k[0], v => v[1].Split(" "));

Console.WriteLine(GetPaths("you", "out"));


var svr_dac = GetPaths("svr", "dac", "fft");
var dac_fft = GetPaths("dac", "fft");
var fft_out = GetPaths("fft", "out", "dac");

var svr_fft = GetPaths("svr", "fft", "dac");
var fft_dac = GetPaths("fft", "dac");
var dac_out = GetPaths("dac", "out", "fft");

Console.WriteLine(svr_dac * dac_fft * fft_out +
                  svr_fft * fft_dac * dac_out);


long GetPaths(string start, string end, string exclude = "")
{
  Dictionary<string, long> memo = new Dictionary<string, long>();

  List<string> paths = new();
  Stack<List<string>> pathStack = new();
  pathStack.Push(new List<string>() { start });

  while (pathStack.Any())
  {
    var current = pathStack.Pop();
    string id = current.Last();

    if (!input.ContainsKey(id))
    {
      memo[id] = 0;
      continue;
    }

    memo[id] = 0;

    List<string> toPush = new();
    foreach (var neighbor in input[id])
    {
      if (neighbor == exclude)
        continue;

      if (current.Contains(neighbor))
        continue;

      if (memo.ContainsKey(neighbor))
      {
        if (memo.ContainsKey(id))
          memo[id] += memo[neighbor];
        else
          memo[id] = memo[neighbor];

        continue;
      }

      if (neighbor == end)
      {
        memo[id] = 1;
        break;
      }

      toPush.Add(neighbor);
    }

    if (toPush.Any())
    {
      pathStack.Push(current);
      foreach (var n in toPush)
      {
        var newPath = current.ToList();
        newPath.Add(n);
        pathStack.Push(newPath);
      }
    }
  }

  return memo[start];
}

//1310733868618 too low