open System

type Bit =
    | I of int
    | L of Bit list
    | End of int // This is a kludge so that I can track the position
                 // of the end of a list when parsing a nested list...
    
// So, recursive func that builds list. Returns all the
// the items that go into a Bit list. Recursive and we finish
// when we reach a ] character
let rec parseList (txt:string) c =
    match txt[c] with
    | ',' -> parseList txt (c+1)
    | ']' -> [ Some (End c) ]
    | d when Char.IsDigit(d) ->
        if Char.IsDigit(txt[c+1]) then            
            [Some (I (txt[c..c+1] |> int))] @ (parseList txt (c+2))
        else
            [Some (I (txt[c..c] |> int))] @ (parseList txt (c+1))
    // '[' -> parse the sub list, find out the pos of the End Bit
    //        then continue recursing at endPos+1 (can trim the
    //        End off the returned list
    | _ -> failwith "Illegal character"
    
let a = parseList "[4,17,9]" 1
        |> List.filter (fun i -> match i with
                                 | Some (End _) -> false
                                 | _ -> true)
printfn $"%A{a}"
    
// let i = I 99
// let l = L [ I 37; I -2; L [I 4; I 4; I 4] ; I 22]

// let v = I -47
// match l with
// | L a -> v::a |> List.iter(fun i -> printfn $"%A{i}")
// | I x -> printfn $"Some digit: {x}"

// let empty = L []
