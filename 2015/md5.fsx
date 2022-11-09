open System
open System.Collections
open System.Text

// A type to hold the 4 32-bit words the MD5 algorithm manipulates
type md5 = { a:uint32; b:uint32; c:uint32; d:uint32 }

let s = [| 7; 12; 17; 22;  7; 12; 17; 22;  7; 12; 17; 22;  7; 12; 17; 22;
           5;  9; 14; 20;  5;  9; 14; 20;  5;  9; 14; 20;  5;  9; 14; 20;
           4; 11; 16; 23;  4; 11; 16; 23;  4; 11; 16; 23;  4; 11; 16; 23;
           6; 10; 15; 21;  6; 10; 15; 21;  6; 10; 15; 21;  6; 10; 15; 21 |]

let k =[| 0xd76aa478u; 0xe8c7b756u; 0x242070dbu; 0xc1bdceeeu;
          0xf57c0fafu; 0x4787c62au; 0xa8304613u; 0xfd469501u;
          0x698098d8u; 0x8b44f7afu; 0xffff5bb1u; 0x895cd7beu;
          0x6b901122u; 0xfd987193u; 0xa679438eu; 0x49b40821u;
          0xf61e2562u; 0xc040b340u; 0x265e5a51u; 0xe9b6c7aau;
          0xd62f105du; 0x02441453u; 0xd8a1e681u; 0xe7d3fbc8u;
          0x21e1cde6u; 0xc33707d6u; 0xf4d50d87u; 0x455a14edu;
          0xa9e3e905u; 0xfcefa3f8u; 0x676f02d9u; 0x8d2a4c8au;
          0xfffa3942u; 0x8771f681u; 0x6d9d6122u; 0xfde5380cu;
          0xa4beea44u; 0x4bdecfa9u; 0xf6bb4b60u; 0xbebfbc70u;
          0x289b7ec6u; 0xeaa127fau; 0xd4ef3085u; 0x04881d05u;
          0xd9d4d039u; 0xe6db99e5u; 0x1fa27cf8u; 0xc4ac5665u;
          0xf4292244u; 0x432aff97u; 0xab9423a7u; 0xfc93a039u;
          0x655b59c3u; 0x8f0ccc92u; 0xffeff47du; 0x85845dd1u;
          0x6fa87e4fu; 0xfe2ce6e0u; 0xa3014314u; 0x4e0811a1u;
          0xf7537e82u; 0xbd3af235u; 0x2ad7d2bbu; 0xeb86d391u; |]


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
    arr |> Seq.toArray // |> Array.rev

// For purposes of md5 I can hard code for 32-bit ints
let rotateLeft x shift =
    let left = x <<< shift
    let right = x >>> (32 - shift)
    left ||| right
    
let padArray (arr: byte array) =
    let arrLengthBits = (uint64 arr.Length) * 8UL
    let arrLength = arr.Length % 64 + 1
    let padding = if arrLength <= 56 then 56 - arrLength
                  else 64 - arrLength + 8
    
    // - 1 because of single pad bit
    let zeroes = [| for j in 1 .. padding -> 0x00uy |]

    let x = (uint64 arr.Length) * 8UL
    let lengthPadding = BitConverter.GetBytes(x)

    let padded = lengthPadding
                 |> Array.append zeroes
                 |> Array.append [| 0x80uy |]
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

let hexString md5 =
    md5.a.ToString("X2") + md5.b.ToString("X2") + md5.c.ToString("X2") + md5.d.ToString("X2")
    
// One iteration of the main loop, as defined in Wikipedia page for MD5
// (Using Wiki's variable naming convention here even though it violates
// standard convention)
let rec mainLoop (words: uint array) i md5 =
    let F, g = if i <= 15 then (md5.b &&& md5.c) ||| ((~~~md5.b) &&& md5.d), i
               elif i <= 31 then (md5.d &&& md5.b) ||| ((~~~md5.d) &&& md5.c), (5*i + 1) % 16
               elif i <= 47 then md5.b ^^^ md5.c ^^^ md5.d, (3*i + 5) % 16
               else md5.c ^^^ (md5.b ||| (~~~md5.d)), (7*i) % 16

    let F' = F + md5.a + k[i] + words[g]
    let b' = md5.b + (rotateLeft F' s[i])
    let md5' = { a = md5.d; b = b'; c = md5.b; d = md5.c }
    if i = 63 then md5'
    else mainLoop words (i + 1) md5'

let md5Hash (message:string) =
    // I *think* this is returning the bytes in Big Endian, but baConvert switches 
    // the input to Little Endian. Or is this going to be a Windows vs ARM on macOS thing??
    let bytes = Encoding.ASCII.GetBytes(message) |> padArray
    
    //let b2 = bytes |> Array.map(fun x -> [| x |]) |> Array.map BitArray // |> baConvert

    //let bits = BitArray bytes |> baConvert
    //let padded = padArray bits

    // splitting the 512 bit array into 16 chunks is nice and easy with splitInto
    //let words = padded |> Array.splitInto 16 |> Array.map toWord 

    //words |> Array.iter Console.WriteLine
    
    //let result = mainLoop words 0 { a = 0x67452301u; b = 0xefcdab89u; c = 0x98badcfeu; d = 0x10325476u }

    let result = { a = 0x67452301u; b = 0xefcdab89u; c = 0x98badcfeu; d = 0x10325476u }
    Console.WriteLine(hexString result)
                                                                                               
    //let m512 = padded.Length % 512
    //Console.WriteLine($"%d{bits.Length} %d{padded.Length} %d{m512}")
    
//let message = "The quick brown fox jumped over the lazy yellow dog and then fell in a puddle lorem ipsum"
let message = "abcdef"
md5Hash message

//let x = 7
//let y = rotateLeft x 5
//Console.WriteLine()
//Console.WriteLine($"%u{y}")




