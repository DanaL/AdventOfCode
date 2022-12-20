open System.IO

let score (arr:int list) =
    let l = arr.Length
    let zi = arr |> List.findIndex(fun x -> x = 0)    
    arr[(zi + 1000) % l] + arr[(zi + 2000) % l] + arr[(zi + 3000) % l]
    
let shuffle (arr:List<int*bool>) i =    
    let v,_ = arr[i]
    let iv = i + v        
    let arr' = arr |> List.removeAt i    
    let ni = if iv = i then i                 
             elif iv >= arr'.Length then iv % arr'.Length 
             elif v > 0 || iv > 0 then iv             
             else arr'.Length - (-iv % arr'.Length)
    arr' |> List.insertAt ni (v,true)

let mutable arr = File.ReadAllLines("input_day20.txt")
                  |> Array.map(fun l -> (l |> int), false)
                  |> List.ofArray
//let mutable arr = [1,false; 2,false; -3,false; 3,false; -2,false; 0,false; 4,false]
//let mutable arr = [-6,false; 0,false; -4,false; -10,false; 37,false; 1,false]

let mutable go = true
while go do
    match arr |> List.tryFindIndex(fun (_,b) -> b = false) with
    | Some i -> arr <- shuffle arr i                
    | None -> go <- false
    
printfn $"P1: {score (arr |> List.map(fun (x,_) -> x))}"


