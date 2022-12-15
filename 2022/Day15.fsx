open System.Text.RegularExpressions

type Sensor = { X:int64; Y:int64; BeaconX: int64; BeaconY:int64 }

let parse line =
    let nums = Regex.Matches(line, "(-?\d+)+") |> List.ofSeq
    { X=(nums[0].Value |> int64)
      Y=(nums[1].Value |> int64)
      BeaconX=(nums[2].Value |> int64)
      BeaconY=(nums[3].Value |> int64) }
    
let b= parse "Sensor at x=-2, y=18: closest beacon is at x=-4, y=15"
printfn $"%A{b}"
