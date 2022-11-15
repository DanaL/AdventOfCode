open System
open System.Collections.Generic
open System.IO

type Mem = Dictionary<string, uint16>

type Expr =
    | Value of uint16
    | Var of string
    | Not of string
    | LShift of string * int
    | RShift of string * int
    | And of string * string
    | Or of string * string
        
let parse (line:string) =
    let j = line.LastIndexOf(' ')
    let out = line[j+1 ..]
    let stmt = line[..j-4]
    let pieces = stmt.Split(' ')

    match pieces.Length with
        | 1 -> match UInt16.TryParse stmt with
                   | true, v -> (out, Value(v))
                   | false, _ -> (out, Var(stmt))
        | 2 -> (out, Not(pieces[1]))
        | _ -> match pieces[1] with
                   | "AND" -> (out, And(pieces[0], pieces[2]))
                   | "OR" -> (out, Or(pieces[0], pieces[2]))
                   | "LSHIFT" -> (out, LShift(pieces[0], Int32.Parse(pieces[2])))
                   | "RSHIFT" -> (out, RShift(pieces[0], Int32.Parse(pieces[2])))
                   | _ -> failwith "We should not ever get here :o"

let evalSetVar out key (signals:Mem) =
    if signals.ContainsKey(key) then
        signals[out] <- signals[key]
        true
    else
        false

let evalNot out key (signals:Mem) =
    if signals.ContainsKey(key) then
        signals[out] <- ~~~signals[key]
        true
    else
        false

let getVal (value:string) (signals:Mem) =
    match UInt16.TryParse value with
        | true, v -> Some v
        | false, _ -> if signals.ContainsKey(value) then Some signals[value]
                      else None
                      
let evalAnd out a b (signals:Mem) =
    let valA = getVal a signals
    let valB = getVal b signals
    if valA.IsNone || valB.IsNone then
        false
    else
        signals[out] <- valA.Value &&& valB.Value
        true
        
let evalOr out a b (signals:Mem) =
    let valA = getVal a signals
    let valB = getVal b signals
    if valA.IsNone || valB.IsNone then
        false
    else
        signals[out] <- valA.Value ||| valB.Value
        true
        
let evalLShift out a b (signals:Mem) =
    if not <| signals.ContainsKey(a) then
        false
    else
        signals[out] <- signals[a] <<< b
        true

let evalRShift out a b (signals:Mem) =
    if not <| signals.ContainsKey(a) then
        false
    else
        signals[out] <- signals[a] >>> b
        true
        
let eval expr (signals:Mem) =
    match expr with
        | out, Value(v) ->
            signals[out] <- v
            None
        | out, Var(v) -> 
            if evalSetVar out v signals then None else Some expr
        | out, Not(v) ->
            if evalNot out v signals then None else Some expr
        | out, And(a, b) ->
            if evalAnd out a b signals then None else Some expr
        | out, Or(a, b) ->
            if evalOr out a b signals then None else Some expr
        | out, LShift(a, b) ->
            if evalLShift out a b signals then None else Some expr
        | out, RShift(a, b) ->
            if evalRShift out a b signals then None else Some expr

// Probably a rather non-idiomatic solution using a mutable queue
// and dictionary, but any way I could think to use non-mutable
// data structures and avoiding looping over the list of instructions
// felt like it would have just been obfuscation
let instrs = File.ReadAllLines("input_day07.txt")
             |> Array.map(fun line -> parse line)

let exec instrs =
    let signals = new Mem()
    let queue = new Queue<string * Expr>()

    instrs
    |> Array.iter(fun e -> queue.Enqueue e)

    while queue.Count > 0 do
        let expr = queue.Dequeue()    
        let result = eval expr signals
        match result with
            | Some expr -> queue.Enqueue(expr)
            | None -> ()
    signals["a"]
    
let p1 = exec instrs
Console.WriteLine($"P1: %u{p1}")

// Part 2 is just overriding the rule defined for -> b to instead
// use the result of p1 as the input for b
let instr2 = instrs |> Array.map(fun (out, e) ->
                                 if out = "b" then ("b", Value(p1))
                                 else (out, e))
let p2 = exec instr2
Console.WriteLine($"P2: %d{p2}")

