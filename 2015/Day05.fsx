open System
open System.IO

// This is on the whole probably more verbose than it needs to be
// and I could definitely replace a bunch of the helper functions
// with regular expressions but I don't typically try to code golf
// AoC problems too hard and it's been food F# practice
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
    
let noBadPairs s =
    let bad = Set.ofList ["ab"; "cd"; "pq"; "xy"]
    let bp = s |> toPairs
             |> Seq.map(fun (a, b) -> $"%c{a}%c{b}")
             |> Seq.fold(fun v pair -> v || bad.Contains(pair)) false
    not bp
    
let hasDouble s =
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

let triplets (s:string) =
    { 0 .. s.Length - 3 }
    |> Seq.fold(fun arr i ->
                let t = s[i], s[i+1], s[i+2]
                [| t |] |> Array.append arr) [| |]

let repeats s =
    s |> triplets
      |> Array.exists (fun (a, b, c) -> a = c)

let crossProduct s1 s2 =
    seq { for elt1 in s1 do
             for elt2 in s2 do
                 yield elt1, elt2 }
             
let pairsOfPairs (s:string) =
    let pairs =
        { 0 .. s.Length - 2 }
        |> Seq.fold (fun arr i ->
                     let p = s[i], s[i+1], i, i+1
                     [| p |] |> Array.append arr) [| |]
    
    // Okay, I have pairs from the string in the form:
    //    ('a', 'a', 0, 1) (#s are their index in the string)
    // Now I want a cross-join of all the elts of the list having
    // matching letters and no shared indexes
    let cp = crossProduct pairs pairs
    cp |> Seq.exists (fun (p1, p2) ->
                      let a1, a2, a3, a4 = p1
                      let b1, b2, b3, b4 = p2
                      a1 = b1 && a2 = b2 && a3 <> b3 && a3 <> b4
                          && a4 <> b3 && a4 <> b4)

let validate2 = makeValidator [ repeats; pairsOfPairs ]
let p2 = File.ReadAllLines("input_day05.txt")
         |> Array.filter(fun line -> validate2 line)
         |> Array.length
Console.WriteLine($"P2: %d{p2}")

