open System.IO
open System.Text.RegularExpressions

type Op = Add | Mul | Pow
type Monkey = { Items:int list; M1: int; M2: int
                Denom: int; Op: Op; X: int }

let eval m i =
    let r = match m.Op with
            | Add -> i + m.X
            | Mul -> i * m.X
            | Pow -> i * i
    match r % m.Denom = 0 with
    | true -> m.M1
    | false -> m.M2

let l = "Starting items: 71, 86, 54"
let m = Regex.Match(l, "(\d+)")

let parse (txt:string) =
    let pieces = txt.Split('\n') |> Array.skip(1)

    printfn $"%A{pieces}"
    let items = Regex.Split(pieces[0], "\D+") |> Seq.skip(1) |> Seq.map int
    printfn $"%A{items}"
    let m = Regex.Match(pieces[1], "([+|\*]) (\d+)")
    let op, x = if m.Success then
                    let v = m.Groups[2].Value |> int
                    let op = if m.Groups[1].Value = "+" then Add else Mul
                    op, v
                else
                    Pow, 0
    let extract line = Regex.Split(line, "\D+")[1] |> int 
    let denom = extract pieces[2]
    let m1 = extract pieces[3]
    let m2 = extract pieces[4]
    
    printfn $"{denom}"
    
    // Denom is # in Test line
    // M1 is # in penult line
    // M2 is # in last line
    //monkey
    
let input = File.ReadAllText("input_day11.txt").Split("\n\n")
parse(input[4])
//let m0 = { Items=[71;86]; M1=6; M2=7; Denom=19; Op=Mul; X=13 }

