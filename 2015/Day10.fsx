open System
open System.Text
open System.Text.RegularExpressions

// Whoops this works but it takes a couple of hours to calculate the
// 40th iteration so uhh I'll probably take a different approach
// for part 2. I'm leaving my recursive grouping function though
// because I just think it looks nice
let rec groupUp ch groups curr = function
    | x::xs when x = ch -> groupUp ch groups (x::curr) xs
    | x::xs as a when x <> ch -> groupUp x (groups@[curr]) [] a
    | _ -> groups@[curr]

let group (s:string) =
    s.ToCharArray() |> List.ofArray |> groupUp s[0] [] []

let lookAndSay s =
    let sb = StringBuilder(1_000_000)
    s |> group
      |> List.map(fun g -> sb.AppendFormat("{0}{1}", g.Length, g[0]))
      |> ignore
      
    sb.ToString()

let lookAndSayRE s = 
    Regex.Matches(s, @"([\d])\1*")
    |> Seq.map(fun g -> $"%d{g.Value.Length}%c{g.Value[0]}")
    |> String.concat ""

let cycle s n =
    let mutable a = s
    for _ in 1 .. n do
        a <- lookAndSayRE a
    a.Length

let p1 = cycle "1321131112" 40      
Console.WriteLine($"P1: %d{p1}")

let p2 = cycle "1321131112" 50
Console.WriteLine($"P2: %d{p2}")

