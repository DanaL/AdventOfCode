type State = { Knots: (int*int) list; Visited: Set<int*int> }

let parseLine (line:string) =
    let pieces = line.Split(' ')
    let mv = pieces[0] 
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

let overlapMoveDiag hr hc tr tc =
    let neighboursHead = Set.ofList [(hr-1,hc); (hr+1,hc); (hr,hc-1); (hr,hc+1);
                                     (hr-1,hc-1); (hr-1,hc+1); (hr+1,hc-1); (hr+1,hc+1)]
    let neighboursTail = Set.ofList [(tr-1,tc-1); (tr-1,tc+1); (tr+1,tc-1); (tr+1,tc+1)]
    Set.intersect neighboursHead neighboursTail |> Set.maxElement

let overlapMove hr hc tr tc =
    let neighboursHead = Set.ofList [(hr-1,hc); (hr+1,hc); (hr,hc-1); (hr,hc+1)]
    let neighboursTail = Set.ofList [(tr-1,tc); (tr+1,tc); (tr,tc-1); (tr,tc+1)]    
    Set.intersect neighboursHead neighboursTail |> Set.maxElement
    
let rec moveTails toR toC (tail:(int*int) list) =
    let kr, kc = tail[0]    
    let ntr, ntc =
        if dist toR toC kr kc > 1 then
            // Okay, we need to move this tail segment
            // if they are on the same row or col, just follow,
            // otherwise it's a diaganol move
            if kr = toR || kc = toC then overlapMove toR toC kr kc
            else overlapMoveDiag toR toC kr kc
        else
            kr, kc
    if tail |> List.length = 1 then [ ntr, ntc]
    else (ntr,ntc)::(moveTails ntr ntc tail[1..])
        
let move state mv =
    let dr, dc = match mv with
                 | "U" -> -1, 0
                 | "D" -> 1, 0
                 | "L" -> 0, -1
                 | "R" -> 0, 1
                 | _ -> failwith "Hmm this shouldn't happen"
    let hr, hc = state.Knots[0]
    let nhr, nhc = hr+dr, hc+dc
    let knots' = (nhr,nhc)::(moveTails nhr nhc state.Knots[1..])
    { Knots=knots'; Visited = state.Visited.Add(knots' |> List.last) }
 
let lines = fetchInput()
let s0 = { Knots = [0,0; 0,0]; Visited= Set.empty.Add(0,0) }
let res = lines |> Array.fold(fun s mv -> move s mv) s0
printfn $"P1: {res.Visited.Count}"

let s1 = { Knots= [0,0; 0,0; 0,0; 0,0; 0,0; 0,0; 0,0; 0,0; 0,0; 0,0]
           Visited= Set.empty.Add(0,0) }
let res2 = lines |> Array.fold(fun s mv -> move s mv) s1
printfn $"P2: {res2.Visited.Count}"

