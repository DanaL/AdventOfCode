open System.IO
open System.Text.RegularExpressions

exception Finished

type Tile = Stone | Sand
type Tiles = Map<int*int,Tile>
    
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

let coords =
    File.ReadAllLines("input_day14.txt")
    |> Array.map parse
    |> List.ofArray
    |> List.map(fun line -> line |> List.map calcRock |> List.concat |> List.distinct)
    |> List.concat |> List.distinct
    
let maxY = (coords |> List.map (fun (_,y) -> y) |> List.max) + 2

let dump (tiles:Tiles) =    
    for r in 0..maxY do
        let mutable s = ""
        for c in 490..510 do            
            s <- s + if not(tiles |> Map.containsKey (c,r)) then "."
                     else if tiles[c,r] = Stone then "#"
                     else "o"
        printfn $"{s}"
    printfn ""

let caveFull (tiles:Tiles) =
    (tiles |> Map.containsKey (500, 1))
    && (tiles |> Map.containsKey (499, 1))
    && (tiles |> Map.containsKey (501, 1))
    
let rec drop (tiles:Tiles) x y part1 =
    if (part1 && y = maxY) || caveFull tiles then
        raise Finished
    elif (not part1) && y+1 = maxY then
        x,y
    elif not (tiles |> Map.containsKey (x, y+1)) then
        drop tiles x (y+1) part1
    elif not (tiles |> Map.containsKey (x-1, y+1)) then
        drop tiles (x-1) (y+1) part1
    elif not (tiles |> Map.containsKey (x+1, y+1)) then
        drop tiles (x+1) (y+1) part1
    else
        x,y

let find tiles part1 =
    try
        let r = drop tiles 500 0 part1
        let tiles' = tiles |> Map.add r Sand
        Some(tiles', tiles')
    with
    | :? Finished -> None
    
let run part1 =
    coords |> List.map(fun p -> p, Stone) |> Map.ofList                       
           |> List.unfold(fun tiles -> find tiles part1)                                                
           |> List.last |> Map.values
           |> Seq.filter(fun t -> t = Sand) |> Seq.length
    
printfn $"P1: {run true}"
printfn $"P2: {(run false) + 1}"
