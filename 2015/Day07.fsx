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

let evalAnd out a b (signals:Mem) =
    if not <| (signals.ContainsKey(a) || signals.ContainsKey(b))  then
        false
    else
        signals[out] <- signals[a] &&& signals[b]
        true

let evalOr out a b (signals:Mem) =
    if not <| (signals.ContainsKey(a) || signals.ContainsKey(b))  then
        false
    else
        signals[out] <- signals[a] ||| signals[b]
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

let signals = new Mem()

let s0 = parse "14 -> a"
eval s0 signals
let s1 = parse "5 -> b"
eval s1 signals

let s2 = parse "1 -> c"
eval s2 signals

let s3 = parse "NOT a -> d"
eval s3 signals

let s4 = parse "a OR c -> xx"
eval s4 signals

let s5 = parse "a LSHIFT 2 -> xy"
eval s5 signals

let s6 = parse "20 -> rt"
eval s6 signals
let s7 = parse "rt RSHIFT 1 -> tr"
eval s7 signals
Console.WriteLine(signals["tr"])
eval (parse "tr -> gb") signals
Console.WriteLine(signals["gb"])
//Console.WriteLine(s2)
//let s3 = parse "a RSHIFT 2 -> d"
//Console.WriteLine(s3)
//let s4 = parse "jj -> wd"
//Console.WriteLine(s4)

//Console.WriteLine($"%A{signals}")
// So, now:
// 1) parse all the lines, build a queue of statements
// 2) eval statements one by one, eval returns Some statement
//    or None (if None, we assigned a val to Signals?). If there is
//    a Stmt returned then add it back to the queue
// 3) Loop until the value we want is set or there are no more
//    statements in the queue
