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
        
let p1 = File.ReadAllLines("input_day13.txt")
         |> Array.chunkBySize 3
         |> Array.mapi(fun i a -> match cmp (fetchLine a[0]) (fetchLine a[1]) 0 with
                                  | Yes -> i + 1
                                  | _ -> 0)
         |> Array.sum
printfn $"P1: {p1}"

// Okay, for Part 2 I'm just going to implement a quick, dumb insertion
// sort by finding the first index where the packet is in the correct order.
// If we get to the end, insertion point is, duh, the end
let findInsertion arr packet =
    try
        arr |> List.findIndex(fun a -> match cmp packet a 0 with
                                       | Yes -> true
                                       | _ -> false)
    with
    | :? KeyNotFoundException -> arr.Length

let packets = File.ReadAllLines("input_day13.txt")
              |> Array.filter(fun line -> line.Trim() <> "")
              |> Array.map(fun line -> fetchLine line)

let divider2 = fetchLine "[[2]]"
let divider6 = fetchLine "[[6]]"
let initial = [ divider2; divider6]

let sorted = packets
             |> Array.fold(fun arr p -> let i = findInsertion arr p
                                        arr |> List.insertAt i p) initial

let d2i = 1 + (sorted |> List.findIndex(fun p -> p = divider2))
let d6i = 1 + (sorted |> List.findIndex(fun p -> p = divider6))
printfn $"P2 {d2i * d6i}"
