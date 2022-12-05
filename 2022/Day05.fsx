open System
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions

let calcCrates line =
     line
     |> Seq.mapi(fun i c -> if c >= 'A' && c <= 'Z' then Some (c, i/4 + 1) else None)
     |> Seq.choose id

let newStacks (lines: string array) =
    let stacks = new Dictionary<int, Stack<char>>()
    let initStacks ch (col:int) =
        if not (stacks.ContainsKey col) then stacks.Add(col, new Stack<char>())        
        stacks[col].Push(ch)
    
    lines |> Array.takeWhile(fun l -> l[0..1] <> " 1")
          |> Array.map calcCrates |> Seq.collect id |> Seq.rev
          |> Seq.toList |> List.iter(fun (ch, col) -> initStacks ch col)
    stacks

let sim mv =
    let lines = File.ReadAllLines("input_day05.txt")
    let s = newStacks lines
    lines |> Array.filter(fun l -> l.StartsWith("move"))
          |> Array.iter(fun l -> let m = Regex.Match(l, "move (\d+) from (\d+) to (\d+)")
                                 mv (m.Groups[1].Value |> int)
                                    (m.Groups[2].Value |> int)
                                    (m.Groups[3].Value |> int) s)
    s.Keys |> Seq.sort |> Seq.map(fun k -> s[k].Pop()) |> String.Concat

let doMovePt1 amt src dest (stacks:Dictionary<int, Stack<char>>) =
    for _ in 1..amt do
        stacks[dest].Push(stacks[src].Pop())
let p1 = sim doMovePt1
printfn $"P1: {p1}"

let doMovePt2 amt src dest (stacks:Dictionary<int, Stack<char>>) =    
    let tmp = new Stack<char>()
    for _ in 1..amt do
        tmp.Push(stacks[src].Pop())
    for _ in 1..amt do
        stacks[dest].Push(tmp.Pop())
let p2 = sim doMovePt2
printfn $"P2: {p2}"
