open System
open System.IO

let eval (line:string) x =
    match line with
    | "noop" -> [ x ]
    | _ -> [ x; x + (line[5..] |> int) ]

let cycles = File.ReadAllLines("input_day10.txt")
             |> Array.fold(fun arr line -> let n = eval line (arr |> List.last)
                                           arr @ n) [ 1 ]

let p1 = cycles[19] * 20 + cycles[59] * 60 + cycles[99] * 100 +
                cycles[139] * 140 + cycles[179] * 180 + cycles[219] * 220
printfn $"P1: {p1}"             

cycles |> List.mapi(fun c x -> if abs(c % 40 - x) < 2 then '#' else ' ')
       |> List.chunkBySize 40
       |> List.map String.Concat
       |> List.iter Console.WriteLine

