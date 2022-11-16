open System

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

// Okay now I want to:
// 1) generate map of distances
//    for each line foo to bar = 7, I want (foo, bar): 7 and (bar, foo): 7
// 2) Generate all permutations of the locations
// 3) Calculate the distance of each route and select shortest
       
