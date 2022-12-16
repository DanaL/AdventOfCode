open System.IO
open System.Text.RegularExpressions

type Sensor = { X:int64; Y:int64; BeaconX: int64; BeaconY:int64; D:int64 }

let taxi x0 y0 x1 y1 = ((x0 - x1) |> abs) + ((y0 - y1) |> abs)
    
let parse line =
    let nums = Regex.Matches(line, "(-?\d+)+") |> List.ofSeq
    let xs = nums[0].Value |> int64
    let ys = nums[1].Value |> int64
    let xb = nums[2].Value |> int64
    let yb = nums[3].Value |> int64
    
    { X=xs; Y=ys; BeaconX=xb; BeaconY=yb; D=taxi xs ys xb yb }

// Okay, so figure out how far the sensor is from the target row and
// how far from beacon (it doesn't scan any further than the beacon).
// After that, scan all sqs on the target row between sensorX - beaconD
// to sensorX + beaconD. (That's more squares than we need to scan but
// I'm too dumb to figure out the geometry on that)
let scan sensor targetRow =    
    let rowD = taxi sensor.X sensor.Y sensor.X targetRow    
    if rowD > sensor.D then List.empty
    else
        seq { sensor.X - sensor.D .. sensor.X + sensor.D }
        |> Seq.choose(fun x -> if (taxi sensor.X sensor.Y x targetRow) <= sensor.D then
                                    Some(x, targetRow)
                               else None)
        |> List.ofSeq
    
let sensors = File.ReadAllLines("input_day15.txt")
              |> Array.map parse
let beacons = sensors |> Array.map(fun s -> s.BeaconX, s.BeaconY)
                      |> List.ofArray |> List.distinct |> Set.ofList

let maxDim = 20L // 4_000_000L
let targetRow = 10L // 2000000L

let part1 =
    let scanned = sensors |> Array.map(fun s -> scan s targetRow)
                          |> List.concat 
                          |> Set.ofList
    let p1 = Set.difference scanned beacons |> Set.count          
    printfn $"P1: {p1}"

let skipTo sensor row =
    sensor.D - ((sensor.Y - row) |> abs) - (-sensor.X) + 1L

let inRange sensor x y =
    taxi sensor.X sensor.Y x y <= sensor.D

// let checkRow sensors row =
//     let mutable x = 0L
//     let mutable scanned = true
      
//     for sensor in sensors do
//         if inRange sensor x row then
//             x <- skipTo sensor row
//             found <- true
            
//     if not found then
//         // actually x will be past the end of row at this point
//         printfn $"P2: {x}, {row}"
//         true
//     else
//         false

let perimeter sensor =
    // (x, y) deltas
    let ds = [1L,1L; -1L,1L; -1L,-1L; 1L,-1L]

    // start at 'north' and walk around the perimeter of the diamond
    // that is the radius of the sensor's scan area
    seq {
        let mutable pt = sensor.X, sensor.Y - sensor.D    
        for dx,dy in ds do
            for _ in 1L..sensor.D do
                let x, y = pt            
                pt <- (x+dx, y+dy)
                yield pt
    } |> Set.ofSeq

// calc the complete edge of squares running along outside the perimeter
// (including diaganols)
let edges sensor =
    let ds = [ 1L,1L,1L; -1L,1L,1L; -1L,-1L,-1L; 1L,-1L,-1L ]

    seq {
        let mutable pt = sensor.X, sensor.Y - sensor.D
        for i in 0..3 do
            for _ in 1L..sensor.D do               
                let x,y = pt
                let dx,dy,d = ds[i]
                pt <- (x+dx, y+dy)
                yield if y+dy = sensor.Y then                    
                          [x+dx+d, y+dy]
                      elif x+dx = sensor.X && y < sensor.Y then
                          [x+dx+d, y+dy; x+dx+d+d, y+dy;
                           x+dx+1L, y+dy; x+dx+2L, y+dy;
                           x+dx, y+dy-1L; x+dx-1L, y+dy-1L; x+dx+1L, y+dy-1L]
                      elif x+dx = sensor.X && y > sensor.Y then
                          [x+dx+d, y+dy; x+dx+d+d, y+dy;
                           x+dx-1L, y+dy; x+dx-2L, y+dy;
                           x+dx, y+dy+1L; x+dx+1L, y+dy+1L; x+dx-1L, y+dy+1L]
                      else                    
                          [x+dx+d, y+dy; x+dx+d+d, y+dy] }
    |> Seq.concat |> Set.ofSeq
    
let test() =
    let s = { X=10; Y=10; BeaconX=0; BeaconY=0; D=9 }
    let p = perimeter s
    let p' = edges s

    let minX = p' |> Seq.map(fun (x, _) -> x) |> Seq.min
    let maxX = p' |> Seq.map(fun (x, _) -> x) |> Seq.max
    let minY = p' |> Seq.map(fun (_, y) -> y) |> Seq.min
    let maxY = p' |> Seq.map(fun (_, y) -> y) |> Seq.max
    
    for y in minY-1L..maxY+1L do
        let sb = System.Text.StringBuilder()
        for x in minX-1L..maxX+1L do
            let ch = if s.X = x && s.Y = y then 's'
                     elif Set.contains (x,y) p then '#'
                     elif Set.contains (x,y) p' then '*'
                     else '.'
            ignore(sb.Append(ch))
        printfn $"{sb.ToString()}"
    
let part2 =
    let sensors' = sensors |> Array.sortBy(fun s-> s.Y, s.X)
    
    test()
    //checkRow sensors' 11
    
    // Okay, we want start at 0,0 and then scan forward.
    //   - if pt is within a scanner's range, skip ahead to the next one
    //   - go row-by-row
    // let maxX = 20UL
    // let maxY = 20UL
    // let mutable x = 15L
    // let mutable y = 0L
    // let mutable go = true
    
    // while go do
    //     for sensor in sensors' do
    //         if inRange sensor x y then
    //             printfn $"{sensor} {inRange sensor x y}"
    //             printfn $"  next x = {skipTo sensor y}"
    //     go <- false

    //printfn $"{skipTo sensors[7] 2}"
    //let f = { X=(4L); Y=(5L); BeaconX=0; BeaconY=0L; D=2 }
    //printfn $"{skipTo f 3}"
    
