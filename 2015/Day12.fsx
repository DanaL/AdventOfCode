open System.IO
open System.Text.RegularExpressions

let txt = File.ReadAllText("input_day12.txt")
let p1 = Regex.Matches(txt, "(-?\d+)")
         |> Seq.map(fun m -> m.Value |> int)
         |> Seq.sum
printfn $"P1: {p1}"
