open fable
open System
open System.Configuration
open System.IO
open FileSystem
open PageBuilder

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let rootDir = ConfigurationManager.AppSettings.Item("rootDirectory") 
                |> createDirectoryIfItDoesNotExist

    let outputDir = rootDir 
                 |> addOutputDirectorySuffix 
                 |> createDirectoryIfItDoesNotExist
    
    let htmlFilesInRoot = IO.Directory.GetFiles(rootDir, "*.html") 

    printfn "%A" htmlFilesInRoot
    
    let pages = htmlFilesInRoot 
                |> mapFilesToPages outputDir

    let defaultLayoutTemplate = getDefaultLayoutTemplate rootDir

    let tagParser = new TagParser(defaultLayoutTemplate)

    do pages 
       |> Array.map (fun page -> buildPages tagParser page)
       |> ignore

    let sourceCssDir = rootDir
                       |> addThemesDir
                       |> addSelectedThemeDir
                       |> addStyleDir
                       |> createDirectoryIfItDoesNotExist

    let outputCssDir = outputDir
                       |> addStyleDir
                       |> createDirectoryIfItDoesNotExist

    do StyleBuilder.buildStyle sourceCssDir outputCssDir

    let sourceImageDir = rootDir
                         |> addImageDir
                         |> createDirectoryIfItDoesNotExist

    let outputImageDir = outputDir
                         |> addImageDir
                         |> createDirectoryIfItDoesNotExist

    do ImageBuilder.buildImages sourceImageDir outputImageDir

    let result = System.Console.ReadLine();

    0 // return an integer exit code
