open System

let eval (mem:Map<string, int>) (c:string) = 
    match Int32.TryParse c with
    | true, int -> int
    | _ -> mem[c]

// Doggedly not using mutable values even know imo if makes sense for
// this problem :P
let inp (mem:Map<string, int>) c v =
    mem.Add(c, v)

let add (mem:Map<string, int>) a b =
    let r = (eval mem a) + (eval mem b)
    inp mem a r

let mul (mem:Map<string, int>) a b =
     let r = (eval mem a) * (eval mem b)
     inp mem a r

let div (mem:Map<string, int>) a b =
     let r = (eval mem a) / (eval mem b)
     inp mem a r

let rem (mem:Map<string, int>) a b =
     let r = (eval mem a) % (eval mem b)
     inp mem a r
    
let eql (mem:Map<string, int>) a b =
     if eval mem a = eval mem b then inp mem a 1
                                else inp mem a 0

let m = Map.empty.Add("w", 0).Add("x", 4).Add("y", 0).Add("z", 0)
let m2 = add m "w" "7"
let m3 = add m2 "w" "x"
let m4 = div m3 "w" "2"
let m5 = add m4 "w" "-1"
let m6 = eql m5 "w" "y"
printfn $"%A{m6}"

