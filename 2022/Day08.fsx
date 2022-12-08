let lines = System.IO.File.ReadAllLines("input_day08.txt")
let height = lines.Length
let width = lines[0].Length
let forest = lines |> String.concat ""

let index r c = r * width + c
let inBounds r c = r >= 0 && r < height && c >= 0 && c < width

let rec blocked dr dc (forest:string) h r c =    
    if not (inBounds r c) then false
    elif forest[index r c] >= h then true
    else blocked dr dc forest h (r + dr) (c + dc)
    
let blockedWest = blocked 0 -1
let blockedEast = blocked 0 1
let blockedNorth = blocked -1 0
let blockedSouth = blocked 1 0

let visible (forest:string) r c =
    let t = forest[index r c]   
    not (blockedWest forest t r (c-1) && blockedEast forest t r (c+1)
         && blockedNorth forest t (r-1) c && blockedSouth forest t (r+1) c)

let p1 =
    (seq { for r in 1..height-2 do
               for c in 1..width-2 do
                  yield r, c }
     |> Seq.map(fun (r,c) -> visible forest r c)             
     |> Seq.filter(fun p -> p)
     |> Seq.length) + 4 * height
printfn $"P1: {p1}"

