open System.IO

let score (arr:int64 list) =
    let l = arr.Length
    let zi = arr |> List.findIndex(fun x -> x = 0)    
    arr[(zi + 1000) % l] + arr[(zi + 2000) % l] + arr[(zi + 3000) % l]
    
let shuffle (arr:List<int64*bool>) i =    
    let v,_ = arr[i]
    let iv = (i |> int64) + v        
    let arr' = arr |> List.removeAt i
    let len = arr'.Length |> int64
    let ni = if iv = i then i
             elif iv >= len then (iv % len) |> int
             elif v > 0L || iv > 0L then iv |> int            
             else (len - (-iv % len)) |> int
    arr' |> List.insertAt ni (v, true)

let mutable arr = File.ReadAllLines("input_day20.txt")
                  |> Array.map(fun l -> (l |> int64), false)
                  |> List.ofArray
//let mutable arr = [1,false; 2,false; -3,false; 3,false; -2,false; 0,false; 4,false]
//let mutable arr = [-6,false; 0,false; -4,false; -10,false; 37,false; 1,false]

let part1 =
    let mutable go = true
    while go do
        match arr |> List.tryFindIndex(fun (_,b) -> b = false) with
        | Some i -> arr <- shuffle arr i                
        | None -> go <- false    
    printfn $"P1: {score (arr |> List.map(fun (x,_) -> x))}"


