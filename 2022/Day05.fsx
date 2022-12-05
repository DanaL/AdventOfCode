open System
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions

let lines = File.ReadAllLines("input_day05.txt")

let calcCrates line =
     line |> Seq.mapi(fun i c -> if c >= 'A' && c <= 'Z' then
                                     Some (c, i/4 + 1)
                                 else
                                     None)
          |> Seq.choose id

let stacks = new Dictionary<int, Stack<char>>()
let initStacks ch (col:int) =
    if not (stacks.ContainsKey col) then stacks.Add(col, new Stack<char>())        
    stacks[col].Push(ch)
    
lines |> Array.takeWhile(fun l -> l[0..1] <> " 1")
      |> Array.map calcCrates |> Seq.collect id |> Seq.rev
      |> Seq.toList |> List.map(fun (ch, col) -> initStacks ch col)

let doMovePt1 amt src dest =
    for _ in 1..amt do
        stacks[dest].Push(stacks[src].Pop())
    
lines |> Array.filter(fun l -> l.StartsWith("move"))
      |> Array.map(fun l -> let m = Regex.Match(l, "move (\d+) from (\d+) to (\d+)")
                            doMovePt1 (m.Groups[1].Value |> int) (m.Groups[2].Value |> int)
                                      (m.Groups[3].Value |> int))

let p1 = stacks.Keys |> Seq.sort |> Seq.map(fun k -> stacks[k].Pop())
         |> String.Concat
printfn $"P1: {p1}"
