open System
open System.Collections.Generic
open System.IO

type Correct = Yes | No | Cont

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
    
let rec cmp left right i =
    let a = decon left
    let b = decon right
    
    if i >= a.Length && i < b.Length then
        Yes
    elif i < a.Length && i >= b.Length then
        No
    elif i >= a.Length && i >= b.Length then
        // This is the case where we're comparing say:
        // [[1,2,3],4]
        // [[1,2,3],5]
        // and after comparing the nested sublist, we aren't done yet
        Cont
    else
        match a[i] with
        | I x -> match b[i] with
                 | I y -> if x < y then Yes
                          elif x > y then No
                          else cmp left right (i+1)
                 | L _ -> let res = cmp (L [I x]) b[i] 0
                          match res with
                          | Cont -> cmp left right (i+1)
                          | _ -> res
                 | End _ -> failwith "Hmm this shouldn't happen"
        | L _ -> let res = match b[i] with
                           | I y -> cmp a[i] (L [I y]) 0
                           | L _ -> cmp a[i] b[i] 0
                           | End _ -> failwith "Hmm this shouldn't happen..."
                 match res with
                 | Cont -> cmp left right (i+1)
                 | _ -> res
        | End _ -> failwith "Hmm this shouldn't happen"
        
let pairs = File.ReadAllText("input_day13.txt").Split("\n")

let r = cmp (fetchLine "[1,1,3,1,1]") (fetchLine "[1,1,5,1,1]") 0
printfn $"R {r}"

let r1 = cmp (fetchLine "[[1],[2,3,4]]") (fetchLine "[[1],4]") 0
printfn $"R1 {r1}"

let r2 = cmp (fetchLine "[1,[2,[3,[4,[5,6,7]]]],8,9]") (fetchLine "[1,[2,[3,[4,[5,6,0]]]],8,9]") 0
printfn $"R2 {r2}"
//let r1 = cmpLists (fetchLine "[1,[2,[3,[4,[5,6,7]]]],8,9]") (fetchLine "[1,[2,[3,[4,[5,6,0]]]],8,9]")
//printfn $"{r1}"
