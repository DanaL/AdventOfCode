open System
open System.Collections.Generic
open System.IO

type Graph = Map<int*int, int>

let toInt ch =
    match ch with
    | 'S' -> -1
    | 'E' -> 26
    | _ -> (ch |> int) - ('a' |> int)
    
let graph = File.ReadAllLines("input_day12.txt")
            |> Array.map(fun l -> l |> Seq.indexed |> Array.ofSeq)
            |> Array.mapi(fun r s -> s |> Array.map(fun (c, ch) -> (r, c), toInt ch))
            |> Array.concat |> Map.ofArray

// For part 2, I want to work in reverse. So to find adjacent squares,
// I need to see, for whichever sq I am looking it, if it was adjacent
// to its neighbour in part 1. (Because the graph edges aren't
// necessarily bidirectional)

let isVertex (graph:Graph) curr neighbour =
    match graph.ContainsKey(neighbour) with
    | true -> if graph[neighbour] + 1 >= curr then Some(neighbour)
              else None
    | false -> None
    
let adj (graph:Graph) (r, c) =
     [ -1, 0; 1,0; 0,-1; 0,1 ]
     |> List.choose (fun (dr, dc) -> let nr, nc = r+dr, c+dc
                                     let v = graph[r, c]
                                     isVertex graph v (nr,nc))

let shortestPath (graph:Graph) start goalElevation =
    let distances = new Dictionary<int*int, int>()
    let keys = graph.Keys |> List.ofSeq
    keys |> List.iter(fun k -> distances.Add(k, Int32.MaxValue))
    distances[start] <- 0
    let visited = new HashSet<int*int>()

    let mutable goal = start
    let mutable cont = true
    let mutable v = start
    while cont do
        if graph[v] = goalElevation then
            goal <- v
            cont <- false
        else            
            ignore(visited.Add(v))
            adj graph v |> List.iter(fun c -> if distances[c] > distances[v] + 1 then
                                                  distances[c] <- distances[v] + 1)
            let mutable dist = Int32.MaxValue
            keys |> List.iter(fun j -> if not <| visited.Contains(j) && dist > distances[j] then
                                           dist <- distances[j]
                                           v <- j)
    
    distances[goal]

let start = graph.Keys |> Seq.find(fun k -> graph[k] = -1)
let peak = graph.Keys |> Seq.find(fun k -> graph[k] = 26)

let p1 = shortestPath graph peak -1
printfn $"P1: {p1}"

let p2 = shortestPath graph peak 0
printfn $"P2: {p2}"
