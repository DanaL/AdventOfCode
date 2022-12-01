open System
open System.IO

let delim = $"{Environment.NewLine}{Environment.NewLine}"
let elves = File.ReadAllText("input_day01.txt").Split(delim)
            |> Array.map(fun elf -> elf.Split("\n") |> Array.sumBy(int))
            |> Array.sortDescending
            
printfn $"P1: {elves[0]}"

let p2 = elves[0..2] |> Array.sum
printfn $"P2: {p2}"
