//var boxes = File.ReadAllLines("in.txt").Select(l => l.Split(',').Select(long.Parse).ToArray()).ToList();
var boxes = File.ReadAllLines("in.txt").Select(l => l.Split(',').Select(long.Parse).ToArray()).Select(p => (p[0], p[1], p[2])).ToArray();
var distances = new List<(double, ((long, long, long), (long, long, long)))>();

List<HashSet<(long, long, long)>> circuits = new List<HashSet<(long, long, long)>>();

for (int i = 0; i < boxes.Length; i++)
  for (int j = i + 1; j < boxes.Length; j++)
    distances.Add((Distance(boxes[i], boxes[j]), (boxes[i], boxes[j])));

var orderedDistances = distances.OrderBy(d => d.Item1).ToList();

foreach (var od in orderedDistances)
{
  var b1 = od.Item2.Item1;
  var b2 = od.Item2.Item2;

  //Console.WriteLine(b1);
  //Console.WriteLine(b2);
  //Console.WriteLine("-----------------------");

  if (circuits.Any(c => c.Contains(b1) && c.Contains(b2)))
    continue; // both boxes are already in the same circuit

  var circuitsFound = circuits.Where(c => c.Contains(b1) || c.Contains(b2));
  var c = circuitsFound.FirstOrDefault();
  if (c != null)
  {
    c.Add(b1);
    c.Add(b2);

    if (c.Count == boxes.Length)
      Console.WriteLine(b1.Item1 * b2.Item1);
  }
  else
    circuits.Add(new HashSet<(long, long, long)> { b1, b2 });

  if (circuitsFound.Count() > 1) // merge
  {
    int before = c.Count;

    var toMerge = circuitsFound.Last();

    foreach (var other in toMerge)
      c.Add(other);

    if (c.Count == boxes.Length)
      Console.WriteLine(b1.Item1 * b2.Item1);

    circuits.Remove(toMerge);
  }
}

Console.WriteLine(circuits.OrderByDescending(c => c.Count).Take(3).Aggregate(1, (acc, c) => acc * c.Count));

double Distance((long, long, long) a, (long, long, long) b)
{
  return Math.Sqrt(Math.Pow(a.Item1 - b.Item1, 2)
                 + Math.Pow(a.Item2 - b.Item2, 2)
                 + Math.Pow(a.Item3 - b.Item3, 2));
}