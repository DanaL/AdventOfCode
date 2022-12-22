open System.IO
open System.Text.RegularExpressions

type Tile = Floor | Wall | Space

let toTile = function
    | '.' -> Floor
    | '#' -> Wall
    | ' ' -> Space
    | _ -> failwith "Hmm this shouldn't happen"

let dump (map:Tile[,]) pr pc =
    let h = map |> Array2D.length1
    let w = map |> Array2D.length2

    for r in 0..h-1 do
        let mutable s = ""
        for c in 0..w-1 do
            if r = pr && c = pc then s <- s + "@"
            else
                s <- s + match map[r,c] with
                         | Space -> "*"
                         | Wall -> "#"
                         | Floor -> "."
        printfn $"{s}"
        
let parseMap (lines:string array) =
    let h = lines.Length
    let w = lines |> Array.map(fun l -> l.Length) |> Array.max
    let mutable map = Array2D.create h w Space

    lines |> Array.iteri(fun r l -> l |> Seq.iteri(fun c ch -> map[r,c] <- toTile ch))

    map

let parseMoves line =
    let turns = Regex.Split(line, "\d+")[1..line.Length-1]
    let steps = Regex.Split(line, "\D+") |> Array.map int

    Array.zip steps turns

let rec nextSq (map:Tile[,]) r c dr dc =
    let h = map |> Array2D.length1
    let w = map |> Array2D.length2
    let r' = if r+dr < 0 then h + r + dr
             else (r+dr) % h
    let c' = if c+dc < 0 then w + c + dc
             else (c+dc) % w

    match map[r',c'] with
    | Wall -> r,c
    | Floor -> r',c'
    | Space -> let tr,tc = nextSq map r' c' dr dc
               if map[tr,tc] = Space then r,c
               else tr,tc

// This took me so long to debug T_T
let turn d r c =
    match d with
    | "R" when r = -1 && c = 0  -> 0,1
    | "R" when r = 1  && c = 0  -> 0,-1
    | "R" when r = 0  && c = -1 -> -1,0
    | "R" when r = 0  && c = 1  -> 1,0
    | "L" when c > 0 -> -1,0
    | "L" -> -c,r
    | _ -> failwith $"Hmm this shouldn't happen {r} {c}"

let rec move (map:Tile[,]) (actions:(int*string)[]) r c  dr dc i =
    if i < (Array.length actions) then
        let steps, t = actions[i]              
        let r', c' = seq { 1..steps } |> Seq.fold(fun(r,c) _ -> nextSq map r c dr dc) (r,c)
        let dr', dc' = if t = "" then dr,dc
                       else turn t dr dc
        move map actions r' c' dr' dc' (i+1)
    else
        r, c, dr, dc        

let dirToN dr dc =
    match dr, dc with
    | 0, 1  -> 0
    | 0, -1 -> 2
    | 1, 0  -> 1
    | -1, 0 -> 3
    | _ -> failwith "Hmmm this should happen"
    
let lines = File.ReadAllLines("input_day22_2.txt")
let map = parseMap lines[0..lines.Length-3]
let actions = parseMoves (lines |> Array.last)
let sr,sc = 0, lines[1] |> Seq.findIndex(fun ch -> ch = '.')

let r,c,dr,dc = move map actions sr sc 0 1 0
let p1 = 1000 * (r+1) + 4 * (c+1) + (dirToN dr dc)
printfn $"P1: {p1}"

