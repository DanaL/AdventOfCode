open System.IO

let calc line =
    match line with
    | "A X" | "B Y" | "C Z" -> 3
    | "A Y" | "B Z" | "C X" -> 6
    | _ -> 0
    + (int line[2] - int 'X' + 1)

let part2 = function
    | "A X" -> calc "A Z"
    | "B X" -> calc "B X"
    | "C X" -> calc "C Y"
    | "A Y" -> calc "A X"
    | "B Y" -> calc "B Y"
    | "C Y" -> calc "C Z"
    | "A Z" -> calc "A Y"
    | "B Z" -> calc "B Z"
    | "C Z" -> calc "C X"
    | _ -> failwith "Hmm this shouldn't happen"

let lines = File.ReadAllLines("input_day02.txt")
printfn $"P1: {lines |> Array.map calc |> Array.sum}"
printfn $"P2: {lines |> Array.map part2 |> Array.sum}"
