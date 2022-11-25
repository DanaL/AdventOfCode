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

let rnd = System.Random()
let shuffle (nums: int array) =
    let mutable nums' = nums
    for j in 0..nums'.Length-1 do
        let a = rnd.Next(0, nums'.Length-1)
        let b = rnd.Next(0, nums'.Length-1)
        let tmp = nums'[a]
        nums'[a] <- nums'[b]
        nums'[b] <- tmp
    nums'

let find molecule (keys:string array) (values:string array) =
    let mutable m = molecule
    let mutable steps = 0
    while m <> "e" && m <> "NRnBSiRnCaRnFArYFArFArF" do        
        let i = rnd.Next(0, keys.Length)
        let re = Regex(keys[i])
        let m' = re.Replace(m, values[i], 1)
        if m' <> m then            
            steps <- steps + 1
            m <- m'
    if m = "e" then
        printfn $"P2: {steps}"
    else
        printfn $"Deadend: {m}"
        
// For part 2, we are going in the reverse order to build a new dictionary
let dict' = lines[0..lines.Length - 3]
            |> Array.map(fun line -> line.Split(" => "))
            |> Array.map(fun arr -> arr[1], arr[0])
            |> Array.sortBy(fun (a, _) -> a.Length)
            |> Array.rev
let keys, values = Array.unzip dict'

find molecule keys values            
// let seen = HashSet<string>()
// let molecules = PriorityQueue<string * int, int>()
// molecules.Enqueue((molecule, 0), 0)

// let mutable count = 0
// while molecules.Count > 0 do
//     let curr, step = molecules.Dequeue()    
//     if curr = "e" then
//         printfn $"P2: {step}"
//         molecules.Clear()    
//     elif seen.Contains(curr) |> not then        
//         printfn $"{step} {curr} {curr.Length}"
//         dict'.Keys
//         |> Seq.iter(fun k -> let re = Regex(k)
//                              let m' = re.Replace(curr, dict'[k], 1)
//                              if seen.Contains(curr) |> not then
//                                  molecules.Enqueue((m', step+1), m'.Length))

//         ignore(seen.Add(curr))
        
//     count <- count + 1
    

