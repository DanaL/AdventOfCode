// Seq.windowed 4 piped to:
// fold so that I can pass an indeox?
// or just a recursive function?

let s = System.IO.File.ReadAllText("input_day06.txt")
let seek s size =
    s |> Seq.windowed size
      |> Seq.takeWhile(fun w -> let a = Array.distinct w
                                a.Length <> 4)
      |> Seq.length + size                                
printfn $"P1: {seek s 4}"
