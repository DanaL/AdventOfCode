let seek s size =
    (s |> Seq.windowed size
       |> Seq.takeWhile(fun w -> (Array.distinct w).Length <> size)
       |> Seq.length) + size
let s = System.IO.File.ReadAllText("input_day06.txt")    
printfn $"P1: {seek s 4}"
printfn $"P1: {seek s 14}"

