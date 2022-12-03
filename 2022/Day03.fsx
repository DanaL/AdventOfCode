open System.IO

let score = function
    | c when c <= 'Z' -> int c - int 'A' + 27
    | c -> int c - int 'a' + 1
    
let shared (line:string) =
    let a,b = line |> List.ofSeq |> List.splitAt (line.Length/2)
    score (Set.intersect (Set.ofSeq a) (Set.ofSeq b)
           |> Set.minElement)

let lines = File.ReadAllLines("input_day03.txt")   
let p1 = lines |> Array.map shared |> Array.sum
printfn $"P1 {p1}"

let badge grp =
    grp |> Array.map Set.ofSeq |> Set.intersectMany
        |> Set.minElement |> score
    
let p2 = lines |> Array.chunkBySize 3 |> Array.map badge |> Array.sum
printfn $"P2 {p2}"
