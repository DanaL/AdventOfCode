let lines = System.IO.File.ReadAllLines("input_day08.txt")
let height = lines.Length
let width = lines[0].Length
let forest = lines |> String.concat ""

let index r c = r * width + c
let inBounds r c = r >= 0 && r < height && c >= 0 && c < width

let rec blocked (forest:string) h r c dr dc =    
    if not (inBounds r c) then false
    elif forest[index r c] >= h then true
    else blocked forest h (r + dr) (c + dc) dr dc

let visible (forest:string) r c =
    let t = forest[index r c]   
    not (blocked forest t r (c-1) 0 -1 && blocked forest t r (c+1) 0 1
         && blocked forest t (r-1) c -1 0 && blocked forest t (r+1) c 1 0)

let pts = seq { for r in 1..height-2 do
                    for c in 1..width-2 do
                        yield r, c }
          |> List.ofSeq
          
let p1 =
     (pts
      |> List.map(fun (r,c) -> visible forest r c)             
      |> List.filter(fun p -> p)
      |> List.length) + 4 * height - 4
printfn $"P1: {p1}"

let rec countTrees (forest:string) h r c dr dc v =
    if not (inBounds r c) then v
    elif forest[index r c] >= h then v + 1
    else countTrees forest h (r + dr) (c + dc) dr dc v + 1

let scenicity (forst:string) r c =
    let t = forest[index r c]
    (countTrees forest t (r-1) c -1 0 0) * (countTrees forest t (r+1) c 1 0 0)
       * (countTrees forest t r (c-1) 0 -1 0) * (countTrees forest t r (c+1) 0 1 0)
let p2 = pts |> List.map(fun (r,c) -> scenicity forest r c) |> List.max
printfn $"P2: {p2}"
