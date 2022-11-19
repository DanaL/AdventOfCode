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

let rec rule1 i (s:string) =    
    if i < s.Length - 2 then
        let c = s[i] |> int
        if c + 1 = (s[i+1] |> int) && c + 2 = (s[i+2] |> int) then        
            true
        else
            rule1 (i+1) s
    else        
        false
        
let rule2 (s:string) =
    not <| (s.IndexOf('i') > -1 || s.IndexOf('l') > -1 || s.IndexOf('o') > -1)

// I don't want to look up the regex pattern to find pairs of same
// chars so I'm going to do it in a dorky way, but it'll be prue F#
// and I guess that's better practice
let rule3 (s:string) =
    s.ToCharArray()
    |> Array.pairwise
    |> Array.filter(fun (a, b) -> a = b)
    |> Array.distinct
    |> Array.length >= 2
    
let nextPwd start =
    Seq.initInfinite(fun i -> base26Str (start + (i |> uint64)))
    |> Seq.find(fun pwd -> rule1 0 pwd && rule2 pwd && rule3 pwd)             

let n = toDec "cqjxjnds"
let pwd = nextPwd n
Console.WriteLine($"P1: {pwd}")

// I assume part 2 is going to be trickier...
// ...apparently not :o
let n' = (toDec pwd) + 1UL
let pwd' = nextPwd n'
Console.WriteLine($"P2: {pwd'}")

