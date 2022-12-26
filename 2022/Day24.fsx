open System.Collections.Generic

let lines = System.IO.File.ReadAllLines("input_day24.txt")
let height = lines.Length - 2
let width = lines[0].Length - 2
let coords = seq { for r in 1..height do
                       for c in 1..width -> r,c }
let left = coords |> Seq.choose(fun (r,c) -> if lines[r][c] = '<' then Some(r-1,c-1) else None)
                  |> Set.ofSeq
let right = coords |> Seq.choose(fun (r,c) -> if lines[r][c] = '>' then Some(r-1,c-1) else None)
                   |> Set.ofSeq
let up = coords |> Seq.choose(fun (r,c) -> if lines[r][c] = '^' then Some(r-1,c-1) else None)
                |> Set.ofSeq
let down = coords |> Seq.choose(fun (r,c) -> if lines[r][c] = 'v' then Some(r-1,c-1) else None)
                  |> Set.ofSeq
    
let isOpen r c turn =
    // how F# (and I guess .net as a platform) handles % for negative
    // values is sure inconvenient for a lot of Advent of Code tasks
    // (as opposed to how python does it)
    let rd = if r - turn >= 0 then
                 r - turn
             elif (r - turn) % height = 0 then
                 0
             else
                 height + (r - turn) % height
    let cd = if c - turn >= 0 then
                 c - turn
             elif (c - turn) % width = 0 then
                 0
             else
                 width + (c - turn) % width
    
    not(left |> Set.contains (r, (c + turn) % width)
        || up |> Set.contains ((r + turn) % height, c)
        || down |> Set.contains (rd, c)
        || right |> Set.contains (r, cd))

let inBounds r c = r >= 0 && r < height && c >= 0 && c < width

let openSqs r c turn =
    [ (-1, 0); (1, 0); (0,0); (0,1); (0, -1) ]
    |> List.choose(fun (dr,dc) -> let ar = r+dr
                                  let ac = c+dc
                                  if inBounds ar ac && isOpen ar ac turn then
                                      Some(ar,ac,turn)
                                  else
                                      None)

// We may need to wait for the square for the first move to be open. (Not a problem in
// part one but it is in part two)    
let rec waitForStart r c t =
    if not (isOpen r c t) then
        waitForStart r c (t+1)
    else
        t
        
let findPath sr sc gr gc startTurn =
    let mutable found = false    
    let mutable st = startTurn
    let mutable path = System.Int32.MaxValue
    
    while not(found) do
        st <- waitForStart sr sc (st+1)        
        let seen = HashSet<int*int*int>()   
        let pq = Queue<int*int*int>()
        pq.Enqueue(sr, sc, st)
           
        while pq.Count > 0 do         
             let r, c, turn = pq.Dequeue()         
             if r = gr && c = gc then
                 found <- true
                 path <- turn + 1
                 pq.Clear()
             elif not(seen.Contains(r, c, turn)) then
                 ignore(seen.Add(r, c, turn))
                 let sqs = openSqs r c (turn+1)
                 sqs |> List.iter pq.Enqueue
             
    path
    
let first = findPath 0 0 (height-1) (width-1) 0
printfn $"P1: {first}"
let second = findPath (height-1) (width-1) 0 0 first
let third = findPath 0 0 (height-1) (width-1) second
printfn  $"P2: {third}"
