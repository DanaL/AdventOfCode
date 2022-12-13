open System
open System.Collections.Generic
open System.IO

type Bit =
    | I of int
    | L of Bit list
    | End of int // This is a kludge so that I can track the position
                 // of the end of a list when parsing a nested list...

let trimEnd list =
    list |> List.filter (fun i -> match i with
                                  | End _ -> false
                                  | _ -> true)
                                  
let rec parseList (txt:string) c =
    match txt[c] with
    | ',' -> parseList txt (c+1)
    | ']' -> [ End c ]
    | d when Char.IsDigit(d) ->
        if Char.IsDigit(txt[c+1]) then            
            [I (txt[c..c+1] |> int)] @ (parseList txt (c+2))
        else
            [I (txt[c..c] |> int)] @ (parseList txt (c+1))
    |  '[' -> let nested = parseList txt (c+1) 
              let pos = match nested |> List.last with
                        | End pos -> pos
                        | _ -> failwith "This shouldn't happen :o"
              [L (trimEnd nested)] @ parseList txt (pos+1)              
    | _ -> failwith $"Illegal character"

let fetchLine txt =
    L (parseList txt 1 |> trimEnd)

let enqueueList list =
    let q = new Queue<Bit>()    
    match list with
    | L items -> items |> List.iter(fun i -> q.Enqueue(i))
    | _ -> failwith "Hmm this shouldn't happen"
    q

let decon = function
    | L [] -> []
    | L a -> a
    | _ -> failwith "Hmm this shouldn't happen"
    
let rec cmpLists line0 line1 =
    let a = decon line0
    let b = decon line1
    printfn $"Left: %A{a}    Right: %A{b}"

    let count = if a.Length > b.Length then b.Length else a.Length 
    if a.Length = 0 then
        true
    else
        a |> List.take count
          |> List.mapi(fun i e -> cmpBits e b[i])
          |> List.reduce (&&)
and cmpBits left right =
    match left with
    | I x -> match right with
             | I y -> x <= y
             | L _ -> cmpLists (L [ left ]) right
             | End _ -> failwith "This shouldn't happen!"
    | L ll -> match right with
              | I _ -> cmpLists left (L [ right ])
              | L _ -> cmpLists left right
              | End _ -> failwith "This shouldn't happen!"
    | End _ -> failwith "This shouldn't happen!"
    
let pairs = File.ReadAllText("input_day13.txt").Split("\n")

let r = cmpLists (fetchLine "[4,4,4,4]") (fetchLine "[4,4,4]")
printfn $"{r}"

let r1 = cmpLists (fetchLine "[1,[2,[3,[4,[5,6,7]]]],8,9]") (fetchLine "[1,[2,[3,[4,[5,6,0]]]],8,9]")
printfn $"{r1}"
