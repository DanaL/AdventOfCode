open System.IO

let chSet (s:string) =
    s.ToCharArray() |> Set.ofArray
    
let shared (line:string) =
    let mid = line.Length/2 - 1
    let c = Set.intersect (chSet line[0..mid])  (chSet line[mid+1..])
            |> Set.minElement
    if c <= 'Z' then int c - int 'A' + 27 else int c - int 'a' + 1

let p1 = File.ReadAllLines("input_day03.txt") |> Array.map shared |> Array.sum
printfn $"P1 {p1}"
