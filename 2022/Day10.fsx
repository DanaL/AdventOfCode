open System
open System.IO

let eval (line:string) x =
    match line with
    | "noop" -> [ x ]
    | _ -> [ x + (line[5..] |> int); x ]

let cycles = File.ReadAllLines("input_day10.txt")
             |> Array.fold(fun arr line -> let n = eval line (arr |> List.head)
                                           n @ arr) [ 1 ]
             |> List.rev

let p1 = cycles[19] * 20 + cycles[59] * 60 + cycles[99] * 100 +
                cycles[139] * 140 + cycles[179] * 180 + cycles[219] * 220
printfn $"P1: {p1}"             

cycles |> Seq.mapi(fun c x -> let p = c % 40
                              if p >= x - 1 && p <= x + 1 then '#'
                              else ' ')
       |> Seq.chunkBySize 40
       |> Seq.map String.Concat
       |> Seq.iter Console.WriteLine

