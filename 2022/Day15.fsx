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

let maxDim = 4_000_000L
let targetRow = 2000000L

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

// Look for a square in the row not covered by the sensors
//   - get all relevant sensors (sensorX, row within range)
//   - sort them by x co-ord (this is probably unnecessary but it
//     helps me conceptualize what's going on)
//   - start at (0, row). While x is < maxDim, check if square is in
//     scan range of sensor. If so, x becomes skipTo. If x > 4_000_000
//     entire row is covered. If we find a point that isn't covered, we win!
let testRow (sensors:Sensor array) row =
    let mutable x = 0L
    let mutable i = 0
    let mutable found = false    
    while x <= maxDim && i < sensors.Length do
        found <- false
        if inRange sensors[i] x row then
            x <- skipTo sensors[i] row
            i <- 0
            found <- true
        else
            i <- i + 1

    if x < maxDim then Some ((x, row))
    else None

let part2 =
    let sensors' = sensors |> Array.sortBy(fun s-> s.Y, s.X)

    let (x,y) = seq { 0L..maxDim } |> Seq.choose(fun row -> testRow sensors' row)
                                   |> Seq.head
    printfn $"P2: {x * 4_000_000L + y}"
    
