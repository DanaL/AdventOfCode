open System.IO
open System.Text.RegularExpressions

type Build = Orebot | Claybot | ObsidianBot | GeodeBot

type State = { BluePrintID:int; Turn:int; Orebots:int; Claybots:int;
               Obsidianbots:int; Geodebots:int
               Ore:int; Clay:int; Obsidian:int; Geodes:int }
               
type BluePrint = { ID:int; Orebot:int; Claybot:int;
                 OBsOre:int; OBsClay:int;
                 GBOre:int; GBObsid:int }
                 
let bluePrints = File.ReadAllLines("input_day19.txt")
                 |> Array.map(fun l -> Regex.Split(l, "\D+"))
                 |> Array.map(fun arr -> let nums =  arr[1..arr.Length-2] |> Array.map int
                                         { ID=nums[0];
                                           Orebot=nums[1];
                                           Claybot=nums[2];
                                           OBsOre=nums[3]; OBsClay=nums[4];
                                           GBOre=nums[5]; GBObsid=nums[6] })
                 |> Array.map(fun bp -> bp.ID, bp )
                 |> Map.ofArray

// What robots can we build with our current inventory?
let canBuild state =
    let bp = bluePrints[state.BluePrintID]

    printfn $"state {state}"
    printfn $"blueprint {bp}"
    let orebot = if state.Ore >= bp.Orebot then Some Orebot
                 else None
    let claybot = if state.Ore >= bp.Claybot then Some Claybot
                  else None
    let obsidBot = if state.Ore >= bp.OBsOre && state.Clay >= bp.OBsClay then Some ObsidianBot
                   else None
    let geodeBot = if state.Ore >= bp.GBOre && state.Obsidian >= bp.GBObsid then Some GeodeBot
                   else None
    printfn $"{orebot}"
    [orebot; claybot; obsidBot; geodeBot]
    |> List.choose id

let build state option =
    let bp = bluePrints[state.BluePrintID]
    match option with
    | Orebot -> { state with Orebots=state.Orebots+1;
                             Ore=state.Ore - bp.Orebot }
    | Claybot -> { state with Claybots=state.Claybots+1;
                              Ore=state.Ore - bp.Claybot }
    | ObsidianBot -> { state with Obsidianbots=state.Obsidianbots+1;
                                  Ore=state.Ore - bp.OBsOre;
                                  Clay=state.Clay - bp.OBsClay }
    | GeodeBot -> {state with Geodebots=state.Geodebots+1;
                              Ore=state.Ore - bp.GBOre;
                              Obsidian=state.Obsidian - bp.GBObsid }
let calcNextStates state =
    let options = canBuild state
    
    let state' = { state with Turn=state.Turn+1;
                              Ore=state.Ore + state.Orebots
                              Clay=state.Clay + state.Claybots
                              Obsidian=state.Obsidian + state.Obsidianbots
                              Geodes=state.Geodes + state.Geodebots }    
    if options.Length = 0 then
        [ state' ]
    else
        options |> List.map(fun opt -> build state' opt)

let bp = bluePrints[1]
let initial = { BluePrintID=bp.ID; Turn=0;
                Orebots=1; Claybots=0;Obsidianbots=0; Geodebots=0;
                Ore=4; Clay=0; Obsidian=0; Geodes=0 }

let next = calcNextStates initial
printfn $"%A{next}"
