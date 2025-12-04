using System.Drawing;

var input = File.ReadAllLines("in.txt");
var map = input.Select((line, lineIndex) => line.Select((p, pIndex) => (new Point(pIndex, lineIndex), p)))
               .Aggregate(new List<(Point, char)>(), (acc, line) => { acc.AddRange(line); return acc; })
               .ToDictionary(k => k.Item1, v => v.Item2);


Console.WriteLine(GetRemovablePaper().Count());

int totalPaper = 0;
while(true)
{
  var removablePaper = GetRemovablePaper().ToList();
  if(!removablePaper.Any())
    break;

  foreach (var paper in removablePaper)
    map[paper.Key] = '.';

  totalPaper += removablePaper.Count();
}

Console.WriteLine(totalPaper);


IEnumerable<KeyValuePair<Point, char>> GetRemovablePaper()
{
  return map.Where(paper => paper.Value == '@' && GetNeighborPapers(paper.Key).Count() < 4);
}

IEnumerable<Point> GetNeighborPapers(Point p)
{
  var allNeighbours = new List<Point>
    {
        new Point(p.X + 1, p.Y),
        new Point(p.X - 1, p.Y),
        new Point(p.X, p.Y + 1),
        new Point(p.X, p.Y - 1),
        new Point(p.X + 1, p.Y + 1),
        new Point(p.X - 1, p.Y - 1),
        new Point(p.X + 1, p.Y - 1),
        new Point(p.X - 1, p.Y + 1)
    };

  return allNeighbours.Where(n => map.ContainsKey(n) && map[n] == '@');
}