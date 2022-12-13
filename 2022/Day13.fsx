type Bit =
    | I of int
    | L of Bit list

let i = I 99
let l = L [ I 37; I -2; L [I 4; I 4; I 4] ; I 22]

let v = I -47
match l with
| L a -> v::a |> List.iter(fun i -> printfn $"%A{i}")
| I x -> printfn $"Some digit: {x}"


