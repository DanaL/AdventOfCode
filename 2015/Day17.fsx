open System.IO

let jugs = File.ReadAllLines("input_day17.txt")
           |> Array.map(int)
           |> Array.sortDescending
           |> List.ofArray
           
let rec combos goal (jugs: int list) (curr: int list) found =
    let found' =
        seq { 0..(jugs.Length - 1) }
        |> Seq.map(fun j ->
                   let combo = jugs[j]::curr                   
                   let total = combo |> List.sum
                   if total = goal then
                       [combo]
                   else if total < goal then
                       let jugs' = jugs |> List.removeAt j
                       (combos goal jugs' combo found)
                   else
                       []
                   )
        |> Seq.concat
        |> List.ofSeq

    found'

let printArr arr =
    arr |> List.iter(fun a -> System.Console.Write($"{a} "))
    printfn ""

let p1 = 
    seq { 0..jugs.Length - 1}
    |> Seq.map(fun j ->
               let curr = [jugs[j]]
               let jugs' = jugs |> List.removeAt j
               combos 150 jugs' curr [])
    |> Seq.concat
    |> Seq.map(fun c -> c |> List.sort)
    |> Seq.distinct
    |> List.ofSeq

printfn $"P1: {p1.Length}"
//let found = combos 25 jugs[1..] [jugs[0]] []
//found |> List.iter(fun c -> printfn $"{c}")





