open System
open System.IO

// Part One
let elves = File.ReadAllLines("input_day01.txt")
            |> Array.fold(fun arr c ->
                          match Int32.TryParse c with
                          | true, n -> match arr with
                                       | [] -> [[n]]
                                       | h::t -> (n::h)::t
                          | _ -> []::arr) []
            |> List.map(List.sum)

let p1 = elves |> List.max
printfn $"P1: {p1}"

let p2 = elves |> List.sort |> List.rev |> List.take(3) |> List.sum
printfn $"P2: {p2}"
