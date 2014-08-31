open fable
open System
open System.Configuration
open System.IO
open FileSystem

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
    
    let archiveReader = new StreamReader(fileList.[0]) 
    let indexReader = new StreamReader(fileList.[1])
    let layoutReader = new StreamReader(root + "\\_themes\\default\\default.html")

    let tagParser = new TagParser(layoutReader.ReadToEnd())

    let indexOutput = tagParser.compile(indexReader.ReadToEnd())
    let archiveOuput = tagParser.compile(archiveReader.ReadToEnd())

    let indexWriter = new StreamWriter(output + "\\index.html")
    let archiveWriter = new StreamWriter(output + "\\archive.html")

    indexWriter.Write(indexOutput)
    indexWriter.Close()
    archiveWriter.Write(archiveOuput)
    archiveWriter.Close()

    let result = System.Console.ReadLine();

    0 // return an integer exit code
