module PageBuilder

open Fable
open System.IO
open FileSystem

let transformSourcePathToDestination destinationRoot filePath =
    let filename = Path.GetFileName filePath
    Path.Combine(destinationRoot, filename)

let getPageType destinationRoot filePath =
    let destinationPath = transformSourcePathToDestination destinationRoot filePath

    {
        sourcePath = filePath; 
        destinationPath = destinationPath 
    }

let mapFilesToPages outputDirectoryRoot filenameArray =
    filenameArray 
    |> Array.map (fun filename -> getPageType outputDirectoryRoot filename)

let buildPages (tagParser:TagParser) pageInfo =
    let {
            sourcePath = inputPath; 
            destinationPath = outputPath
        } = pageInfo
    
    use reader = new StreamReader(inputPath)
    let input = reader.ReadToEnd()

    let output = tagParser.compile input

    use writer = new StreamWriter(outputPath)
    writer.Write(output)

let buildAllPages rootDir outputDir =
    let htmlFilesInRoot = Directory.GetFiles(rootDir, "*.html") 

    printfn "%A" htmlFilesInRoot
    
    let pages = htmlFilesInRoot 
                |> mapFilesToPages outputDir

    let defaultLayoutTemplate = getDefaultLayoutTemplate rootDir

    let tagParser = new TagParser(defaultLayoutTemplate)

    do pages 
       |> Array.map (fun page -> buildPages tagParser page)
       |> ignore
    