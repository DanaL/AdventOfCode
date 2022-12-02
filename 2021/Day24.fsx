open System
open System.Collections.Generic

let input = new Queue<int>()

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

let exec mem (stmt:string) =
    let pieces = stmt.Split(' ')
    match pieces[0] with
    | "add" -> add mem pieces[1] pieces[2]
    | "mul" -> mul mem pieces[1] pieces[2]
    | "div" -> div mem pieces[1] pieces[2]
    | "mod" -> rem mem pieces[1] pieces[2]
    | "eql" -> eql mem pieces[1] pieces[2]
    | _ -> failwith "Hmm this shouldn't happen."

let modelNum (num:uint64) =
    let s = num.ToString()
    if s.IndexOf('0') <> -1 then
        None
    else
        let digits = s.ToCharArray()
                     |> Array.map(fun c -> c.ToString())
                     |> Array.map(Int32.Parse)                     
        Some (new Queue<int>(digits))
    
let printQ (q:Queue<int>) =
    while q.Count > 0 do
        printf $"{q.Dequeue()}"
    printfn ""
    
let m = Map.empty.Add("w", 0).Add("x", 0).Add("y", 0).Add("z", 0)
let mutable model = 11111111111119UL
let mutable go = true
while go do
    match modelNum model with
    | Some digits ->
        printQ digits
        go <- true
    | None ->
        printfn "invalid"
        go <- true
    model <- model + 1UL
    if model > 11111111111129UL then go <- false

