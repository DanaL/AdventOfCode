
static int NextTerm(IEnumerable<int> arr) 
{
    var reduced = arr.Zip(arr.Skip(1), (a, b) => b - a);
 
    if (reduced.Distinct().Count() == 1)
        return reduced.First();
    
    return reduced.Last() + NextTerm(reduced);
}

var arrs = File.ReadAllLines("input.txt")
               .Select(l => l.Trim().Split(' ').Select(n => int.Parse(n)).ToArray()).ToList();

var p1 = arrs.Select(arr => arr.Last() + NextTerm(arr)).Sum();
Console.WriteLine($"P1: {p1}");

//var pairs = nums.Zip(nums.Skip(1), (a, b) => b - a).ToArray();
