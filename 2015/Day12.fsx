open System
open System.IO
open System.Text.RegularExpressions

let txt = File.ReadAllText("input_day12.txt")
let p1 = Regex.Matches(txt, "(-?\d+)")
         |> Seq.map(fun m -> m.Value |> int)
         |> Seq.sum
printfn $"P1: {p1}"

// Okay, fine, for Part Two they're making us write a bit of a JSON
// parser. (Well, I could use Newtonsoft's JSON parser but that's not
// going to teach me much F#

// This could be done in a single pass but I find it easier to reason
// about by doing a first pass to get rid of the junk I don't care
// about and generate a list of tokens
type Token =
    | ObjectStart
    | ObjectEnd
    | ArrayStart
    | ArrayEnd
    | Red
    | Number of int
    | Junk

let numberToken (chars: char array) i =
    let token = chars[i..]
                |> Array.takeWhile(fun c -> c = '-' || (c >= '0' && c <= '9'))
                |> Array.map(fun c -> c |> string)
                |> String.concat ""
    let n = Int32.Parse(token)
    Number(n), (i + token.Length - 1)

// A rather janky tokenizer...        
let tokenize (chars: char array) =
    let mutable j = 0
    let tokens = new System.Collections.Generic.List<Token>()
    
    while j < chars.Length do
        if chars[j] = '"' || chars[j] = ':' then
            j <- j + 1
        let token = match chars[j] with
                    | '{' -> ObjectStart
                    | '}' -> ObjectEnd
                    | '[' -> ArrayStart
                    | ']' -> ArrayEnd
                    | 'r' -> if chars[j+1] = 'e' && chars[j+2] = 'd' then
                                 j <- j + 2
                                 Red
                             else
                                 Junk
                    | '-' ->
                        let t, j' = numberToken chars j
                        j <- j'
                        t
                    | t when t >= '0' && t <= '9' ->
                        let t, j' = numberToken chars j
                        j <- j'
                        t
                    | _ -> Junk
        j <- j + 1
        tokens.Add(token)    
    tokens |> Seq.filter(fun t -> match t with
                                  | Junk -> false
                                  | _ -> true)
           |> List.ofSeq

// Luckily I can assume a well-formed JSON doc in these functions
let sumObject tokens j =
    0, j
    
let rec sumArray (tokens: Token list) i =
    let mutable j = i
    let mutable sum = 0
    let mutable c = true
    while c do
        let s = match tokens[j] with
                | Number(n) -> n
                | Red -> 0
                | ArrayEnd -> c <- false
                              0
                | ArrayStart -> let s, j' = sumArray tokens (j+1)
                                j <- j'
                                s
                | _ -> 0
        j <- j + 1
        sum <- sum + s
    sum, j
     
let tokens = txt.ToCharArray() |> tokenize

let t2 = [ ArrayStart; Number(1); Number(3); ArrayStart; Number(2); Number(2); ArrayEnd; Red; Number(-7); ArrayEnd ]
Console.WriteLine($"{sumArray t2 1}")
