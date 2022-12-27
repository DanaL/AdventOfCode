let toInt (s:string) =
    let v,_ = s |> Seq.fold(fun (x,i) ch -> let n = match ch with
                                                    | '=' -> -2L * (pown 5L i)
                                                    | '-' -> -(pown 5L i)
                                                    | _ -> let d = ch |> string |> int64
                                                           d * (pown 5L i)
                                            x + n, i-1) (0L, s.Length-1)
    v                                       

let snafuDigit = function
    | 0 -> ('0', '0')
    | 1 -> ('0', '1')
    | 2 -> ('0', '2')
    | 3 -> ('1', '=')
    | 4 -> ('1', '-')
    | _ -> failwith "Hmm this shouldn't happen"
    
let rec toSnafuDigits x =
    if x / 5 <> 0 then
        snafuDigit(x % 5) :: toSnafuDigits (x/5)
    else
        [ snafuDigit(x) ]

let syms = [ '0'; '1'; '2'; '='; '-' ]

let rec mashUp (digits:List<char*char>) i carry =
    if i < digits.Length then        
        let a,b = digits[i]
        let s = syms |> List.findIndex(fun c -> c = b)
        let cs = syms |> List.findIndex(fun c -> c = carry)
        let sym = syms[(s + cs) % syms.Length]
        printfn $"{carry} {s + cs}"
        string(sym) :: mashUp digits (i + 1) a
    elif carry = '0' then
        [ "" ]
    else
        [ string(carry) ]
        
let toSnafu x =
    let digits = toSnafuDigits x
    printfn $"%A{digits}"
    mashUp digits 0 '0' |> List.rev |> String.concat ""
printfn $"%A{toSnafu 1747}"
// let mutable n = "0"
// for _ in 1..3 do
//     printf $"{n} + {1} = "
//     n <- snafuAdd n "1"
//     printfn $" {n}"
    
    //let pairs = Seq.zip a b
    //p/rintfn $"{pairs}"
// let rec snafuDigits x =
//     let v = x / 5L
//     let b10 = x % 10L
//     let d = match b10 with
//     | 0 | 1 | 2 -> string(b10) + " "
//     | 3 -> "1= "
//     | 4 -> "1- "
//     | 5 -> "10 "
//     | 6 -> "11 "
//     | 7 -> "12 "
//     | 8 -> "2= "
//     | 9 -> "2- "
    
//     printfn $"{b10}"
//     let d = match x % 5L with
//                          | 0L | 1L | 2L -> string(x % 5L) + " "
//                          | 3L -> "1= "
//                          | 4L -> "1- "
//                          | _ -> failwith "Hmm this shouldn't happen"
//     if v > 0 then
//         d :: snafuDigits v
//     else
//         [d]

// let toSnafu x = String.concat "" (snafuDigits x |> List.rev)

//let x = snafuAdd "22" "1"
//printfn $"%A{x}"
//let v0 = toInt "10-"
//printfn $"{v0}"
//let v1 = toInt "1121-1110-1=0"
//printfn $"{v1}"

//printfn $"{toSnafu 9L}"
//printfn $"{toSnafu 12345L}"

