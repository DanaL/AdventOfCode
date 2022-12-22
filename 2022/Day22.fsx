open System.IO
open System.Text.RegularExpressions

type Tile = Floor | Wall | Space

let toTile = function
    | '.' -> Floor
    | '#' -> Wall
    | ' ' -> Space
    | _ -> failwith "Hmm this shouldn't happen"
    
let parseMap (lines:string array) =
    let h = lines.Length
    let w = lines |> Array.map(fun l -> l.Length) |> Array.max
    let mutable map = Array2D.create h w Space

    lines |> Array.iteri(fun r l -> l |> Seq.iteri(fun c ch -> map[r,c] <- toTile ch))

    map

let parseMoves line =
    let turns = Regex.Split(line, "\d+")[1..line.Length-1]
    let steps = Regex.Split(line, "\D+") |> Array.map int
    
    printfn $"%A{steps.Length} {turns.Length}"
let lines = File.ReadAllLines("input_day22.txt")

let map = parseMap lines[0..lines.Length-2]
parseMoves (lines |> Array.last)

let start = 0, lines[0] |> Seq.findIndex(fun ch -> ch = '.')
printfn $"{start}"
