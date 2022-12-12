open System.IO
open System.Text.RegularExpressions

let mutable inspections = [| 0UL; 0UL; 0UL; 0UL; 0UL; 0UL; 0UL; 0UL |]

let monkey0 (items: uint64 list) destress =
    inspections[0] <- inspections[0] + (items.Length |> uint64)
    items |> List.map(fun x -> destress (x * 13UL))
          |> List.map(fun x -> if x % 19UL = 0UL then x, 6 else x, 7)
let monkey1 (items: uint64 list) destress =
    inspections[1] <- inspections[1] + (items.Length |> uint64)  
    items |> List.map(fun x -> destress (x + 3UL))
          |> List.map(fun x -> if x % 2UL = 0UL then x, 5 else x, 4)
let monkey2 (items: uint64 list) destress =
    inspections[2] <- inspections[2] + (items.Length |> uint64)
    items |> List.map(fun x -> destress (x + 6UL))
          |> List.map(fun x -> if x % 13UL = 0UL then x, 4 else x, 1)
let monkey3 (items: uint64 list) destress =
    inspections[3] <- inspections[3] + (items.Length |> uint64)   
    items |> List.map(fun x -> destress (x + 2UL)) 
          |> List.map(fun x -> if x % 5UL = 0UL then x, 6 else x, 0)
let monkey4 (items: uint64 list) destress =
    inspections[4] <- inspections[4] + (items.Length |> uint64)       
    items |> List.map(fun x -> destress (x * x))
          |> List.map(fun x -> if x % 7UL = 0UL then x, 5 else x, 3)
let monkey5 (items: uint64 list) destress =
    inspections[5] <- inspections[5] + (items.Length |> uint64)    
    items |> List.map(fun x -> destress (x + 4UL))
          |> List.map(fun x -> if x % 11UL = 0UL then x, 3 else x, 0)
let monkey6 (items: uint64 list) destress =
    inspections[6] <- inspections[6] + (items.Length |> uint64)       
    items |> List.map(fun x -> destress (x * 7UL))
          |> List.map(fun x -> if x % 17UL = 0UL then x, 2 else x, 7)
let monkey7 (items: uint64 list) destress =
    inspections[7] <- inspections[7] + (items.Length |> uint64)        
    items |> List.map(fun x -> destress (x + 7UL))
          |> List.map(fun x -> if x % 3UL = 0UL then x, 2 else x, 1)

let monkeys = [| monkey0; monkey1; monkey2; monkey3; monkey4;
                 monkey5; monkey6; monkey7 |]

let calc rounds stressFun = 
    let mutable inventories =
        File.ReadAllText("input_day11.txt").Split("\n\n")
        |> Array.map(fun txt -> txt.Split("\n")[1])
        |> Array.map(fun line -> Regex.Split(line, "\D+")[1..] |> Array.map uint64 |> List.ofArray)

    for _ in 1..rounds do
        for m in 0..7 do
            let res = monkeys[m] inventories[m] stressFun
            res |> List.iter(fun (v, nm) -> inventories[nm] <- v::inventories[nm])
            inventories[m] <- []

    inspections <- inspections |> Array.sortDescending
    inspections[0] * inspections[1]

let dBy3 x = x / 3UL
let p1 = calc 20 dBy3
printfn $"P1: {p1}"

inspections <- [| 0UL; 0UL; 0UL; 0UL; 0UL; 0UL; 0UL; 0UL |]
let d x = x % 9699690UL;; 
let p2 = calc 10_000 d
printfn $"P2: {p2}"

