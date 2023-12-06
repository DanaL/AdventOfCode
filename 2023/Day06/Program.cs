
static ulong NumThatWin(ulong time, ulong distance)
{
    // I'm assuming there's a bell curve situation here and that 
    // the best result will be time/2 and then search up and down 
    // from there. (There's almost certainly simply arithmetic to 
    // determine this...)
    ulong wins = 0UL;

    ulong start = time/2UL;
    ulong d = 0UL;
    do 
    {
        d = start * (time - start);
        ++start;
        if (d > distance)
            ++wins;
        else
            break;
    } while (true);

    start = time/2UL - 1UL;
    d = 0UL;
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

var races = new List<(ulong, ulong)>() { (38, 234), (67, 1027), (76, 1157), (73, 1236) };

var p1 = races.Select(r => NumThatWin(r.Item1, r.Item2)).Aggregate(1UL, (x, y) => x * y);
Console.WriteLine($"P1: {p1}");

// Can...can I just brute force part 2??
Console.WriteLine($"P2: {NumThatWin(38_677_673, 234_102_711_571_236)}");
