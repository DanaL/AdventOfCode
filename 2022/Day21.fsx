open System
open System.IO

type Op = Add | Sub | Mul | Div
type Val =
    | Num of Decimal
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
    let v = match Decimal.TryParse pieces[1] with
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

let part1 =                                    
    let monkeysTable = monkeys |> Map.ofArray
    let res = resolve monkeysTable "root"
    printfn $"P1 {res}"

let part2 =
    let _,root = monkeys |> Array.find(fun (m,_) -> m = "root")
    let left,right = match root.Val with
                     | Math (m1,_,m2) -> m1,m2
                     | _ -> failwith "Hmm this shouldn't happen"
    let monkeysTable = monkeys
                       |> Array.filter(fun (m,_) -> m <> "root" && m <> "humn")
                       |> Map.ofArray
    let rightVal = resolve monkeysTable right

    let mutable v = 3000000000000M
    let mutable delta = 100000000000M
    let mutable go = true
    while go do        
        let leftVal = resolve (monkeysTable |> Map.add "humn" { Name="humn";Val=Num(v) }) left        
        if leftVal = rightVal then go <- false
        elif leftVal > rightVal then v <- v + delta
        else 
            v <- v - delta
            delta <- delta / 10M
    printfn $"P2: {v}"

