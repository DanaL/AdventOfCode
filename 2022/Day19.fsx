open System.IO
open System.Text.RegularExpressions

type Build = Orebot | Claybot | ObsidianBot | GeodeBot

type State = { BluePrintID:int; Turn:int; Orebots:int; Claybots:int;
               Obsidianbots:int; Geodebots:int
               Ore:int; Clay:int; Obsidian:int; Geodes:int }
               
type BluePrint = { ID:int; Orebot:int; Claybot:int;
                 ObsidianBotOre:int; ObsidianBotClay:int;
                 GeodeBotOre:int; GeodeBotObsidian:int }
                 
let bluePrints = File.ReadAllLines("input_day19.txt")
                 |> Array.map(fun l -> Regex.Split(l, "\D+"))
                 |> Array.map(fun arr -> let nums =  arr[1..arr.Length-2] |> Array.map int
                                         { ID=nums[0];
                                           Orebot=nums[1];
                                           Claybot=nums[2];
                                           ObsidianBotOre=nums[3]; ObsidianBotClay=nums[4];
                                           GeodeBotOre=nums[5]; GeodeBotObsidian=nums[6] })
                 |> Array.map(fun bp -> bp.ID, bp )
                 |> Map.ofArray

let bp = bluePrints[1]
let initial = { BluePrintID=bp.ID; Turn=0;
                Orebots=1; Claybots=0;Obsidianbots=0; Geodebots=0;
                Ore=0; Clay=0; Obsidian=0; Geodes=0 }
