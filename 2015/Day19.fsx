open System.IO

let lines = File.ReadAllLines("input_day19.txt")
let molecule = lines[lines.Length - 1]

let dict = lines[0..lines.Length - 3]
           |> Array.map(fun line -> line.Split(" => "))
           |> Array.map(fun arr -> arr[0], arr[1])
           |> Array.groupBy(fst)
           |> Array.map(fun (key, values) -> (key, [for k, v in values -> v]))
           |> Map.ofArray

let find (m:string) (s:string) (i:int) =
    let i' = m.IndexOf(s, i)
    if i' = -1 then
        []
    else        
        let head = m[0..(i'-1)]
        let tail = m[i'+s.Length..]
        dict[s] |> List.map(fun v -> head + v + tail)
        
let r = find molecule "H" 6
printfn $"%A{r}"
//let p1 = dict.Keys |> Seq.map(fun k -> Regex.Matches(molecule, k).Count * dict[k].Length)
//                   |> Seq.sum
//printfn $"P1: {p1}"
