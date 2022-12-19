open System
open System.IO
open System.Text.RegularExpressions

type Valve = int*int*string[]

// get the valve id, flow rate and links from a line of the input file
let parse (line:string) =    
    let valve = line.Substring(6, 2)
    let flow = Regex.Match(line, "\d+").Value |> int

    // So grumpy about how they did the input file for this one :/
    let links = (line.Split("; ")[1]).Split(" ")[4..]
                |> Array.map(fun v -> v.Replace(",", ""))
                             
    valve, flow, links

// Floyd-Warshall to find the shortest paths between all valves,
// copied straight from the Wikipedia article. (I'll also eliminte all
// the valves with zero flow because we only ever travel through those
// rooms). 
let shortestPaths (valves:Map<string, Valve>) =
    let dim = valves.Count
    let mutable dist: int[,] = Array2D.create dim dim 9999

    // Dist to yourself is 0 of course
    for j in 0..dim-1 do
        dist[j,j] <- 0

    // Set direct neigbhours
    valves |> Map.keys
           |> Seq.iter(fun k -> let i,_,adj = valves[k]
                                adj |> Array.iter(fun s -> let j,_,_ = valves[s]
                                                           dist[i,j] <- 1))

    // update all the paths                                                        
    for k in 0 .. dim-1 do
        for i in 0 .. dim-1 do
            for j in 0 .. dim-1 do                
                if dist[i,j] > dist[i,k] + dist[k,j] then
                    dist[i,j] <- dist[i,k] + dist[k,j]
    dist
       
let valves = File.ReadAllLines("input_day16.txt")             
            |> Array.map parse
            |> Array.sortBy(fun (v, _, _) -> v)
            |> Array.mapi(fun i (v,f,l) -> v, (i,f,l))
            |> Map.ofArray

let dist = shortestPaths valves

// keys of valves with flow > 0
let kwf = valves.Keys |> Seq.filter(fun k -> let _,f,_ = valves[k]
                                             f > 0)
                      |> List.ofSeq
// Since I am skipping AA (it has flow 0) we have options for which
// node to start on.                       
let _,_,starts = valves["AA"]

let start = "DD"

let si, _, _ = valves[start]
let others = kwf |> List.map(fun ok -> let oi,_,_ = valves[ok]
                                       oi)
                 |> List.filter(fun oi -> oi <> si)
let time = others |> List.map(fun oi -> dist[si, oi] + 1)
                  |> List.sum
printfn $"{time}"

printfn $"%A{dist}"

