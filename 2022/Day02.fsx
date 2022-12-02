open System.IO

type Move = Rock | Paper | Scissors
type Result = Win | Loss | Draw

let toMove = function
    | 'A' | 'X' -> Rock
    | 'B' | 'Y' -> Paper
    | 'C' | 'Z' -> Scissors
    | _ -> failwith "Hmm this shouldn't happen"

let round m1 m2 =
    if m1 = m2 then Draw
    elif m1 = Rock && m2 = Scissors then Win
    elif m1 = Rock && m2 = Paper then Loss
    elif m1 = Paper && m2 = Rock then Win
    elif m1 = Paper && m2 = Scissors then Loss
    elif m1 = Scissors && m2 = Paper then Win
    else Loss

let score move result =
    let pts = match move with
              | Rock -> 1
              | Paper -> 2
              | Scissors -> 3
    match result with
    | Loss -> 0 + pts
    | Draw -> 3 + pts
    | Win -> 6 + pts
    
let p1 = File.ReadAllLines("input_day02.txt")
         |> Array.map(fun line -> let o = (toMove line[0])
                                  let me = (toMove line[2])
                                  let res = round me o
                                  score me res)
         |> Array.sum
printfn $"P1: {p1}"
