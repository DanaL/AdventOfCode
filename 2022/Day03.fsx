open System.IO

let score = function
    | when c <= 'Z' -> c - int 'A' + 27
    | _ -> int c - int 'a' + 1
    
let shared (line:string) =
    let mid = line.Length/2 - 1
    score (Set.intersect (Set.ofSeq line[0..mid]) (Set.ofSeq line[mid+1..])
           |> Set.minElement)

let lines = File.ReadAllLines("input_day03.txt")   
let p1 = lines |> Array.map shared |> Array.sum
printfn $"P1 {p1}"

let badge (grp:string array) =
    grp |> Array.map Set.ofSeq |> Array.reduce Set.intersect
        |> Set.minElement |> score
    
let p2 = lines |> Array.chunkBySize 3 |> Array.map badge |> Array.sum
printfn $"P2 {p2}"
