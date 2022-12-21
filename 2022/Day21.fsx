open System
open System.IO

type Op = Add | Sub | Mul | Div
type Val =
    | Num of int64
    | Math of m1:string * op:Op * m2:string
type Monkey = { Name:string; Val:Val }

let splitOp (text:string) =
    let pieces = text.Split(' ')
    let op = match pieces[1] with
             | "+" -> Add
             | "-" -> Sub
             | "/" -> Div
             | "*" -> Mul
             | _ -> failwith "Hmm this shouldn't happen"
    Math(m1=pieces[0], op=op, m2=pieces[2])
     
let parse (line:string) =
    let pieces = line.Split(": ")
    let v = match Int64.TryParse pieces[1] with
            | true, x -> Num(x)
            | _ -> splitOp pieces[1]
    { Name=pieces[0]; Val=v }

let mathOp op a b =
    match op with
    | Add -> a + b
    | Sub -> a - b
    | Mul -> a * b
    | Div -> a / b
    
let rec resolve (monkeys:Map<string,Monkey>) name =
    let monkey = monkeys[name]
    match monkey.Val with
    | Num x -> x
    | Math (m1,op,m2) -> mathOp op (resolve monkeys m1) (resolve monkeys m2)
    
let monkeys = File.ReadAllLines("input_day21.txt")
              |> Array.map(fun l -> let m = parse l
                                    m.Name, m)
              |> Map.ofArray

let res = resolve monkeys "root"
printfn $"P1 {res}"
