open System.Collections.Generic
open System.IO

type File = { Name:string; Size: int }
type Folder = {
    Name:string
    Parent:Folder option
    Files:File list option
    Children:Folder list option
}


// I think our input files have no 'bad commands' ie., changing to a folder
// that doesn't exist
let rec changeDir curr name =
    if curr.Name = name then
        curr
    elif name = "/" then
        match curr.Parent with
        | Some p -> p
        | None -> failwith "Hmm this shouldn't happen"
    else
        match curr.Children with
        | Some children -> children |> List.find(fun n -> n.Name = name)
        | None -> failwith "Hmm this shouldn't happen"
        
let newFolder name parent =
    { Name=name; Parent=Some parent; Files=None; Children=None }
    
let addFile folder file =
    match folder.Files with
    | Some files -> { folder with Files = Some(file::files) }
    | None -> { folder with Files = Some( [file] ) }

let addFolder folder child =
    match folder.Children with
    | Some children -> { folder with Children = Some(child::children) }
    | None -> { folder with Children = Some([child])}
    
let f1 = { Name = "aaa"; Size = 123 }
let f2 = { Name = "zfg"; Size = 44 }
let root = { Name="/"; Parent=None; Files= None; Children=None }
let r1 = addFile root f1
let r2 = addFile r1 f2

let r3 = addFolder r2 (newFolder "a" r2)
//let f = changeDir sub "/"
let a = changeDir r3 "a"
//printfn $"%A{a}"
let p = changeDir a "/"
printfn $"%A{p}"

// let lines = new Queue<string>(File.ReadAllLines("input_day07.txt")[1..])
// while lines.Count > 0 do
//     let line = lines.Dequeue()
//     match line[0..3] with
//     | "$ cd" -> printfn "Change dir"
//     | "$ ls" -> printfn "List"
//     | "dir " -> printfn "Folder def"
//     | _ -> printfn $"File info {line[0..3]}"
    //printfn $"{lines.Dequeue()}"
//lines |> Array.iter(fun l -> printfn $"{l}")
