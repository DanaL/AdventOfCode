let lines = System.IO.File.ReadAllLines("input_day08.txt")
let height = lines.Length
let width = lines[0].Length
let forest = lines |> String.concat ""

let index r c = r * width + c
let rec blockedWest (forest:string) h r c =
    if c < 0 then false
    elif forest[index r c] > h then true
    else blockedWest forest h r (c - 1)
let rec blockedEast (forest:string) h r c =
    if c = width then false
    elif forest[index r c] > h then true
    else blockedEast forest h r (c + 1)
    
let visible (forest:string) r c =
    let t = forest[index r c]
    printfn $"{t}"
    not (blockedWest forest t r (c-1) && blockedEast forest t r (c+1))
// r 5, c 6
    
//printfn $"{blocked forest 5 6}"
printfn $"{visible forest 12 2}"
