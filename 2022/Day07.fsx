open System.Collections.Generic
open System.IO

type Folder = { mutable Size:int; mutable Folders:string list }
let lines = new Queue<string>(File.ReadAllLines("input_day07.txt")
                              |> Array.skip(1)
                              |> Array.filter(fun l -> l[0..3] <> "$ ls"))

let folders = new Stack<Folder>()
let mutable visited = List.empty
let root = { Size=0; Folders=List.empty }
folders.Push(root)

while lines.Count > 0 do
    let line = lines.Dequeue()
    let pieces = line.Split(' ')
    match pieces[0] with
    | "dir" -> let n = folders.Peek()
               folders.Peek().Folders <- pieces[1]::n.Folders
    | "$" -> match pieces[2] with
             | ".." -> visited <- folders.Pop()::visited
                       folders.Peek().Size <- folders.Peek().Size + visited[0].Size
             | _ -> folders.Push({ Size=0; Folders=List.empty})
    | _ -> folders.Peek().Size <- folders.Peek().Size + (int <| pieces[0])

let mutable total = 0
while folders.Count > 1 do
    let n = folders.Pop()
    total <- total + n.Size
    visited <- n::visited
folders.Peek().Size <- folders.Peek().Size + total
   
let p1 = visited |> List.filter(fun f -> f.Size <= 100_000)
                 |> List.sumBy(fun f -> f.Size)
printfn $"P1: {p1}"

let goal = 30_000_000
let unused = 70_000_000 - folders.Peek().Size
let sizes = visited |> List.map(fun f -> f.Size) |> List.sort
let p2 = sizes |> List.find(fun s -> s + unused >= goal)
printfn $"P2: {p2}"
