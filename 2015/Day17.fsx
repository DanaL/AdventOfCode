open System.IO

let jugs = File.ReadAllLines("input_day17.txt")
           |> Array.map(int)           
           |> List.ofArray
           
let rec powerSet =
    function
    | [] -> [[]]
    | (h::tail) ->
        let ps = powerSet tail
        List.map(fun ps' -> h::ps') ps @ ps

let jugs' = powerSet jugs
            |> List.filter(fun p -> p |> List.sum = 150)

printfn $"P1: {jugs'.Length}"

// For part 2 we want to find the minimum # of jugs to hold 150
// litres of eggnog and count how many combos use that minimum
let min = jugs' |> List.map(fun p -> p.Length)
                |> List.min
let p2 = jugs' |> List.filter(fun p -> p.Length = min)
printfn $"P2 {p2.Length}"                

