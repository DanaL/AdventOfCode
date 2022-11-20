open System
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
    
let happiness (seating:string list) (rels:Map<(string * string),int>)  =
    let score = (seating[0], seating |> List.last)::(seating |> List.pairwise)
                |> List.map(fun (a, b) -> rels[a, b] + rels[b, a])
                |> List.sum
    score, seating
    
let p1,_ = (permutations people)
           |> List.map(fun s -> happiness s rels)
           |> List.reduce(fun (sa, pa) (sb, pb) ->                              
                              if sb > sa then sb, pb else sa, pa)
printfn $"P1: {p1}"

// For part 2, add myself to the list of people, relatinships
// of score 0 with me and everyone, then find the new best arrangement
// and finally the difference without me in it. (I think that's what
// the problem is asking...)
let peopleP2 = "Dana"::people
let relsWithMe = people |> List.map(fun p -> [(p, "Dana"), 0; ("Dana", p), 0])
                        |> List.concat
                        |> Map.ofList

let relsP2 = Map.fold(fun acc key value -> Map.add key value acc) rels relsWithMe

let p2, seating = (permutations peopleP2)
                     |> List.map(fun s -> happiness s relsP2)
                     |> List.reduce(fun (sa, pa) (sb, pb) ->
                                        if sb > sa then sb, pb else sa, pa)
printfn $"P2: {p2}"
