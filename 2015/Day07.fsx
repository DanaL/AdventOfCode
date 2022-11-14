open System
open System.Collections.Generic
open System.IO

type Mem = Dictionary<string, uint16>

type Expr =
    | Value of uint16
    | Var of string
    | Not of string
    | LShift of string * uint16
    | RShift of string * uint16
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
                   | "LSHIFT" -> (out, LShift(pieces[0], UInt16.Parse(pieces[2])))
                   | "RSHIFT" -> (out, RShift(pieces[0], UInt16.Parse(pieces[2])))
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
        
let eval expr (signals:Mem) =
    match expr with
        | out, Value(v) ->
            signals[out] <- v
            None
        | out, Var(v) -> 
            if evalSetVar out v signals then None else Some expr

        | out, Not(v) ->
            if evalNot out v signals then None else Some expr
        | _ -> failwith "not implemented yet"

let signals = new Mem()

let s0 = parse "14 -> a"
Console.WriteLine(s0)
eval s0 signals
let s1 = parse "5 -> b"
eval s1 signals

let s2 = parse "b -> c"
eval s2 signals

let s3 = parse "NOT a -> d"
eval s3 signals

//Console.WriteLine(s2)
//let s3 = parse "a RSHIFT 2 -> d"
//Console.WriteLine(s3)
//let s4 = parse "jj -> wd"
//Console.WriteLine(s4)

Console.WriteLine($"%A{signals}")
// So, now:
// 1) parse all the lines, build a queue of statements
// 2) eval statements one by one, eval returns Some statement
//    or None (if None, we assigned a val to Signals?). If there is
//    a Stmt returned then add it back to the queue
// 3) Loop until the value we want is set or there are no more
//    statements in the queue
