module PageBuilder

open fable
open System.IO

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
    let {sourcePath=inputPath;destinationPath=outputPath} = pageInfo
    
    use reader = new StreamReader(inputPath)
    let input = reader.ReadToEnd()

    let output = tagParser.compile input

    use writer = new StreamWriter(outputPath)
    writer.Write(output)
    