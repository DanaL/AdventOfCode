open System
open System.IO

let threeVowels (s:string) =
    let isVowel (c:char) =
        "aeiouAEIOU".IndexOf(c) >= 0
    let x = s.ToCharArray()
            |> Array.filter(fun c -> isVowel c)
            |> Array.length
    x >= 3
    
let toPairs (s:string) =
    s.ToCharArray()
    |> Seq.ofArray
    |> Seq.pairwise
    
let noBadPairs (s:string) =
    let bad = Set.ofList ["ab"; "cd"; "pq"; "xy"]
    let bp = s |> toPairs
             |> Seq.map(fun (a, b) -> $"%c{a}%c{b}")
             |> Seq.fold(fun v pair -> v || bad.Contains(pair)) false
    not bp
    
let hasDouble (s:string) =
    s |> toPairs
      |> Seq.exists(fun (a, b) -> a = b)

let rules = [ threeVowels; noBadPairs; hasDouble ]
let makeValidator rules =
    rules
    |> List.reduce(fun first second ->
                   fun s ->
                       let passed = first s
                       if passed then
                           second s
                       else false)
                       
let validate = makeValidator rules
let p1 = File.ReadAllLines("input_day05.txt")
         |> Array.filter(fun line -> validate line)
         |> Array.length
Console.WriteLine($"P1: %d{p1}")


