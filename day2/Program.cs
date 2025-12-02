using System.Collections.Generic;

var input = File.ReadAllText("in.txt");

var ids = input.Split(",").Select(tokens => tokens.Split("-").Select(id => long.Parse(id)).ToList());

long sum = 0;
long sum2 = 0;
foreach (var idPair in ids)
{
  for (long i = idPair[0]; i <= idPair[1]; i++)
  {
    if (isInvalid(i.ToString()))
      sum += i;

    if (isInvalid2(i.ToString()))
      sum2 += i;
  }
}

Console.WriteLine(sum);
Console.WriteLine(sum2);

bool isInvalid(string id)
{
  return id.Substring(0, id.Length / 2) == id.Substring(id.Length / 2);
}

bool isInvalid2(string id)
{
  if(id.Length == 1)
    return false;

  if(id.All(c=> c == id[0]))
    return true;

  for (int i = 2; i <= id.Length/2; i++)
  {
    if(id.Length % i != 0)
      continue;

    var chunks = Split(id, i);
    if(chunks.All(c => c == chunks.First()))
      return true;
  }

  return false;
}

IEnumerable<string> Split(string str, int chunkSize)
{
  return Enumerable.Range(0, str.Length / chunkSize)
      .Select(i => str.Substring(i * chunkSize, chunkSize));
}