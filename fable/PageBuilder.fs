module PageBuilder

open Fable
open FileSystem

let transformSourcePathToDestination destinationRoot filePath =
    let fileName = getFileName filePath
    combinePathWithFileName destinationRoot fileName

let getPageType destinationRoot filePath =
    let destinationPath = transformSourcePathToDestination destinationRoot filePath

    {
        Page.SourcePath = filePath; 
        Page.DestinationPath = destinationPath 
    }

let mapFilesToPages outputDirectoryRoot filenameArray =
    filenameArray 
    |> Array.map (fun filename -> getPageType outputDirectoryRoot filename)



let buildPages (tagParser:TagParser) pageInfo =
    let {
            Page.SourcePath = inputPath; 
            Page.DestinationPath = outputPath
        } = pageInfo
    
    let input = getFileContents inputPath

    let output = tagParser.compile input

    writeToFile outputPath output

let buildAllPages rootDir outputDir =
    let htmlFilesInRoot = getHtmlFilesInDirectory rootDir

    printfn "%A" htmlFilesInRoot
    
    let pages = htmlFilesInRoot 
                |> mapFilesToPages outputDir

    let defaultLayoutTemplate = getDefaultLayoutTemplate rootDir

    let tagParser = new TagParser(defaultLayoutTemplate)

    do pages 
       |> Array.map (fun page -> buildPages tagParser page)
       |> ignore
    