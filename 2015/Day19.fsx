open System.IO

let lines = File.ReadAllLines("input_day19.txt")
let molecule = lines[lines.Length - 1]

let dict = lines[0..lines.Length - 3]
           |> Array.map(fun line -> line.Split(" => "))
           |> Array.map(fun arr -> arr[0], arr[1])
           |> Array.groupBy(fst)
           |> Array.map(fun (key, values) -> (key, [for k, v in values -> v]))
           |> Map.ofArray

let rec find (m:string) (s:string) (i:int) =
    let i' = m.IndexOf(s, i)
    if i' = -1 then
        []
    else        
        let head = m[0..(i'-1)]
        let tail = m[i'+s.Length..]
        let replaced = dict[s] |> List.map(fun v -> head + v + tail)
        replaced @ find m s (i' + s.Length)
        |> List.distinct

let r = dict.Keys |> Seq.map(fun k -> find molecule k 0)
                  |> Seq.concat
                  |> Seq.distinct
                  |> List.ofSeq
printfn $"{r.Length}"
//let p1 = dict.Keys |> Seq.map(fun k -> Regex.Matches(molecule, k).Count * dict[k].Length)
//                   |> Seq.sum
//printfn $"P1: {p1}"
