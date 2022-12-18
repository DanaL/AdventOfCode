open System.IO

let cubes = File.ReadAllLines("input_day18.txt")
            |> Array.map(fun l -> let p = l.Split(',')
                                  p[0]|>int, p[1]|>int, p[2]|>int)
            |> Set.ofArray

let neighbours = [ (-1,0,0); (1,0,0); (0,1,0); (0,-1,0);
                   (0,0,1); (0,0,-1) ]

let countOpen (cubes:Set<int*int*int>) (cx, cy, cz) =
    neighbours |> Seq.map(fun (x, y, z) -> cx+x, cy+y, cz+z)
               |> Seq.filter(fun c -> cubes |> Set.contains(c) |> not )
               |> Seq.length

let p1 = cubes |> Seq.map(fun c -> countOpen cubes c)
               |> Seq.sum
printfn $"P1: {p1}"               
