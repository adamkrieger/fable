open System
open System.Configuration
open FileSystem
open Fable

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let rootDir = ConfigurationManager.AppSettings.Item("rootDirectory") 
                |> createDirectoryIfItDoesNotExist

    let outputDir = 
        combinePaths [| rootDir; "bin" |]
        |> createDirectoryIfItDoesNotExist

    let pageLayout = LayoutLoader.getDefaultLayoutTemplate rootDir

    let layoutParser = PageBuilder.createLayoutParser pageLayout

    let commonMarkParser = new CommonMark.CommonMarkParser()

    let formatPages = PostBuilder.buildBlogPostPage layoutParser commonMarkParser

    let outputPathCurry = FileSystem.combinePath outputDir

    do (PostLoader.readBlogPostFiles rootDir)
                    |> Array.map (fun x -> x |> PostBuilder.buildBlogPostPartial)
                    |> Array.map (fun x -> x |> formatPages)
                    |> Array.map (fun x -> FileSystem.writeToFile
                                                (x.FileNameForWeb |> outputPathCurry)
                                                x.HtmlPage) 
                    |> ignore


    
    do PageBuilder.buildAllPages rootDir outputDir layoutParser

    do StyleBuilder.buildStyle rootDir outputDir

    do ImageBuilder.buildImages rootDir outputDir

    0 // return an integer exit code
