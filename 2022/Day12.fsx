open System.IO

let toInt ch =
    if ch = 'E' then -1
    else (ch |> int) - ('a' |> int)
    
let graph = File.ReadAllLines("input_day12.txt")
            |> Array.map(fun l -> l |> Seq.indexed |> Array.ofSeq)
            |> Array.mapi(fun r s -> s |> Array.map(fun (c, ch) -> (r, c), toInt ch))
            |> Array.concat |> Map.ofArray

let isVertex (graph:Map<int*int, int>) v p =
    match graph.ContainsKey(p) with
    | true -> if v+1 >= graph[p] then
                  Some (p)
              else None
    | false -> None
    
let adj (graph:Map<int*int, int>) (r, c) =
     [ -1, 0; 1,0; 0,-1; 0,1 ]
     |> List.choose (fun (dr, dc) -> let nr, nc = r+dr, c+dc
                                     let v = graph[r, c]
                                     isVertex graph v (nr,nc))
    
printfn $"%A{adj graph (0,2)}"
