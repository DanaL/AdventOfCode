open System.IO

let shuffle (arr:List<int*bool>) i =
    let v,_ = arr[i]
    let iv = i + v
    let ni = if iv > arr.Length then iv % arr.Length + 1
             elif v > 0 || i + v > 0 then iv
             else (arr.Length + iv - 1) % arr.Length
    arr |> List.removeAt i |> List.insertAt ni (v,true)

let arr = [ 1; 2; -3; 3; -2; 0; 4 ]
          |> List.map(fun x -> x,false)

// - turn arr into pairs of (x, visited)
// - always seek the first unvisited number
// - when you re-insert, mark it as visited
let a' = shuffle arr 0          
printfn $"%A{a'}"
let a'' = shuffle a' 0
printfn $"%A{a''}"
let a''' = shuffle a'' 1
printfn $"%A{a'''}"
let a'''' = shuffle a''' 2
printfn $"%A{a''''}"
let a''''' = shuffle a'''' 2
printfn $"%A{a'''''}"
let a6 = shuffle a''''' 3
printfn $"%A{a6}"
let a7 = shuffle a6 5
printfn $"%A{a7 |> List.map(fun (x,_) -> x)}"
