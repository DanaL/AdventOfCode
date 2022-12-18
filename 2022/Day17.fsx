open System.IO

type Dir = Left | Right

let shapes = [ [(0,0); (0,1); (0,2); (0,3)]; // ####

               [(0,1); (1,0); (1,1); (1,2); //  #
                  (2,1) ];                  // ### 
                                            //  #

               [(0,0); (0,1); (0,2);        //   #
                    (1,2); (2,2)];          //   #
                                            // ###

               [(0,0); (1,0); (2,0);        // #
                    (3,0)];                 // #
                                            // #
                                            // #

               [(0,0); (0,1); (1,0);        // ##
                     (1,1) ] ]              // ##

let field = Set.empty.Add(0,0).Add(0,1).Add(0,2).Add(0,3).Add(0,4)
                  .Add(0,5).Add(0,6)

let highestPt field = field |> Seq.map(fun (row, _) -> row) |> Seq.max

let dump field =
    let top = highestPt field

    printfn "|.......|"
    let rows = field |> Seq.sortByDescending(fun (r,_) -> r)
                     |> Seq.groupBy(fun (r,_) -> r)
                     |> Map.ofSeq    
    for r in top.. -1 .. 0 do
        if r = 0 then
            printfn "+-------+"
        elif rows |> Map.containsKey r then
            let mutable s = "|"
            let cols = rows[r] |> Seq.map(fun (_, c) -> c)
                               |> Set.ofSeq
            for j in 0..6 do
                if cols.Contains(j) then s <- s + "#"
                else s <- s + "."
            s <- s + "|"
            printfn $"{s}"
        else
            printfn $"|.......|"
                                          
let canMove (field:Set<int*int>) row col sh =
    let m = shapes[sh] |> List.map(fun (r,c) -> r+row, col+c)
    let minC = m |> List.map(fun (_,c) -> c) |> List.min
    let maxC = m |> List.map(fun (_,c) -> c) |> List.max
    
    if minC < 0 || maxC > 6 then
        false
    else        
        (m |> Set.ofList
           |> Set.intersect field
           |> Set.count) = 0

let place field row col sh =
    shapes[sh] |> List.map(fun (r,c) -> r+row, c+col)
               |> Set.ofList
               |> Set.union field

let rec fall field row col shape (gas:Dir list) g =   
    let mutable c = col
  
    // Can we move as per the gas?
    let dc = if gas[g] = Left then -1 else 1    
    let g' = ((g+1) % gas.Length)

    if canMove field row (col+dc) shape then
        c <- col+dc
        
    if canMove field (row-1) c shape then
        fall field (row-1) c shape gas g'
    else
        place field row c shape, g'

//let input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
let input = File.ReadAllText("input_day17.txt").Trim()

let gas = input |> Seq.map(fun ch -> if ch = '<' then Left else Right)
                |> List.ofSeq
let mutable shape = 0
let mutable g = 0
let mutable field' = field

for _ in 1 .. 2022 do
    // starting co-ords for new rock
    let row = highestPt field'
    let col = 2   
    let nf, ng = fall field' (row+4) col shape gas g
    field' <- nf
    g <- ng    
    shape <- (shape+1) % shapes.Length

//dump field'
let p1 = field' |> Seq.map(fun (r, _) -> r) |> Seq.max
printfn $"P1: {p1}"

