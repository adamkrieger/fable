// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open System.IO
open System.Configuration

let createDirectoryIfItDoesNotExist (dir)=
    match Directory.Exists(dir) with
    | false -> Directory.CreateDirectory(dir) |> ignore
    | _ -> None |> ignore
    dir

let addOutputDirectorySuffix (x)= x + "_bin\\"

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let root = ConfigurationManager.AppSettings.Item("rootDirectory") |>
                createDirectoryIfItDoesNotExist

    let output = root |> 
                    addOutputDirectorySuffix |> 
                    createDirectoryIfItDoesNotExist
    
    use fs = new System.IO.StreamWriter(output + "index.html")

    fs.Write("<html></html>")

    let result = System.Console.ReadLine();

    0 // return an integer exit code
