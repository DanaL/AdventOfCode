open System.IO

#load "Util.fsx"
open Utils

let parseLine (line:string) =
    let pieces = line[0..line.Length - 2].Split(' ')
    let score = if pieces[2] = "gain" then pieces[3] |> int
                else (pieces[3] |> int) * -1
    [(pieces[0], pieces[10]), score]

// Build map of the relationships between people
let rels =    
    File.ReadAllLines("input_day13.txt")
    |> Array.map(fun l -> parseLine l)
    |> List.concat
    |> Map.ofList
let people =
    File.ReadAllLines("input_day13.txt")
    |> Array.map(fun l -> l.Split(' ')[0])
    |> Array.distinct
    |> List.ofArray
    
let happiness (seating:string list) =
    (seating[0], seating |> List.last)::(seating |> List.pairwise)
    |> List.map(fun (a, b) -> rels[a, b] + rels[b, a])
    |> List.sum
    
let p1 = (permutations people) |> List.map(fun s -> happiness s)
                               |> List.max
printfn $"P1: {p1}"
