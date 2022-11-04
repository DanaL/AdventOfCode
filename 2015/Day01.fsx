open System
open System.IO;

let convert c =
    if c = ')' then -1
    else 1

let input = File.ReadAllText("input_day01.txt")

let partOne =
    let result = input.ToCharArray()
                 |> Array.map convert
                 |> Array.sum

    Console.WriteLine($"Part 1: %d{result}")

let partTwo =
    let arr = input.ToCharArray() |> Array.map convert
    
    let rec elevate (arr : int[]) floor pos =
        let newFloor = floor + arr[pos]
        if newFloor < 0 then pos + 1
        else elevate arr newFloor (pos + 1)

    
    let result = elevate arr 0 0
    Console.WriteLine($"Part 2: %d{result}")
    
