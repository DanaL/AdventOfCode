open System.IO
open System.Text.RegularExpressions

type Sensor = { X:int64; Y:int64; BeaconX: int64; BeaconY:int64 }

let taxi x0 y0 x1 y1 =
    ((x0 - x1) |> abs) + ((y0 - y1) |> abs)
    
let parse line =
    let nums = Regex.Matches(line, "(-?\d+)+") |> List.ofSeq
    { X=(nums[0].Value |> int64)
      Y=(nums[1].Value |> int64)
      BeaconX=(nums[2].Value |> int64)
      BeaconY=(nums[3].Value |> int64) }


// Okay, so figure out how far the sensor is from the target row and
// how far from beacon (it doesn't scan any further than the beacon).
// After that, scan all sqs on the target row between sensorX - beaconD
// to sensorX + beaconD. (That's more squares than we need to scan but
// I'm too dumb to figure out the geometry on that)
let scan sensor targetRow =
    let beaconD = taxi sensor.X sensor.Y sensor.BeaconX sensor.BeaconY
    let rowD = taxi sensor.X sensor.Y sensor.X targetRow

    //let xd = beaconD - ((sensor.Y - targetRow) |> abs) + (sensor.X |> abs)
    //printfn $"{xd} {beaconD}"
    
    if rowD > beaconD then List.empty
    else
        seq { sensor.X - beaconD .. sensor.X + beaconD }
        |> Seq.choose(fun x -> if (taxi sensor.X sensor.Y x targetRow) <= beaconD then
                                    Some(x, targetRow)
                               else None)
        |> List.ofSeq
    
let sensors = File.ReadAllLines("input_day15.txt")
              |> Array.map parse
let beacons = sensors |> Array.map(fun s -> s.BeaconX, s.BeaconY)
                      |> List.ofArray |> List.distinct |> Set.ofList

let targetRow = 2000000L
let scanned = sensors |> Array.map(fun s -> scan s targetRow)
              |> List.concat 
              |> Set.ofList
let p1 = Set.difference scanned beacons |> Set.count          
printfn $"P1: {p1}"

