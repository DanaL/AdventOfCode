open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions

let inspections = new Queue<int>()

let monkey0 (items: int list) =
    seq { 1..items.Length } |> Seq.iter(fun _ -> inspections.Enqueue(0))
    items |> List.map(fun x -> (x * 13) / 3)
          |> List.map(fun x -> if x % 19 = 0 then x, 6 else x, 7)
let monkey1 (items: int list) =
    seq { 1..items.Length } |> Seq.iter(fun _ -> inspections.Enqueue(1))
    items |> List.map(fun x -> (x + 3) / 3)
          |> List.map(fun x -> if x % 2 = 0 then x, 5 else x, 4)
let monkey2 (items: int list) =
    seq { 1..items.Length } |> Seq.iter(fun _ -> inspections.Enqueue(2))
    items |> List.map(fun x -> (x + 6) / 3)
          |> List.map(fun x -> if x % 13 = 0 then x, 4 else x, 1)
let monkey3 (items: int list) =
    seq { 1..items.Length } |> Seq.iter(fun _ -> inspections.Enqueue(3))
    items |> List.map(fun x -> (x + 2) / 3)
          |> List.map(fun x -> if x % 5 = 0 then x, 6 else x, 0)
let monkey4 (items: int list) =
    seq { 1..items.Length } |> Seq.iter(fun _ -> inspections.Enqueue(4))
    items |> List.map(fun x -> (x * x) / 3)
          |> List.map(fun x -> if x % 7 = 0 then x, 5 else x, 3)
let monkey5 (items: int list) =
    seq { 1..items.Length } |> Seq.iter(fun _ -> inspections.Enqueue(5))
    items |> List.map(fun x -> (x + 4) / 3)
          |> List.map(fun x -> if x % 11 = 0 then x, 3 else x, 0)
let monkey6 (items: int list) =
    seq { 1..items.Length } |> Seq.iter(fun _ -> inspections.Enqueue(6))
    items |> List.map(fun x -> (x * 7) / 3)
          |> List.map(fun x -> if x % 17 = 0 then x, 2 else x, 7)
let monkey7 (items: int list) =
    seq { 1..items.Length } |> Seq.iter(fun _ -> inspections.Enqueue(7))
    items |> List.map(fun x -> (x + 7) / 3)
          |> List.map(fun x -> if x % 3 = 0 then x, 2 else x, 1)

let mutable inventories =
    File.ReadAllText("input_day11.txt").Split("\n\n")
    |> Array.map(fun txt -> txt.Split("\n")[1])
    |> Array.map(fun line -> Regex.Split(line, "\D+")[1..] |> Array.map int |> List.ofArray)
    
let monkeys = [| monkey0; monkey1; monkey2; monkey3; monkey4;
                 monkey5; monkey6; monkey7 |]

for _ in 1..20 do
    for m in 0..7 do
        let res = monkeys[m] inventories[m]        
        res |> List.iter(fun (v, nm) -> inventories[nm] <- v::inventories[nm])
        inventories[m] <- []

let monkeyBusiness = inspections |> Seq.groupBy id
                                 |> Seq.map(fun (i, items) -> items |> List.ofSeq |> List.length)
                                 |> List.ofSeq |> List.sortDescending
printfn $"P1: {monkeyBusiness[0] * monkeyBusiness[1]}"
                       
                       
