open System
open System.IO;

let partOne =
    let convert c =
        if c = ')' then -1
        else 1

    let input = File.ReadAllText("input.txt")
    let result = input.ToCharArray()
                 |> Array.map (fun c -> if c = ')' then -1
                                        else 1)
                 |> Array.sum

    Console.WriteLine(result)

    
