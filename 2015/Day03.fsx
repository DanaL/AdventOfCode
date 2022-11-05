open System
open System.IO

let step dir (row, col) =
    match dir with
        | '^' -> (row - 1, col)
        | 'v' -> (row + 1, col)
        | '<' -> (row, col - 1)
        | '>' -> (row, col + 1)
        | _ -> failwith "Whoops"

let doMove (coords: _ list) dir =
    step dir coords[0] :: coords
    
let moves = File.ReadAllText("input_day03.txt").ToCharArray()

let part1 =
    let result = moves |> Array.fold doMove [ 0,0 ]
                       |> List.distinct
                       |> List.length
    Console.WriteLine($"Part 1: %d{result}")

              
