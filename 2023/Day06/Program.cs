
static int NumThatWin(int time, int distance)
{
    // I'm assuming there's a bell curve situation here and that 
    // the best result will be time/2 and then search up and down 
    // from there. (There's almost certainly simply arithmetic to 
    // determine this...)
    var wins = 0;

    var start = time/2;
    var d = 0;
    do 
    {
        d = start * (time - start);
        ++start;
        if (d > distance)
            ++wins;
        else
            break;
    } while (true);

    start = time/2 - 1 ;
    d = 0;
    do 
    {
        d = start * (time - start);
        --start;
        if (d > distance)
            ++wins;
        else
            break;
    } while (true);

    return wins;
}

// var races = new List<(int, int)>() { (7, 9), (15, 40), (30, 200) };
var races = new List<(int, int)>() { (38, 234), (67, 1027), (76, 1157), (73, 1236) };

var p1 = races.Select(r => NumThatWin(r.Item1, r.Item2)).Aggregate(1, (x, y) => x * y);
Console.WriteLine($"P1: {p1}");
