open System.IO

// Returns a Map of traits and the Sue #
let parse (line:string) =
    let j = line.IndexOf(':')
    let n = (line.Substring(4, j - 4)) |> int    
    let traits = line[j+1..line.Length-1].Trim().Split(',')
                 |> Array.map(fun s -> s.Trim().Split(':'))
                 |> Array.map(fun p -> p[0], (p[1].Trim()) |> int)
                 |> Map.ofArray                 
    n, traits

let sue = [ ("children", 3); ("cats", 7); ("samoyeds", 2); ("pomeranians", 3)
            ("akitas", 0); ("vizslas", 0); ("goldfish", 5); ("trees", 3)
            ("cars", 2); ("perfumes", 1) ]
          |> Map.ofList

let aunts = File.ReadAllLines("input_day16.txt")
            |> Array.map(parse)
            
let p1, _ = aunts
            |> Array.find(fun (n, s) -> s |> Map.forall(fun k v -> sue[k] = v))
printfn $"P1: {p1}"

let p2, _ = aunts
            |> Array.find(fun (n, s) -> s |> Map.forall(
                   fun k v -> match k with
                              | "trees" | "cats" -> v > sue[k]
                              | "pomeranians" | "goldfish" -> v < sue[k]
                              | _ -> sue[k] = v))
printfn $"P2: {p2}"
