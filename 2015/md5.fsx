open System
open System.Collections
open System.Text

let s = [| 7; 12; 17; 22;  7; 12; 17; 22;  7; 12; 17; 22;  7; 12; 17; 22;
           5;  9; 14; 20;  5;  9; 14; 20;  5;  9; 14; 20;  5;  9; 14; 20;
           4; 11; 16; 23;  4; 11; 16; 23;  4; 11; 16; 23;  4; 11; 16; 23;
           6; 10; 15; 21;  6; 10; 15; 21;  6; 10; 15; 21;  6; 10; 15; 21 |]

let k =[| 0xd76aa478; 0xe8c7b756; 0x242070db; 0xc1bdceee;
          0xf57c0faf; 0x4787c62a; 0xa8304613; 0xfd469501;
          0x698098d8; 0x8b44f7af; 0xffff5bb1; 0x895cd7be;
          0x6b901122; 0xfd987193; 0xa679438e; 0x49b40821;
          0xf61e2562; 0xc040b340; 0x265e5a51; 0xe9b6c7aa;
          0xd62f105d; 0x02441453; 0xd8a1e681; 0xe7d3fbc8;
          0x21e1cde6; 0xc33707d6; 0xf4d50d87; 0x455a14ed;
          0xa9e3e905; 0xfcefa3f8; 0x676f02d9; 0x8d2a4c8a;
          0xfffa3942; 0x8771f681; 0x6d9d6122; 0xfde5380c;
          0xa4beea44; 0x4bdecfa9; 0xf6bb4b60; 0xbebfbc70;
          0x289b7ec6; 0xeaa127fa; 0xd4ef3085; 0x04881d05;
          0xd9d4d039; 0xe6db99e5; 0x1fa27cf8; 0xc4ac5665;
          0xf4292244; 0x432aff97; 0xab9423a7; 0xfc93a039;
          0x655b59c3; 0x8f0ccc92; 0xffeff47d; 0x85845dd1;
          0x6fa87e4f; 0xfe2ce6e0; 0xa3014314; 0x4e0811a1;
          0xf7537e82; 0xbd3af235; 0x2ad7d2bb; 0xeb86d391; |]


let baPrint arr =
    arr |> Array.map(fun v -> if v then '1'
                              else '0')
        |> Array.iter Console.Write
    Console.WriteLine()
    
// helper function to turn BitArray into a standard
// array of bools
let baConvert (current:BitArray) =
    let arr = seq {
        for bit in current do
        yield bit
    } 

    // I think I need to reserve it to switch from big to little endian,
    // at least on Windows...
    arr |> Seq.toArray |> Array.rev

// For purposes of md5 I can hard code for 32-bit ints
let rotateLeft x shift =
    let left = x <<< shift
    let right = x >>> (32 - shift)
    left ||| right
    
let padArray (arr: bool array) =
    let arrLength = (uint64 (arr.Length % 512))
    let padding = if arrLength <= 448UL then 448UL - arrLength
                      else 512UL - arrLength + 48UL
    
    // - 1 because of single pad bit
    let zeroes = [| for j in 1UL .. padding  - 1UL -> false |]

    // padding the length of the original message
    let x = (uint64 arr.Length) &&& 0xffffffffffffffffUL
    let lengthPadding = BitConverter.GetBytes x
                        |> BitArray
                        |> baConvert
                        
    let padded = lengthPadding
                 |> Array.append zeroes
                 |> Array.append [| true |]
                 |> Array.append arr
             
    padded

    // Takes a bool array of length 32 and converts it to a unsigned int32
let toWord bits =
    let foldToWord = Array.fold(fun (num, s) b ->
                           let next = if b then num ||| (1u <<< s)
                                      else num
                           (next, s - 1))
    let word, _ = foldToWord (0u, 31) bits
    word
    
let md5Hash (message:string) =
    let bytes = Encoding.ASCII.GetBytes(message) // I *think* this is returning the bytes in Big Endian, but baConvert switches 
                                                 // the input to Little Endian. Or is this going to be a Windows vs ARM on macOS thing??
    let bits = BitArray bytes |> baConvert
    let padded = padArray bits

    // splitting the 512 bit array into 16 chunks is nice and easy with splitInto
    padded |> Array.splitInto 16 |> Array.map toWord |> Array.iter (fun w -> Console.WriteLine($"%u{w}"))
                                                                                               
    //let m512 = padded.Length % 512
    //Console.WriteLine($"%d{bits.Length} %d{padded.Length} %d{m512}")
    
let message = "The quick brown fox jumped over the lazy yellow dog and then fell in a puddle lorem ipsum"
//let message = "a"
md5Hash message

let x = 7
let y = rotateLeft x 5
Console.WriteLine()
Console.WriteLine($"%u{y}")




