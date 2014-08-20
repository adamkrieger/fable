// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open System.Configuration
open FileSystem

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let root = ConfigurationManager.AppSettings.Item("rootDirectory") |>
                createDirectoryIfItDoesNotExist

    let output = root |> 
                    addOutputDirectorySuffix |> 
                    createDirectoryIfItDoesNotExist
    
    use fs = new System.IO.StreamWriter(output + "index.html")

    let fileList = IO.Directory.GetFiles root

    printfn "%A" fileList

    let result = System.Console.ReadLine();

    0 // return an integer exit code
