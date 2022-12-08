open System.Collections.Generic
open System.IO

type Folder = { mutable Size:int; mutable Folders:string list }
let lines = new Queue<string>(File.ReadAllLines("input_day07.txt")
                              |> Array.skip(1)
                              |> Array.filter(fun l -> l[0..3] <> "$ ls"))

let mutable folders = new Stack<Folder>()
let mutable visited = List.empty
let mutable root = { Size=0; Folders=List.empty }
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

let p1 = visited |> List.filter(fun f -> f.Size <= 100_000)
                 |> List.sumBy(fun f -> f.Size)
printfn $"P1: {p1}"

