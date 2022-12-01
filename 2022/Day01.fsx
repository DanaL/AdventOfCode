open System
open System.IO

let elves = File.ReadAllLines("input_day01.txt")
            |> Array.fold(fun arr c ->
                          match Int32.TryParse c with
                          | true, n -> match arr with
                                       | [] -> [[n]]
                                       | h::t -> (n::h)::t
                          | _ -> []::arr) []
            |> List.map(List.sum)
            |> List.sortDescending
            
printfn $"P1: {elves[0]}"

let p2 = elves[0..2] |> List.sum
printfn $"P2: {p2}"
