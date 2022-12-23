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

let rec nextSqPt1 (map:Tile[,]) r c dr dc =
    let h = map |> Array2D.length1
    let w = map |> Array2D.length2
    let r' = if r+dr < 0 then h + r + dr
             else (r+dr) % h
    let c' = if c+dc < 0 then w + c + dc
             else (c+dc) % w

    match map[r',c'] with
    | Wall -> r,c,dr,dc
    | Floor -> r',c',dr,dc
    | Space -> let tr,tc,_,_ = nextSqPt1 map r' c' dr dc
               if map[tr,tc] = Space then r,c,dr,dc
               else tr,tc,dr,dc

// A cursed function...
let nextSqPt2 (map:Tile[,]) r c dr dc =
    let h = map |> Array2D.length1
    let w = map |> Array2D.length2
    let r' = r+dr
    let c' = c+dc

    let fr,fc,dr',dc' =
                 if r' = -1 && c >= 100 then
                     // top edge of Panel 1
                     199, c'-100, -1, 0
                 elif c' = 150 then
                     // right edge of Panel 1
                     149 - r', 99, 0, -1
                 elif r' = 50 && c' >= 100 && dr = 1 then
                     // bottom edge of Panel 1
                     c'-50, 99, 0, -1
                 elif c' = 49 && r' < 50 then
                     // left edge of Panel 2
                     149-r', 0, 0, 1
                 elif r' = -1 && c' < 100 then
                     // top edge of Panel 2
                     100+c', 0, 0, 1
                 elif c' = 49 && r' < 100 && dc = -1 then
                     // left edge of Panel 3
                     100, r' - 50, 1, 0
                 elif c' = 100 && r' >= 50 && r' <= 99 then
                     // right edge of Panel 3
                     49, r' + 50, -1, 0
                 elif c' = 100 && r' >= 100 && r' <= 149 then
                     // right edge of Panel 4
                     149 - r', 149, 0, -1
                 elif r' = 150 && c' >= 50 && dr = 1 then
                     // bottom edge of panel 4
                     c' + 100, 49, 0, -1
                 elif r' = 99 && c' <= 49 && dr = -1 then
                     // top edge of panel 5
                     c' + 50, 50, 0, 1
                 elif c' = -1 && r' < 150 then
                     // left edge of panel 5
                     149 - r', 50, 0, 1
                 elif r' = 200 then
                     // bottom edge of panel 6
                     0, c' + 100, 1, 0
                 elif c' = -1 && r' >= 150 then
                     // left edge of panel 6
                     0, r' - 100, 1, 0
                 elif c' = 50 && r' >= 150 && dc = 1 then
                     // right edge of panel 6
                     149, r' - 100, -1, 0
                 else
                     r', c',dr,dc
                     
    match map[fr,fc] with
    | Wall -> r,c,dr,dc
    | Floor -> fr,fc,dr',dc'
    | Space -> failwith "Hmm should never land on a space in Part 2"
    
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

let rec move (map:Tile[,]) (actions:(int*string)[]) r c  dr dc i f =
    if i < (Array.length actions) then
        let steps, t = actions[i]              
        let r', c', dr', dc' = seq { 1..steps }
                               |> Seq.fold(fun(r,c,dr,dc) _ -> f map r c dr dc) (r,c,dr,dc)
        let dr'', dc'' = if t = "" then dr',dc'
                         else turn t dr' dc'
        move map actions r' c' dr'' dc'' (i+1) f
    else
        r, c, dr, dc        

let dirToN = function
    | 0, 1  -> 0
    | 0, -1 -> 2
    | 1, 0  -> 1
    | -1, 0 -> 3
    | _ -> failwith "Hmmm this should happen"
    
let lines = File.ReadAllLines("input_day22_2.txt")
let map = parseMap lines[0..lines.Length-3]
let actions = parseMoves (lines |> Array.last)
let sr,sc = 0, lines[1] |> Seq.findIndex(fun ch -> ch = '.')

let part1 =
    let r,c,dr,dc = move map actions sr sc 0 1 0 nextSqPt1
    let p1 = 1000 * (r+1) + 4 * (c+1) + (dirToN (dr,dc))
    printfn $"P1: {p1}"

let part2 =    
    let r,c,dr,dc = move map actions sr sc 0 1 0 nextSqPt2    
    let p2 = 1000 * (r+1) + 4 * (c+1) + (dirToN (dr,dc))
    printfn $"P2: {p2}"
    
