open System.Collections.Generic
open System.IO

let lines = File.ReadAllLines("input_day19.txt")
let molecule = lines[lines.Length - 1]

let dict = lines[0..lines.Length - 3]
           |> Array.map(fun line -> line.Split(" => "))
           |> Array.map(fun arr -> arr[0], arr[1])
           |> Array.groupBy(fst)
           |> Array.map(fun (key, values) -> (key, [for k, v in values -> v]))
           |> Map.ofArray
let initial = dict["e"]

// Filter the electrons out of the dictionary because it'll save a teeny bit of time
let dict' = dict |> Map.filter(fun key items -> key <> "e")

let rec find (m:string) (s:string) (i:int) =
    let i' = m.IndexOf(s, i)
    if i' = -1 then
        []
    else        
        let head = m[0..(i'-1)]
        let tail = m[i'+s.Length..]
        let replaced = dict'[s] |> List.map(fun v -> head + v + tail)
        replaced @ find m s (i' + s.Length)
        |> List.distinct

let r = dict'.Keys |> Seq.map(fun k -> find molecule k 0)
                   |> Seq.concat
                   |> Seq.distinct
                   |> List.ofSeq
printfn $"P1 {r.Length}"

let queue = new Queue<(int * string)>()
initial |> List.iter(fun s -> queue.Enqueue(1, s))

let steps, str = queue.Dequeue()
printfn $"{str}"
dict'.Keys |> Seq.map(fun k -> find str k 0)           
           |> Seq.iter(fun strs ->                       
                       strs |> List.iter(fun s -> queue.Enqueue(steps + 1, s)))

printfn $"%A{queue}"           
