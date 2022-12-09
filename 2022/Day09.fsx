type Mv = Up | Down | Left | Right
type State = { Head: int*int; Tail: int*int; Visited: Set<int*int> }

let parseLine (line:string) =
    let pieces = line.Split(' ')
    let mv = match pieces[0] with
             | "R" -> Right
             | "L" -> Left
             | "U" -> Up
             | "D" -> Down
             | _ -> failwith "Hmm this shouldn't happen"
    let amt = pieces[1] |> int
    let mvs = seq { 1..amt } |> Seq.map(fun _ -> mv) |> Array.ofSeq
    mvs
    
let fetchInput() =    
    System.IO.File.ReadAllLines("input_day09.txt")
    |> Array.map(fun l -> parseLine l)
    |> Array.concat

let dist x0 y0 x1 y1 =
    let xd = pown (x0 - x1) 2
    let yd = pown (y0 - y1) 2
    int <| sqrt(float xd + float yd)
    
let move state mv =
    let dr, dc = match mv with
                 | Up -> -1, 0
                 | Down -> 1, 0
                 | Left -> 0, -1
                 | Right -> 0, 1
    let hr, hc = state.Head
    { state with Head=hr+dr,hc+dc }

let s0 = { Head= 0,0; Tail= 0,0; Visited= Set.empty.Add(0,0) }
let s1 = move s0 Right
printfn $"%A{s0}"
printfn $"%A{s1}"

printfn $"{dist 2 1 0 1}"
//let q = fetchInput()
//printfn $"%A{q}"
