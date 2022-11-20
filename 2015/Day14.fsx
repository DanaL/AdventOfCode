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

                        
