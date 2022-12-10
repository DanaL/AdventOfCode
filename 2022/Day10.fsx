open System
open System.IO

let eval (line:string) (cycle,x) =
    if line <> "noop" then
        let v = line.Split(' ')[1] |> int
        [ cycle+2,x+v; cycle+1,x ]
    else
        [ cycle+1,x ]

let cycles = File.ReadAllLines("input_day10.txt")
             |> Array.fold(fun arr line -> let n = eval line (arr |> List.head)
                                           n @ arr) [1,1]
             |> List.rev |> Map.ofList

let p1 = cycles[20] * 20 + cycles[60] * 60 + cycles[100] * 100 +
                cycles[140] * 140 + cycles[180] * 180 + cycles[220] * 220
printfn $"P1: {p1}"             

cycles |> Seq.map(fun kvp -> kvp.Key - 1,kvp.Value)
       |> Seq.map(fun (c, x) -> let p = c % 40
                                if p >= x - 1 && p <= x + 1 then '#'
                                else ' ')
       |> Seq.chunkBySize 40
       |> Seq.map(fun row -> row |> System.String.Concat)
       |> Seq.iter(System.Console.WriteLine)

