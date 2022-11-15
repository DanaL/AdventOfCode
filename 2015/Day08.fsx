open System
open System.IO
open System.Text.RegularExpressions

let pattern = "(\\\\[\\\\\\\"])|(\\\\x[0-9|a-f][0-9|a-f])" 
let count (s:string) =
    s.Length - (Regex.Replace(s, pattern, "*").Length - 2)
let p1 = File.ReadAllLines("input_day08.txt")
         |> Array.fold(fun total s -> total + count s) 0
Console.WriteLine($"%d{p1}")

