open System
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions

let lines = File.ReadAllLines("input_day19.txt")
let mutable molecule = lines[lines.Length - 1]

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
printfn $"P1 {r.Length}"

// For part 2, we are going in the reverse order to build a new dictionary
let dict' = lines[0..lines.Length - 3]
            |> Array.map(fun line -> line.Split(" => "))
            |> Array.map(fun arr -> arr[1], arr[0])
            |> Map.ofArray

let rec replace m k (v:string) n =
    let re = Regex(k)
    let m' = re.Replace(m, v, 1)
    if m' <> m then replace m' k v (n + 1)
    else m, n

let mutable count = 0
let mutable steps = 0
while molecule <> "e" do
    dict'.Keys
    |> Seq.iter(fun k -> let m', s = replace molecule k dict'[k] 0
                         molecule <- m'
                         steps <- steps + s)
    printfn $"{molecule}"
    count <- count + 1
    if count = 10 then molecule <- "e"
printfn $"P2: {steps}"

