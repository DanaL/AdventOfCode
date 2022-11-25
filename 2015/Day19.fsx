open System
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions

let lines = File.ReadAllLines("input_day19.txt")
let molecule = lines[lines.Length - 1]

let dict = lines[0..lines.Length - 3]
           |> Array.map(fun line -> line.Split(" => "))
           |> Array.map(fun arr -> arr[0], arr[1])
           |> Array.groupBy(fst)
           |> Array.map(fun (key, values) -> (key, [for k, v in values -> v]))
           |> Map.ofArray

let rec p1 (m:string) (s:string) (i:int) =
    let i' = m.IndexOf(s, i)
    if i' = -1 then
        []
    else        
        let head = m[0..(i'-1)]
        let tail = m[i'+s.Length..]
        let replaced = dict[s] |> List.map(fun v -> head + v + tail)
        replaced @ p1 m s (i' + s.Length)
        |> List.distinct

let r = dict.Keys |> Seq.map(fun k -> p1 molecule k 0)
                  |> Seq.concat
                  |> Seq.distinct
                  |> List.ofSeq
printfn $"P1 {r.Length}"

// For my data, some paths through the molecule converge on a dead end
// molecule that isn't the solution but can't be further modified.
// Terrible but it works: I randomize which transformation I'm using
// at each step, which gets me the correct answer after a few tries
let rnd = System.Random()
let rec find m (dict:(string*string) array) steps =    
    let i = rnd.Next(0, dict.Length)
    let k, v = dict[i]
    let re = Regex(k)
    let m' = re.Replace(m, v, 1)

    if m' = "e" then
        printfn $"P2: {steps+1}"
    elif m' = "NRnBSiRnCaRnFArYFArFArF" then
        printfn $"Deadend {m'}"
    elif m' <> m then
        find m' dict (steps+1)
    else
        find m dict steps
        
let dict' = lines[0..lines.Length - 3]
            |> Array.map(fun line -> line.Split(" => "))
            |> Array.map(fun arr -> arr[1], arr[0])            

find molecule dict' 0           

