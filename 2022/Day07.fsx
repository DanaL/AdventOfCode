type File = { Name:string; Size: int }
type Node =
    | Leaf of File 
    | Internal of Node list

let f1 = { Name = "aaa"; Size = 123 }
let f2 = { Name = "zfg"; Size = 44 }

let n1 = Leaf(f1)
let n2 = Leaf(f2)
let dir = Internal([n1; n2])
printfn $"%A{n1}"
printfn $"%A{n2}"
printfn $"%A{dir}"
//let d1 = Dirs(n)

type Node2 = { Name:string; Children: Node2 list option;
               Parent: Node2 option }

let root = { Name="/"; Children=None; Parent=None }
