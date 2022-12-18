open System.Collections.Generic
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

let mutable rock = 0
let fullRowTurns = new Dictionary<int, int>()
let rowToX field row =
    seq { 0..6 } |> Seq.map(fun c -> if field |> Set.contains (row,c) then 1 <<< c
                                     else 0)
                 |> Seq.sum
                 
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
        m |> List.exists(fun loc -> field |> Set.contains(loc)) |> not

let place field row col sh =
    let placed = shapes[sh] |> List.map(fun (r,c) -> r+row, c+col)
                            |> Set.ofList

    let field' = Set.union field placed
    placed |> Seq.map(fun (r,_) -> r) |> Seq.distinct
           |> Seq.iter(fun r -> if rowToX field' r = 127 then
                                    fullRowTurns.Add(r, rock))
                                    
    field'
                                                  
let rec fall field row col shape (gas:Dir list) g =   
    // Can we move as per the gas?
    let dc = if gas[g] = Left then -1 else 1    
    let g' = ((g+1) % gas.Length)

    let col' = if canMove field row (col+dc) shape then col+dc
               else col
            
    if canMove field (row-1) col' shape then
        fall field (row-1) col' shape gas g'
    else
        place field row col' shape, g'

//let input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
let input = File.ReadAllText("input_day17.txt").Trim()

let gas = input |> Seq.map(fun ch -> if ch = '<' then Left else Right)
                |> List.ofSeq

let zz = 1000000000000UL
let simulate turns =
    fullRowTurns.Clear()
    let mutable field = Set.empty.Add(0,0).Add(0,1).Add(0,2).Add(0,3).Add(0,4)
                           .Add(0,5).Add(0,6)
    let mutable shape = 0
    let mutable g = 0
    
    for r in 1 .. turns do
        rock <- r
        let row = highestPt field
        let nf, ng = fall field (row+4) 2 shape gas g
        field <- nf
        g <- ng    
        shape <- (shape+1) % shapes.Length

    field

let p1 = simulate 2022 |> Seq.map(fun (r, _) -> r) |> Seq.max
printfn $"P1: {p1}"

// Okay, part 2, lets generate a bigger field to see if we can see any patterns
let f2 = simulate 3349
printfn $"{f2 |> Seq.map(fun (r, _) -> r) |> Seq.max}"
let f3 = simulate (3349 + 1566)
printfn $"{f3 |> Seq.map(fun (r, _) -> r) |> Seq.max}"
let f4 = simulate (1594 + 1566)
printfn $"{f4 |> Seq.map(fun (r, _) -> r) |> Seq.max}"


let fullRows = new Dictionary<int, int array>()                 
// lets see if any rows are completely full
for r in 1..(highestPt f2) do    
    if rowToX f2 r = 127 then
        fullRows.Add(r, [| rowToX f2 (r+1); rowToX f2 (r+2); rowToX f2 (r+3);
                         rowToX f2 (r+4); rowToX f2 (r+5) |])

for k in fullRows.Keys do
    printfn $"{k} %A{fullRows[k]} {fullRowTurns[k]}"

// I used the above code to find and print all the full rows and track 5 rows
// above them. From that I figured out that every 1755 stones, the height of the
// tower increases by 2768 and from there just worked out the final answer
