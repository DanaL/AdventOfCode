open System.IO

type Move = Rock | Paper | Scissors

let move = function
    | 'A' | 'X' -> Rock
    | 'B' | 'Y' -> Paper
    | 'C' | 'Z' -> Scissors
    | _ -> failwith "Hmm this shouldn't happen"

let value = function
    | Rock -> 1
    | Paper -> 2
    | Scissors -> 3
    
let scorePt1 m1 m2 =
    if m1 = m2 then 3 + value m1
    elif m1 = Rock && m2 = Scissors then 6 + value m1
    elif m1 = Paper && m2 = Rock then 6 + value m1
    elif m1 = Scissors && m2 = Paper then 6 + value m1
    else value m1

let scorePt2 mv res =
    match res with
    | 'Y' -> 3 + value mv
    | 'Z' -> match mv with
             | Rock -> 6 + value Paper
             | Paper -> 6 + value Scissors
             | Scissors -> 6 + value Rock
    | 'X' -> match mv with
             | Rock -> value Scissors
             | Paper -> value Rock
             | Scissors -> value Paper
    | _ -> failwith "Hmm this shouldn't happen"

let lines = File.ReadAllLines("input_day02.txt")

let p1 = lines |> Array.map(fun t -> scorePt1 (move t[2]) (move t[0]))
               |> Array.sum
printfn $"P1: {p1}"

let p2 = lines |> Array.map(fun t -> scorePt2 (move t[0]) t[2])
               |> Array.sum
printfn $"P2: {p2}"

