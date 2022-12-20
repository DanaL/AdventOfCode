open System.IO

let score (arr:int64 list) =
    let l = arr.Length
    let zi = arr |> List.findIndex(fun x -> x = 0)    
    arr[(zi + 1000) % l] + arr[(zi + 2000) % l] + arr[(zi + 3000) % l]
    
let switch (arr:List<int64*int>) i item =    
    let v, _ = item
    let arr' = arr |> List.removeAt i
    let n = int ((int64 i + v) % int64 arr'.Length)
    let ni = if n < 0 then arr'.Length + n
             else n        
    arr' |> List.insertAt ni item

let shuffle rounds encryptKey =
    let mutable arr = File.ReadAllLines("input_day20.txt")    
                      |> Array.mapi(fun i l -> int64(l)*encryptKey, i)
                      |> List.ofArray    
    let orig = arr        
    for _ in 1..rounds do
        for num in orig do
            let i = arr |> List.findIndex(fun x -> x = num)
            arr <- switch arr i num
                                 
    score (arr |> List.map(fun (x,_) -> x))
    
let part1 =
    let score = shuffle 1 1L
    printfn $"P1: {score}"
    
let part2 =
     let score = shuffle 10 811589153L
     printfn $"P2: {score}"
