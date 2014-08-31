open fable
open System
open System.Configuration
open System.IO
open FileSystem
open PageBuilder

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let root = ConfigurationManager.AppSettings.Item("rootDirectory") |>
                createDirectoryIfItDoesNotExist

    let output = root |> 
                    addOutputDirectorySuffix |> 
                    createDirectoryIfItDoesNotExist
    
    let fileList = IO.Directory.GetFiles root

    printfn "%A" fileList
    
    let pages = fileList |> mapFilesToPages output

    let defaultLayoutTemplate = getDefaultLayoutTemplate root

    let tagParser = new TagParser(defaultLayoutTemplate)

    let outFiles = pages |> Array.map (fun page -> buildPages tagParser page)

    let result = System.Console.ReadLine();

    0 // return an integer exit code
