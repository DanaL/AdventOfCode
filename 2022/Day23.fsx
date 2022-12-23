open System.IO

type Board = Set<int*int>

let N = (-1,0)
let NW = (-1,-1)
let NE = (-1,1)
let S = (1,0)
let SW = (1,-1)
let SE = (1,1)
let E = (0,1)
let W = (0,-1)

let options = [| [|N;NE;NW|]; [|S;SE;SW|]; [|W;NW;SW|]; [|E;NE;SE|] |]

let splitLine r line =
    line |> Seq.mapi(fun c ch -> if ch = '#' then Some(r,c)
                                 else None)
         |> Seq.choose id
         |> Array.ofSeq
         
let lines = File.ReadAllLines("input_day23.txt")
let elves = lines |> Array.mapi(fun r line -> splitLine r line)
                  |> Array.concat
                  |> Set.ofArray

let boundaries (sqs:Board) =
    let rows = sqs |> Seq.map(fun (r,_) -> r)
    let cols = sqs |> Seq.map(fun (_,c) -> c)
    let minRow = rows |> Seq.min
    let maxRow = rows |> Seq.max
    let minCol = cols |> Seq.min
    let maxCol = cols |> Seq.max
    minRow, maxRow, minCol, maxCol

let score (sqs:Board) =
    let minRow, maxRow, minCol, maxCol = boundaries sqs
    let _,empty = seq { for r in minRow..maxRow do
                            for c in minCol..maxCol -> r,c }
                  |> Seq.countBy(fun (r,c) -> sqs.Contains(r,c))
                  |> Seq.head
    empty
    
let dump (sqs:Board) =
    let minRow, maxRow, minCol, maxCol = boundaries sqs
    for r in minRow-1 .. maxRow+1 do
        let mutable s = ""
        for c in minCol-1 .. maxCol+1 do
            let ch = if sqs.Contains(r,c) then "#"
                     else "."
            s <- s + ch
        printfn $"{s}"
    printfn ""
    
let elf (board:Board) r c option =
    options[option]
    |> Array.exists(fun (orow, ocol) -> board.Contains(r+orow, c+ocol))

let lonely (board:Board) r c =
    not ([ N; S; E; W; NE; NW; SE; SW ]
         |> List.exists(fun (dr,dc) -> board.Contains(r+dr,c+dc)))
    
let pickOpt (board:Board) r c option =
    let spaces =
        seq { 0.. 3 }
        |> Seq.choose(fun x -> let opt = (option + x) % options.Length   
                               if lonely board r c then
                                   Some(r, c)
                               elif not(elf board r c opt) then
                                   let pr, pc = options[opt][0]
                                   Some(r + pr, c + pc)
                               else None)
        |> List.ofSeq
    
    if spaces.Length = 0 then
        (r, c), (r, c)
    else
        (r, c), spaces |> List.head
        
let propose state option =
    let next = state |> Seq.map(fun (r,c) -> pickOpt state r c option)

    // okay, I have the proposals, now I need to check to see if any
    // elves want to move to the same location
    // So, first group and count the proposed destinations. If the
    // count > 1 the elf will remain in their spot
    let dest = next |> Seq.map(fun (_,d) -> d)
                    |> Seq.groupBy(fun d -> d)
                    |> Seq.map(fun (d, arr) -> d, arr |> List.ofSeq
                                                      |> List.length)
                    |> Map.ofSeq
    next |> Seq.map(fun (s,d) -> if dest[d] > 1 then s else d)
         |> Set.ofSeq

let part1 =
    let rounds = seq { 0..9 }
                 |> Seq.fold(fun state opt -> propose state (opt % options.Length)) elves
    printfn $"P1: {score rounds}"

let rec part2 state round =
    let next = propose state (round % options.Length)
    let same = Set.difference state next
    if same.Count <> 0 then
        part2 next (round+1)
    else
        round + 1
        
let p2 = part2 elves 0
printfn $"P2: {p2}"    

