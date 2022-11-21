
let mutable best = 0UL
let mutable best500 = 0UL
for a in 0 .. 100 do
    for b in 0 .. (100 - a) do
        for c in 0 .. (100 - a - b) do
            let d = 100 - c - b  - a
            let capacity = (max (3 * a + -3 * b + -1 * c) 0) |> uint64
            let durability = (max (3 * b) 0) |> uint64
            let flavour = (max (c * 4 + -2 * d) 0) |> uint64
            let texture = (max (-3 * a + 2 * d) 0) |> uint64
            let calories = (2 * a + 9 * b + 1 * c + 8 * d)
            // ignoring calories for part 1
            let score = capacity * durability * flavour * texture
            if score > best then best <- score
            if calories = 500 && score > best500 then best500 <- score

printfn $"P1 {best}"
printfn $"P2 {best500}"
    
