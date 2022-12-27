let toInt (s:string) =
    let v,_ = s |> Seq.fold(fun (x,i) ch -> let n = match ch with
                                                    | '=' -> -2L * (pown 5L i)
                                                    | '-' -> -(pown 5L i)
                                                    | _ -> let d = ch |> string |> int64
                                                           d * (pown 5L i)
                                            x + n, i-1) (0L, s.Length-1)
    v                                       

let snafuDigit = function
    | 0L -> (0, 0)
    | 1L -> (0, 1)
    | 2L -> (0, 2)
    | 3L -> (1, 3)
    | 4L -> (1, 4)
    | _ -> failwith "Hmm this shouldn't happen"
    
let rec toSnafuDigits (x:int64) =
    if x / 5L <> 0L then
        snafuDigit(x % 5L) :: toSnafuDigits (x/5L)
    else
        [ snafuDigit(x) ]

let syms = [ "0"; "1"; "2"; "="; "-" ]
let rec mashUp (digits:List<int*int>) i carry =
    if i < digits.Length then        
        let a,b = digits[i]
        let j = (b + carry) % syms.Length
        let nc = if b + carry = 3 then 1
                 else a
        syms[j] :: mashUp digits (i + 1) nc
    elif carry = 0 then
        [ "" ]
    else
        [ string(syms[carry]) ]
        
let toSnafu x =
    let digits = toSnafuDigits x
    mashUp digits 0 0 |> List.rev |> String.concat ""
    
let p1 = System.IO.File.ReadAllLines("input_day25.txt")
         |> Array.map toInt |> Array.sum |> toSnafu
printfn $"P1: {p1}"
