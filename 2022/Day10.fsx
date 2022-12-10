open System
open System.IO
open System.Collections.Generic

let eval (line:string) (stack:Stack<int*int>) =   
    let cycle,x = stack.Peek()
    stack.Push(cycle+1,x)
    if line <> "noop" then
        let v = line.Split(' ')[1] |> int
        stack.Push(cycle+2,x+v)

let stack = new Stack<int*int>()
stack.Push(1,1)

File.ReadAllLines("input_day10.txt") |> Array.iter(fun l -> eval l stack)
let cycles = stack |> Map.ofSeq

let p1 = cycles[20] * 20 + cycles[60] * 60 + cycles[100] * 100 +
                cycles[140] * 140 + cycles[180] * 180 + cycles[220] * 220
printfn $"P1: {p1}"             

let pixels =
    cycles |> Seq.map(fun kvp -> kvp.Key - 1,kvp.Value)
           |> Seq.map(fun (c, x) -> let p = c % 40
                                    if p >= x - 1 && p <= x + 1 then '#'
                                    else ' ')
           |> Seq.chunkBySize 40
           |> Seq.map(fun row -> row |> System.String.Concat)
           |> Seq.iter(System.Console.WriteLine)

