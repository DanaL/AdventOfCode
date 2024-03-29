open System.IO
open System.Text.RegularExpressions

type Reindeer = { Name:string; Speed:int; Con:int; Cooldown: int}
let parse line =    
    let m = Regex.Match(line, "(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.")
    { Name= m.Groups[1].Value
      Speed= int <| m.Groups[2].Value
      Con= int <| m.Groups[3].Value
      Cooldown= int <| m.Groups[4].Value }

let distance time r =
    let fullMove = r.Con + r.Cooldown
    let numOfTurns = time / fullMove    
    let leftover = time - (fullMove * numOfTurns)
    (r.Speed * r.Con) * numOfTurns + (min leftover r.Con) * r.Speed

let p1 = File.ReadAllLines("input_day14.txt")
         |> Array.map(parse)
         |> Array.map(distance 2503)
         |> Array.max
printfn $"P1: {p1}"

// Okay, for part 2 we need to check after each second to see which
// reindeer is in the lead and award them a point
let herd = File.ReadAllLines("input_day14.txt")
           |> Array.map(parse)
let pts = herd |> Array.map(fun r -> r.Name, 0)
               
let checkPlaces time herd pts =
    let scores = herd |> Array.map(fun r -> r.Name, distance time r)         
    let best = scores |> Array.map(fun (_, score) -> score) |> Array.max
    let leaders = scores |> Array.filter(fun (r, score) -> score = best)
                         |> Array.map(fun (r, _) -> r)
    pts |> Array.map(fun (r, sc) -> if Array.contains r leaders then r, sc+1
                                    else r, sc)
    
let p2 = seq { 1 .. 2503 }
         |> Seq.fold(fun scores time -> checkPlaces time herd scores) pts
         |> Array.map(fun (r, s) -> s) |> Array.max
printfn $"P2: {p2}"

