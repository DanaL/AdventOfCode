open System.Text.RegularExpressions

let parse line =
    Regex.Split(line, "\D+")
    |> Array.map int
    |> Array.chunkBySize 2
    |> Array.map(fun a -> a[0],a[1])
    |> Array.pairwise
    
let a = parse "503,4 -> 502,4 -> 502,9 -> 494,9"

printfn $"%A{a}"
