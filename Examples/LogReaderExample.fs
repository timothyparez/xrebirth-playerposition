
(* An example showing how one might go about extracting data from the
   debug log while it's constantly being updated. It also acts as a proxy
   server that relays the position information. This is an example only
   it has not been tested in any way for use in production code *)

open System
open System.IO
open System.Net
open System.Net.Sockets;
open System.Text;
open System.Text.RegularExpressions

let (^?) a b = Regex.IsMatch(a, b, RegexOptions.IgnorePatternWhitespace ||| RegexOptions.Multiline)
let (^!) a b = Regex.Match(a, b, RegexOptions.IgnorePatternWhitespace ||| RegexOptions.Multiline)
let (^@) (a:Match) (b:int) = a.Groups.[b].Value

let regex = @"\[=ERROR=\].*position.\[(?<x>\S*)m,\s(?<y>\S*)m,\s(?<z>\S*)m\]";
let blockSize = 1024l;

let ReadTail filename =    
    use fileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
    fileStream.Seek(-(int64)blockSize, SeekOrigin.End) |> ignore
    let buffer = Array.zeroCreate(blockSize)
    fileStream.Read(buffer, 0, blockSize) |> ignore
    Encoding.UTF8.GetString(buffer);

let GetPositionsAsXML() = 
    ReadTail(@"debug.log")
    |> fun t -> t.Split([|"\r\n"|], StringSplitOptions.None)
    |> Seq.filter(fun l -> l ^? regex)
    |> Seq.map(fun l -> l ^! regex)
    |> Seq.map(fun m -> String.Format("\t<position x=\"{0}\" y=\"{1}\" z=\"{2}\"/>", m^@1, m^@2, m^@3))
    |> String.concat "\r\n"
    |> sprintf "<?xml version=\"1.0\"?>\r\n<positions>\r\n%s\r\n</positions>"


if File.Exists("debug.log") then
    let server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
    server.Bind(new IPEndPoint(IPAddress.Any, 32961))
    server.Listen(2)

    while true do
        use client = server.Accept()
        let positions = GetPositionsAsXML()
        let positionData = Encoding.UTF8.GetBytes(positions)
        client.Send(positionData) |> ignore
        client.Disconnect(false)
    
else
    printfn "Please put this application in the same directory as the 'debug.log' file"
    

