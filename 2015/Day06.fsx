open System
open System.IO
open System.Text.RegularExpressions

// Lol P1 done but very slow with all the immutable collection creation
// and destruction and set operations...
type Switch = On | Off | Toggle
type Move = { Switch : Switch; Start : uint * uint; End : uint * uint}

let parse line =
    let m = Regex.Match(line, @"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)")
    let action = match m.Groups[1].Value with
                 | "turn on" -> On
                 | "turn off" -> Off
                 | _ -> Toggle

    { Switch = action; Start = m.Groups[2].Value |> uint, m.Groups[3].Value |> uint;
      End = m.Groups[4].Value |> uint, m.Groups[5].Value |> uint }
      
let points move =
    let startRow, startCol = move.Start
    let endRow, endCol = move.End

    Set.ofList [for r in startRow .. endRow do for c in startCol .. endCol do r, c]

let doMove lights move =
    let pts = points move
    match move with
          | { Switch = On }  -> Set.union lights pts
          | { Switch = Off } -> Set.difference lights pts
          | { Switch = Toggle } ->
              let toggleOff = Set.intersect lights pts
              let toggleOn = Set.difference pts toggleOff
              toggleOff |> Set.difference lights
                        |> Set.union toggleOn

let initial = Set.empty<uint * uint>                        
let moves = File.ReadAllLines("input_day06.txt")
           |> Array.map parse
let result = moves
             |> Array.fold(fun lights move -> doMove lights move) initial
Console.WriteLine($"P1: %d{result.Count}")
