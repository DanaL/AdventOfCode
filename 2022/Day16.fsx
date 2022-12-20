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

// Get a unique # for a path taken via bit shifting based on its id
let pathNum (valves:Map<string, Valve>) (path:List<string*int>) =
    path |> List.skip(1)
         |> List.fold(fun a (p,_) -> let i,_,_ = valves[p]
                                     a ||| (1 <<< i)) 0
                                     
let pathScores = new System.Collections.Generic.Dictionary<int,int>()
let calcScore (valves:Map<string, Valve>) (path: List<string*int>) timeOut =
    let score =
        path |> List.map(fun (v,t) -> let _,flow,_ = valves[v]
                                      (timeOut - t) * flow)
             |> List.sum
    let pn = pathNum valves path
    if not(pathScores.ContainsKey(pn)) then
        pathScores.Add(pn, score)
    elif score > pathScores[pn] then
        pathScores[pn] <- score
        
let pathLen (dist:int[,]) (valves:Map<string, Valve>) a b =
    let ai,_,_ = valves[a]
    let bi,_,_ = valves[b]
    dist[ai,bi]

let rec pickPath (path: List<string*int>) (dist:int[,]) (valves:Map<string, Valve>) turn timeOut =    
    let curr,_ = path |> List.last    
    let visited = path |> List.map(fun (v,_) -> v) |> Set.ofList    
    let avail = Set.difference kwf visited
    
    calcScore valves path timeOut
        
    if avail.Count > 0 then
        for n in avail do                        
            let cost = (pathLen dist valves curr n) + 1
            if turn + cost < timeOut then
                pickPath (path @ [n, turn + cost]) dist valves (turn + cost) timeOut

pickPath ["AA", 0] dist valves 0 30
let p1 = pathScores.Values |> Seq.max
printfn $"P1: {p1}"

// For part 2, we've been tracking the best score for each path (including
// intermediary paths). So, we just need to find the two pairs of scores
// which produce the best result. (The key for pathScores is a bit-set
// of the node ids in the path so a bitwise AND selects items that
// have mutually exclusive nodes. Ie, 1001 and 0110)
pathScores.Clear()
pickPath ["AA", 0] dist valves 0 26

let p2 = seq {
             for a in pathScores.Keys do
                 for b in pathScores.Keys do
                     if a &&& b = 0 then
                         pathScores[a] + pathScores[b] }
         |> Seq.max                
printfn $"P2: {p2}"                

