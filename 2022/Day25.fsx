let toInt s =
    s |> Seq.fold(fun x ch -> 5L * x + match ch with
                                       | '=' -> -2L
                                       | '-' -> -1L
                                       | _ -> ch |> string |> int64) 0L

let snafuDigit = function
    | 0L -> (0, 0)
    | 1L -> (0, 1)
    | 2L -> (0, 2)
    | 3L -> (1, 3)
    | 4L -> (1, 4)
    | _ -> failwith "Hmm this shouldn't happen"
    
let rec toSnafuDigits x =
    if x / 5L <> 0L then
        snafuDigit(x % 5L) :: toSnafuDigits (x/5L)
    else
        [ snafuDigit(x) ]

let syms = [ "0"; "1"; "2"; "="; "-" ]
let rec mashUp (digits:List<int*int>) i carry =
    if i < digits.Length then        
        let a,b = digits[i]
        let j = (b + carry) % syms.Length
        let nc = if b + carry = 3 then 1 else a
        syms[j] :: mashUp digits (i + 1) nc
    elif carry = 0 then
        [ "" ]
    else
        [ string(syms[carry]) ]
        
let toSnafu x =
    mashUp (toSnafuDigits x) 0 0 |> List.rev |> String.concat ""
    
let p1 = System.IO.File.ReadAllLines("input_day25.txt")
         |> Array.sumBy toInt |> toSnafu
printfn $"P1: {p1}"
