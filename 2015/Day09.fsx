open System
open System.IO

// Permutation code stolen from a stack overflow question. It took me
// a bit to grok how it was working

// Distributes x across the provided array ie:
// distribute 4 [7; 0; -2] generates [4; 7; 0; -2], [7; 4; 0; -2],
//   [7; 0; 4; -2] and [7; 0; -2; 4]
let rec distribute x = function   
    | [] -> [[x]]
    | (y :: ys) as l -> (x::l)::(List.map (fun a -> y::a) (distribute x ys))
                
let rec permutations = function
    | [] -> [ [] ]
    | x :: xs ->        
        let f = distribute x // partially applied function
        let tails = permutations xs
        List.concat (tails |> List.map f)

// Generate map of distances
let distances = File.ReadAllLines("input_day09.txt")
                |> Array.map(fun line -> let p = line.Split(' ')
                                         let d = p[4] |> int
                                         [(p[0], p[2]), d; (p[2], p[0]), d] )
                |> List.concat
                |> Map.ofList

// Make the list of towns and generate all the permutations of them                
let towns = distances |> Map.keys
                      |> Seq.map(fun (a, b) -> a)
                      |> Seq.distinct
                      |> List.ofSeq
let routes = permutations towns

// Calculate the distance of each route and select shortest
let lengthOfRoute route =
    route |> List.pairwise
          |> List.map(fun p -> distances[p])
          |> List.sum

let p1 = routes |> List.map lengthOfRoute
                |> List.min
Console.WriteLine($"P1: %d{p1}")

let p2 = routes |> List.map lengthOfRoute
                |> List.max
Console.WriteLine($"P2: %d{p2}")

