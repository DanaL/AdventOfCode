open System.IO
open System.Text.RegularExpressions

let parse s =
    let m = Regex.Match(s, "(\d+)-(\d+),(\d+)-(\d+)")
    m.Groups[1].Value |> int, m.Groups[2].Value |> int,
    m.Groups[3].Value |> int, m.Groups[4].Value |> int

let contained line =
    let a,b,c,d = parse line
    (a >= c && b <= d) || (c >= a && d <= b)

let btw a b c = a >= b && a <= c
let overlap line =
    let a,b,c,d = parse line
    (btw a c d) || (btw b c d) || (btw c a b) || (btw d a b)
        
let test f =
    File.ReadAllLines("input_day04.txt") |> Array.filter f |> Array.length
    
printfn $"P1: {test contained}"
printfn $"P2: {test overlap}"
