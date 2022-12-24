open System.Collections.Generic
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions

type State = { BluePrintID:int; Turn:int; Orebots:int; Claybots:int;
               Obsidianbots:int; Ore:int; Clay:int; Obsidian:int; Geodes:int }
               
type BluePrint = { ID:int; Orebot:int; Claybot:int;
                 OBsOre:int; OBsClay:int;
                 GBOre:int; GBObsid:int; MaxOre:int }
                 
let bluePrints = File.ReadAllLines("input_day19.txt")
                 |> Array.map(fun l -> Regex.Split(l, "\D+"))
                 |> Array.map(fun arr -> let nums =  arr[1..arr.Length-2] |> Array.map int
                                         let maxOre = [ nums[1]; nums[2]; nums[3]; nums[5] ] |> List.max
                                         { ID=nums[0];
                                           Orebot=nums[1];
                                           Claybot=nums[2];
                                           OBsOre=nums[3]; OBsClay=nums[4];
                                           GBOre=nums[5]; GBObsid=nums[6]; MaxOre=maxOre })
                 |> Array.map(fun bp -> bp.ID, bp )
                 |> Map.ofArray

let nextOreBot state blueprint maxTurns =
    let needed = blueprint.Orebot - state.Ore
    let turns = 1 + if needed <= 0 then 0
                    elif needed % state.Orebots <> 0 then
                        needed / state.Orebots + 1
                    else
                        needed / state.Orebots
    
    if state.Turn + turns > maxTurns then
        None
    else
        Some { state with Turn=state.Turn+turns; Orebots=state.Orebots + 1
                          Ore=state.Ore + state.Orebots * turns - blueprint.Orebot
                          Clay=state.Clay + state.Claybots * turns
                          Obsidian=state.Obsidian + state.Obsidianbots * turns }

let nextClayBot state blueprint maxTurns =
    let needed = blueprint.Claybot - state.Ore
    let turns = 1 + if needed <= 0 then 0
                    elif needed % state.Orebots <> 0 then
                        needed / state.Orebots + 1
                    else
                        needed / state.Orebots
    if state.Turn + turns > maxTurns then
        None
    else        
        Some { state with Turn=state.Turn+turns; Claybots=state.Claybots + 1
                          Ore=state.Ore + state.Orebots * turns - blueprint.Claybot
                          Clay=state.Clay + state.Claybots * turns
                          Obsidian=state.Obsidian + state.Obsidianbots * turns }

let nextObsidianBot state blueprint maxTurns =
    let neededOre = blueprint.OBsOre - state.Ore
    let turnsToOre = if neededOre <= 0 then 0
                     elif neededOre % state.Orebots <> 0 then
                         neededOre / state.Orebots + 1
                     else
                         neededOre / state.Orebots
    let neededClay = blueprint.OBsClay - state.Clay
    let turnsToClay = if neededClay <= 0 then 0
                      elif neededClay % state.Claybots <> 0 then
                          neededClay / state.Claybots + 1
                      else
                          neededClay / state.Claybots
    let turns = 1 + if turnsToClay > turnsToOre then turnsToClay else turnsToOre
    
    if state.Turn + turns > maxTurns then
        None
    else
        Some { state with Turn=state.Turn+turns; Obsidianbots=state.Obsidianbots + 1
                          Ore=state.Ore + state.Orebots * turns - blueprint.OBsOre
                          Clay=state.Clay + state.Claybots * turns - blueprint.OBsClay
                          Obsidian=state.Obsidian + state.Obsidianbots * turns }

let nextGeode state blueprint maxTurns =
    let neededOre = blueprint.GBOre - state.Ore
    let turnsToOre = if neededOre <= 0 then 0
                     elif neededOre % state.Orebots <> 0 then
                         neededOre / state.Orebots + 1
                     else
                         neededOre / state.Orebots
    let neededObsidian = blueprint.GBObsid - state.Obsidian
    let turnsToObs = if neededObsidian <= 0 then 0
                     elif neededObsidian % state.Obsidianbots <> 0 then
                         neededObsidian / state.Obsidianbots + 1
                     else
                         neededObsidian / state.Obsidianbots
    let turns = 1 + if turnsToObs > turnsToOre then turnsToObs else turnsToOre
    
    if state.Turn + turns > maxTurns then
        None
    else
        let geodes = state.Geodes + (maxTurns - (state.Turn + turns))
        Some { state with Turn=state.Turn+turns
                          Ore=state.Ore + state.Orebots * turns - blueprint.GBOre
                          Clay=state.Clay + state.Claybots * turns
                          Obsidian=state.Obsidian + state.Obsidianbots * turns - blueprint.GBObsid
                          Geodes=geodes }
    
let calcNextStates state maxTurns =
    let blueprint = bluePrints[state.BluePrintID]
    let ob = if state.Orebots < blueprint.MaxOre then
                nextOreBot state blueprint maxTurns
             else
                 None
    let cb = if state.Claybots < blueprint.OBsClay then
                 nextClayBot state blueprint maxTurns
             else
                 None
    let obsb = if state.Claybots > 0 && state.Obsidianbots < blueprint.GBObsid then
                   nextObsidianBot state blueprint maxTurns
               else
                   None
    let gb = if state.Obsidianbots > 0 then
                 nextGeode state blueprint maxTurns
             else
                 None
    [ ob; cb; obsb; gb ] |> List.choose id
    
let sim blueprintID rounds =
    let mutable mostGeodes = 0
    let bp = bluePrints[blueprintID]
    let initial = { BluePrintID=bp.ID; Turn=0 
                    Orebots=1; Claybots=0;Obsidianbots=0
                    Ore=0; Clay=0; Obsidian=0; Geodes=0 }
    
    // minGuess -- a guess at the minimum geodes I should be able to fetch. So if turns remaining
    // don't allow for us to beat that guess, abandon this path. 
    //let minGuess = rounds - (bp.Claybot + bp.OBsClay + bp.GBObsid / 2) + 1
    //printfn $"{minGuess}"
    let states = Queue<State>()
    states.Enqueue(initial)
    
    while states.Count > 0 do
        let state = states.Dequeue()       
        let next = calcNextStates state rounds
        for n in next do            
            if n.Geodes > mostGeodes then
                mostGeodes <- n.Geodes
            
            if n.Turn < rounds then
                states.Enqueue(n)
            
    mostGeodes     

let stopWatch = Stopwatch()
stopWatch.Start()

let part =
    let p1 = bluePrints.Keys |> Seq.map(fun id -> let geodes = sim id 24
                                                  printfn $"Testing blueprint {id}  {geodes}"
                                                  id * geodes)
                             |> Seq.sum
    printfn $"P1: {p1}"
    
let part2 =
    let p2 = bluePrints.Keys |> Seq.take 3 |> Seq.map(fun id -> let geodes = sim id 32
                                                                printfn $"Testing blueprint {id} {geodes}"
                                                                geodes)
                             |> List.ofSeq
    printfn $"P2: {p2[0] * p2[1] * p2[2]}"
stopWatch.Stop()

let ts = stopWatch.Elapsed
printfn $"Run time: {ts.TotalSeconds}"
