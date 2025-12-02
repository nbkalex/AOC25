var moves = File.ReadAllLines("in.txt").Select(l => int.Parse(l.Substring(1)) * (l[0] == 'L' ? -1 : 1));

int currentPos = 50;
int counter = 0;
int counter2 = 0;
foreach (var move in moves)
{
  int nextPos = currentPos + move;
  if(nextPos % 100 == 0)
    counter++;

  if (nextPos <= 0 && currentPos != 0)
    counter2++;

  counter2 += Math.Abs(nextPos / 100);
  currentPos = (100 + nextPos % 100) % 100;
}

Console.WriteLine(counter);
Console.WriteLine(counter2);