open System
open System.IO

type Op = And | Or | Not | LShift | RShift | Set
type Val = Var of uint16 | Identifier of string
type UnaryExpr = { op: Op; v: Val }
type BinaryExpr = { op: Op; left: Val; right: Val }
type Expr = Unary of UnaryExpr | Binary of BinaryExpr
type Stmt = { out: string; expr: Expr }

let getVal (s:string) =
    match UInt16.TryParse s with
    |true, v -> Var v
    |false, _ -> Identifier s
    
let parse (line:string) =
    let pieces = line.Split(' ')
    match pieces.Length with
        | 3 -> let v = getVal pieces[0]
               { out = pieces[2]; expr = Unary { op = Set; v = v } }          
        | 4 -> { out = pieces[3];
                 expr = Unary { op = Not; v = Identifier pieces[1] } }
        | 5 -> let left = getVal pieces[0]
               let right = getVal pieces[2]
               let op = match pieces[1] with
                        | "AND" -> And
                        | "OR" -> Or
                        | "LSHIFT" -> LShift
                        | "RSHIFT" -> RShift
                        | _ -> failwith "Hmm shouldn't have got here..."
               { out = pieces[4]; expr = Binary { op = op; left = left; right = right } }
              
        | _ -> failwith "Hmm this is an unknown statement..."

File.ReadAllLines("input_day07.txt")
|> Array.map parse
|> Array.iter (fun f -> Console.WriteLine($"%A{f}"))

