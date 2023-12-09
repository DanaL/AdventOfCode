
static int NextTerm(IEnumerable<int> arr) 
{
    var reduced = arr.Zip(arr.Skip(1), (a, b) => b - a);
 
    if (reduced.Distinct().Count() == 1)
        return reduced.First();
    
    return reduced.Last() + NextTerm(reduced);
}

var arrs = File.ReadAllLines("input.txt")
               .Select(l => l.Trim().Split(' ').Select(n => int.Parse(n)).ToArray()).ToList();

Console.WriteLine($"P1: {arrs.Select(arr => arr.Last() + NextTerm(arr)).Sum()}");
Console.WriteLine($"P2: {arrs.Select(a => a.Reverse()).Select(arr => arr.Last() + NextTerm(arr)).Sum()}");