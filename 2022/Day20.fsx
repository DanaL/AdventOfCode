open System.IO

let score (arr:int list) =
    let l = arr.Length
    let zi = arr |> List.findIndex(fun x -> x = 0)
    //printfn $"{zi}"   
    arr[(zi + 1000) % l] + arr[(zi + 2000) % l] + arr[(zi + 3000) % l]
    
let shuffle (arr:List<int*bool>) i =
    let v,_ = arr[i]
    let iv = i + v
    let ni = if iv > arr.Length then iv % arr.Length + 1
             elif v > 0 || i + v > 0 then iv
             else (arr.Length + iv - 1) % arr.Length
    arr |> List.removeAt i |> List.insertAt ni (v,true)

let mutable arr = File.ReadAllLines("input_day20.txt")
                  |> Array.map(fun l -> (l |> int), false)
                  |> List.ofArray

let mutable go = true
while go do
    match arr |> List.tryFindIndex(fun (_,b) -> b = false) with
    | Some i -> arr <- shuffle arr i
    | None -> go <- false

printfn $"{score (arr |> List.map(fun (x,_) -> x))}"
