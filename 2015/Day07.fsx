open System
open System.Collections.Generic
open System.IO

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

(*
let evalRule rule out signals =
    let pieces = rule.Split(' ')
    match pieces.Length with
        | 1 -> Console.WriteLine("simple assignment")
        | 2 -> Console.WriteLine("not operation")
        
    Console.WriteLine($"eval %s{rule} -- %s{out}")
                      
let eval stmt (signals:Dictionary<string, uint16>) =
    match parse stmt with
        | out, Value(n) -> signals.Add(out, n)
        | out, Rule(s) -> evalRule s out signals
        
let signals = new Dictionary<string, uint16>()

eval "NOT d -> c" signals
eval "14 -> a" signals
eval "5 -> b" signals

Console.WriteLine($"%A{signals}")
*)

let s0 = parse "14 -> a"
Console.WriteLine(s0)
let s1 = parse "5 -> b"
Console.WriteLine(s1)
let s2 = parse "NOT d -> c"
Console.WriteLine(s2)
let s3 = parse "a RSHIFT 2 -> d"
Console.WriteLine(s3)
let s4 = parse "jj -> wd"
Console.WriteLine(s4)

// So, now:
// 1) parse all the lines, build a queue of statements
// 2) eval statements one by one, eval returns Some statement
//    or None (if None, we assigned a val to Signals?). If there is
//    a Stmt returned then add it back to the queue
// 3) Loop until the value we want is set or there are no more
//    statements in the queue
