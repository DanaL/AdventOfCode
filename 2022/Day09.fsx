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

// find the diaganol move that will catch the tail up to the
// N, S, E, or W neighbour of the head
let overlapNeighbour hr hc tr tc =
    let neighboursHead = Set.ofList [(hr-1,hc); (hr+1,hc); (hr,hc-1); (hr,hc+1)]
    let neighboursTail = Set.ofList [(tr-1,tc-1); (tr-1,tc+1); (tr+1,tc-1); (tr+1,tc+1)]
    Set.intersect neighboursHead neighboursTail |> Set.maxElement
    
let move state mv =
    let dr, dc = match mv with
                 | Up -> -1, 0
                 | Down -> 1, 0
                 | Left -> 0, -1
                 | Right -> 0, 1
    let hr, hc = state.Head
    let tr, tc = state.Tail
    let nhr, nhc = hr+dr, hc+dc
    let ntr, ntc =
        if dist nhr nhc tr tc > 1 then
            // we need to move the tail

            // if they are on the same row or col just follow, otherwise
            // it's a diaganol move
            if tr = nhr || tc = nhc then tr+dr, tc+dc
            else overlapNeighbour nhr nhc tr tc
        else
            tr, tc
    { Head=nhr,nhc; Tail=ntr,ntc; Visited = state.Visited.Add(ntr,ntc) }

let q = fetchInput()
let s0 = { Head= 0,0; Tail= 0,0; Visited= Set.empty.Add(0,0) }

let res = q |> Array.fold(fun s mv -> move s mv) s0
printfn $"P1: {res.Visited.Count}"
