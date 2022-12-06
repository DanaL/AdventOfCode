let seek s size =
    s |> Seq.windowed size
      |> Seq.findIndex(fun w -> w = Array.distinct w)
      |> (+) size
let s = System.IO.File.ReadAllText("input_day06.txt")    
printfn $"P1: {seek s 4}"
printfn $"P1: {seek s 14}"

