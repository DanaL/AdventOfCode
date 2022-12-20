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
                      |> Set.ofSeq

let mutable mostFlow = 0
let calcScore (valves:Map<string, Valve>) (path: List<string*int>) =
    let score =
        path |> List.map(fun (v,t) -> let _,flow,_ = valves[v]
                                      (30 - t) * flow)
             |> List.sum                                   
    if score > mostFlow then        
        mostFlow <- score
        
let pathLen (dist:int[,]) (valves:Map<string, Valve>) a b =
    let ai,_,_ = valves[a]
    let bi,_,_ = valves[b]
    dist[ai,bi]

let rec pickPath (path: List<string*int>) (dist:int[,]) (valves:Map<string, Valve>) turn =    
    let curr,_ = path |> List.last    
    let visited = path |> List.map(fun (v,_) -> v) |> Set.ofList    
    let avail = Set.difference kwf visited

    if avail.Count = 0 then
        calcScore valves path
    else
        for n in avail do                        
            let cost = (pathLen dist valves curr n) + 1
            if turn + cost > 30 then
                calcScore valves path
            else
                pickPath (path @ [n, turn + cost]) dist valves (turn + cost)

pickPath ["AA", 0] dist valves 0  

printfn $"P1: {mostFlow}"
