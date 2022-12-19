open System
open System.Collections.Generic
open System.IO

let cubes = File.ReadAllLines("input_day18.txt")
            |> Array.map(fun l -> let p = l.Split(',')
                                  p[0]|>int, p[1]|>int, p[2]|>int)
            |> Set.ofArray

let adj = [ (-1,0,0); (1,0,0); (0,1,0); (0,-1,0); (0,0,1); (0,0,-1) ]
let neighbours (cx, cy, cz) = adj |> List.map(fun (x,y,z) -> cx+x,cy+y,cz+z)

let openSqs (cubes:Set<int*int*int>) (cx, cy, cz) =
    adj |> List.map(fun (x, y, z) -> cx+x, cy+y, cz+z)
        |> List.filter(fun c -> cubes |> Set.contains(c) |> not)
    
let part1 = 
    let p1 = cubes |> Seq.map(fun c -> openSqs cubes c)
                   |> Seq.map(fun a -> a |> Seq.length)
                   |> Seq.sum
    printfn $"P1: {p1}"               

let minX = (cubes |> Seq.map(fun (x,_,_) -> x) |> Seq.min) - 1
let maxX = (cubes |> Seq.map(fun (x,_,_) -> x) |> Seq.max) + 1
let minY = (cubes |> Seq.map(fun (_,y,_) -> y) |> Seq.min) - 1
let maxY = (cubes |> Seq.map(fun (_,y,_) -> y) |> Seq.max) + 1
let minZ = (cubes |> Seq.map(fun (_,_,z) -> z) |> Seq.min) - 1
let maxZ = (cubes |> Seq.map(fun (_,_,z) -> z) |> Seq.max) + 1

let outside x y z =
    x <= minX || x >= maxX || y <= minY || y >= maxY
        || z <= minZ || z >= maxZ
        
// Okay, quick n' dirty floodfill. Essentially start with wit an open
// space and keep checking its neihgbours until I run out or hit an
// out of bounds 
let exterior (cubes:Set<int*int*int>) c =    
    let q = new Queue<int*int*int>()
    let visited = new HashSet<int*int*int>()
    q.Enqueue(c)
    let mutable ext = false
    
    while q.Count > 0 do
        let e = q.Dequeue()
        if not <| visited.Contains(e) then            
            ignore(visited.Add(e))
            let ns = neighbours e
            let outside = ns |> List.filter(fun (x,y,z) -> outside x y z)
            if outside.Length > 0 then
                ext <- true
                q.Clear()
            else
                ns |> List.filter(fun n -> visited.Contains(n) |> not)
                   |> List.filter(fun n -> cubes |> Set.contains n |> not)
                   |> List.iter(fun n -> q.Enqueue(n))

    ext
    
// Okay, part 2: get set of all open, adj squares and then divide
// them into disjoint sets
let part2 = 
    let empty = cubes |> Seq.map(fun c -> openSqs cubes c)
                      |> Seq.concat |> Seq.distinct
                      |> Set.ofSeq
    
    let p2 = empty |> Seq.filter(fun e -> exterior cubes e)
                   |> Seq.map(fun c -> neighbours c
                                       |> List.filter(fun n -> cubes |> Set.contains n)
                                       |> List.length)
                   |> Seq.sum
    printfn $"P2: {p2}"
    
