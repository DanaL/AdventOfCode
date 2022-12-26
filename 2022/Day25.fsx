let toInt (s:string) =
    let v,_ = s |> Seq.fold(fun (x,i) ch -> let n = match ch with
                                                    | '=' -> -2L * (pown 5L i)
                                                    | '-' -> -(pown 5L i)
                                                    | _ -> let d = ch |> string |> int64
                                                           d * (pown 5L i)
                                            x + n, i-1) (0L, s.Length-1)
    
    v                                       

let v0 = toInt "100"
printfn $"{v0}"
let v1 = toInt "1121-1110-1=0"
printfn $"{v1}"
