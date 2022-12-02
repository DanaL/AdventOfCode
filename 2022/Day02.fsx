open System.IO

type Move = Rock | Paper | Scissors
type Result = Win | Loss | Draw

let toMove = function
    | 'A' | 'X' -> Rock
    | 'B' | 'Y' -> Paper
    | 'C' | 'Z' -> Scissors
    | _ -> failwith "Hmm this shouldn't happen"

let toRes = function
    | 'X' -> Loss
    | 'Y' -> Draw
    | 'Z' -> Win
    | _ -> failwith "Hmm this shouldn't happen"
    
let round m1 m2 =
    if m1 = m2 then Draw
    elif m1 = Rock && m2 = Scissors then Win
    elif m1 = Paper && m2 = Rock then Win
    elif m1 = Scissors && m2 = Paper then Win
    else Loss

let findMove mv res =
    match res with
    | Draw -> mv
    | Win -> match mv with
             | Rock -> Paper
             | Paper -> Scissors
             | Scissors -> Rock
    | Loss -> match mv with
              | Rock -> Scissors
              | Paper -> Rock
              | Scissors -> Paper
    
let score move result =
    match move with
    | Rock -> 1
    | Paper -> 2
    | Scissors -> 3
    +
    match result with
    | Loss -> 0 
    | Draw -> 3
    | Win -> 6

let part1 (line:string) =
    score (toMove line[2]) (round (toMove line[2]) (toMove line[0]))

let part2 (line:string) =    
    score (findMove (toMove line[0]) (toRes line[2])) (toRes line[2])

let lines = File.ReadAllLines("input_day02.txt")

let p1 = lines |> Array.map(part1) |> Array.sum
printfn $"P1: {p1}"

let p2 = lines |> Array.map(part2) |> Array.sum
printfn $"P2: {p2}"

