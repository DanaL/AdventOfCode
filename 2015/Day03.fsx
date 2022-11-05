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

let visit moves =
    moves |> Array.fold doMove [ 0,0 ]
          |> List.distinct
                    
let moves = File.ReadAllText("input_day03.txt").ToCharArray()

let p1 = visit moves
Console.WriteLine($"Part 1: %d{p1.Length}")

// split the moves up between Santa and Robo-Santa
let santa = moves |> Array.mapi(fun i e -> if i % 2 = 0 then Some(e) else None)
                  |> Array.choose id
                  |> visit
let roboSanta = moves |> Array.mapi(fun i e -> if i % 2 <> 0 then Some(e) else None)
                      |> Array.choose id
                      |> visit
let houses = (santa @ roboSanta) |> List.distinct      
Console.WriteLine($"Part 2: %d{houses.Length}")                      

                  

              
