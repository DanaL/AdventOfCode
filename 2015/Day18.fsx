open System.IO

let fetchInput filename =
    let txt = File.ReadAllText(filename)
    let width = txt.IndexOf('\n')
    let m = txt.Split('\n')
            |> Array.map(fun line -> line.ToCharArray())
            |> Array.concat
            |> Array.map(fun c -> c = '#')
            |> List.ofArray
    (width, m)
    
let neighbours = [ -1,-1; -1,0; -1,1; 0,-1; 0,1; 1,-1; 1,0; 1,1 ]
let countAdj row col (matrix: bool list) width =
    neighbours |> List.map(fun (dr, dc) -> row+dr, col+dc)
               |> List.filter(fun (r, c) -> r >= 0 && r < width && c >= 0 && c < width)
               |> List.filter(fun (r, c) -> matrix[r*width + c])
               |> List.length

let step1 (matrix: bool list) width =
    List.allPairs [ 0..width-1 ] [ 0..width-1 ]
    |> List.map(fun (r,c) ->
                    let i = r * width + c
                    let adj = countAdj r c matrix width
                    if matrix[i] && (adj = 2 || adj = 3) then true
                    elif (not matrix[i]) && adj = 3 then true
                    else false)
    
let width, m = fetchInput "input_day18.txt"

let p1 = { 1..100 } |> Seq.fold(fun m' _ -> step1 m' width) m
                    |> List.filter(fun sq -> sq)
                    |> List.length
printfn $"P1: {p1}"

let m2 = m |> List.updateAt ((width-1) * width) true

let step2 (matrix: bool list) width =
    List.allPairs [ 0..width-1 ] [ 0..width-1 ]
    |> List.map(fun (r,c) ->
                    let i = r * width + c
                    let adj = countAdj r c matrix width
                    if (r=0 && (c = 0 || c = width-1)) || (r=width-1 && (c=0 || c=width-1)) then true 
                    elif matrix[i] && (adj = 2 || adj = 3) then true
                    elif (not matrix[i]) && adj = 3 then true
                    else false)
let p2 = { 1..100 } |> Seq.fold(fun m' _ -> step2 m' width) m2
                    |> List.filter(fun sq -> sq)
                    |> List.length
printfn $"P2: {p2}"
