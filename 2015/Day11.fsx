open System

// So for day 11 we have to generate sequential passwords by incrementing
// one letter at a time: aaaa, aaab, aaac, ... aaaz, aaba, ... and checking
// it conforms to a few rules. I overengineered the problem by writing a
// functions to convert to/from a base-26 number
let base26Str x =
    let rec digits n d =
        let r = n % 26UL
        let x = n / 26UL
        if x = 0UL then
            r::d
        else
            digits x (r::d)
    
    [] |> digits x       
       |> List.map(fun c -> char(c + 97UL) |> string)      
       |> String.concat ""

let toDec (w:string) =
    w.ToCharArray()
    |> Array.mapi(fun i c ->
                  // Sometimes a strict type system is a real pain...
                  ((c |> uint64) - 97UL) * ((pown 26UL (w.Length - i - 1)) |> uint64))
    |> Array.sum

let rule2 (s:string) =
    not <| (s.IndexOf('i') > -1 || s.IndexOf('l') > -1 || s.IndexOf('o') > -1)

let s = "ghijklmn"
Console.WriteLine(rule2 s)

//let n = toDec s
//for j in 0 .. 50 do
//    let x = n + (j |> uint64)
//    let s = base26Str x
//    Console.WriteLine(s.PadLeft(8, 'a'))
