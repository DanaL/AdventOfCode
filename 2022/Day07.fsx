open System.Collections.Generic
open System.IO

type File = { Name:string; Size: int }
type Folder = {
    Name:string
    mutable Parent:Folder option
    mutable Files:File list
    mutable Children:Folder list
}

let rec changeDir curr name =
    if name = ".." then
        match curr.Parent with
        | Some p -> p
        | None -> failwith "Hmm this shouldn't happen"
    else
        curr.Children |> List.find(fun n -> n.Name = name)

let newFolder name = { Name=name; Parent=None; Files=list.Empty; Children=list.Empty }
let addFile (folder:Folder) file = folder.Files <- file::folder.Files
let addFolder folder child =
    child.Parent <- Some folder
    folder.Children <- child::folder.Children

let mutable root = newFolder "/"
let lines = new Queue<string>(File.ReadAllLines("input_day07.txt")[1..])

let rec printTree node depth =    
    printf $"{node.Name.PadLeft(depth * 4, ' ')}: "
    for file in node.Files do
        printf $"{file.Name} "
    printfn ""
    for dir in node.Children do
        printTree dir (depth+1)

let rec buildTree (root:Folder) (lines:Queue<string>) =
    while lines.Count > 0 do
        let line = lines.Dequeue()
        let pieces = line.Split(' ')
        if pieces[0] = "dir" then addFolder root (newFolder pieces[1])
        elif pieces[0] = "$" then
            if pieces[1] = "cd" then
                let c = changeDir root pieces[2]
                buildTree c lines
            // $ ls doesn't really do anything in part 1
        else
            let f = { Name=pieces[1]; Size= (int <| pieces[0]) }
            addFile root f
        
let rec calcFolderSizes node =
    let size = node.Files |> List.sumBy(fun f -> f.Size)
    if node.Children.Length = 0 then
        [ size ]
    else
        let childSizes = node.Children |>  List.map(fun c -> calcFolderSizes c) |> List.concat
        //(size + childSizes |> List.sum)::childSizes
        let total = childSizes |> List.sum
        (size + total)::childSizes

let rec calc2 node =
    let size = node.Files |> List.sumBy(fun f -> f.Size)    
    let sizes = if node.Children.Length = 0 then
                    [ size ]
                else
                    size::(node.Children |> List.map(fun c -> calc2 c) |> List.concat)
    sizes |> List.filter(fun s -> s <= 100_000)
        
buildTree root lines
printTree root 0
let sizes = calc2 root
let p1 = sizes |> List.sum
printfn $"P1: {p1}"

