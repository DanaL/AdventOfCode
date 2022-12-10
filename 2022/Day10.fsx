open System.Collections.Generic

let lines = System.IO.File.ReadAllLines("input_day10.txt")

let eval (line:string) (stack:Stack<int*int>) =   
    let cycle,x = stack.Peek()
    stack.Push(cycle+1,x)
    if line <> "noop" then
        let v = line.Split(' ')[1] |> int
        stack.Push(cycle+2,x+v)

let stack = new Stack<int*int>()
stack.Push(0,1)
let l = "addx 3"

lines |> Array.iter(fun line -> eval line stack)
let cycles = stack |> Map.ofSeq

let p1 = cycles[19] * 20 + cycles[59] * 60 + cycles[99] * 100 +
                cycles[139] * 140 + cycles[179] * 180 + cycles[219] * 220

printfn $"P1: {p1}"             

        
