open System

let base26Str x =
    let rec digits n d =
        let r = n % 26
        let x = n / 26
        if x = 0 then
            r::d
        else
            digits x (r::d)

    let y = digits x []
    y |> List.iter(fun d -> Console.Write($"%d{d} "))
    Console.WriteLine()
    
    [] |> digits x       
       |> List.map(fun c -> char(c + int 'a') |> string)      
       |> String.concat ""

//Console.WriteLine(base26Str 0)
//Console.WriteLine(base26Str 25)

Console.WriteLine(base26Str 621)
Console.WriteLine(base26Str 622)
Console.WriteLine(base26Str 623)
Console.WriteLine(base26Str 624)
Console.WriteLine(base26Str 675)
Console.WriteLine(base26Str 676)
//Console.WriteLine(base26Str 27)

//for j in 0 .. 30 do
//    let s = base26Str j
//    Console.WriteLine($"%3d{j} -> %s{s}")
