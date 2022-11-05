open System
open System.IO
open System.Text.RegularExpressions

let parseLine txt =
    let m = Regex.Match(txt, @"(\d+)x(\d+)x(\d+)")
    m.Groups[1].Value |> int, m.Groups[2].Value |> int, m.Groups[3].Value |> int

let wrappingNeeded (l, w, h) =
    let sides = [| l * w; l * h; h * w |] |> Array.sort
    2 * (sides |> Array.sum) + sides[0]

let part1 =
    File.ReadAllLines("input_day02.txt")
    |> Array.map parseLine
    |> Array.sumBy wrappingNeeded
    
Console.WriteLine($"Part 1: %d{part1}")
