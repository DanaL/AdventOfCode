open System.IO
open System.Text.RegularExpressions

type Tile = Stone | Sand

let parse line =
    Regex.Split(line, "\D+")
    |> Array.map int
    |> Array.chunkBySize 2
    |> Array.map(fun a -> a[0],a[1])
    |> Array.pairwise
    |> List.ofArray
    
let calcRock (a,b) =
    let calcDelta a b =
        if a = b then 0
        elif a > b then -1
        else 1
    let xa, ya = a
    let xb, yb = b
    let xd = calcDelta xa xb
    let yd = calcDelta ya yb

    let coords = seq { let mutable x,y = a
                       while (x,y) <> b do
                       yield x,y
                       x <- x + xd
                       y <- y + yd }
                 |> List.ofSeq
    coords @ [b]

let tiles =
    File.ReadAllLines("input_day14.txt")
    |> Array.map parse
    |> List.ofArray
    |> List.map(fun line -> line |> List.map calcRock |> List.concat |> List.distinct)
    |> List.concat |> List.distinct
    |> List.map(fun p -> p, Stone)
    |> Map.ofList

printfn $"%A{tiles}"
