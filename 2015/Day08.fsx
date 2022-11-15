open System
open System.IO
open System.Text.RegularExpressions

let pattern = "(\\\\[\\\\\\\"])|(\\\\x[0-9|a-f][0-9|a-f])" 
let countP1 (s:string) =
    s.Length - (Regex.Replace(s, pattern, "*").Length - 2)
let p1 = File.ReadAllLines("input_day08.txt")
         |> Array.fold(fun total s -> total + countP1 s) 0
Console.WriteLine($"%d{p1}")

let countP2 (s:string) =
    s.Replace("\"", "\"\"").Replace("\\", "\\\\").Length - s.Length + 2
let p2 = File.ReadAllLines("input_day08.txt")
         |> Array.fold(fun total s -> total + countP2 s) 0
Console.WriteLine($"%d{p2}")

