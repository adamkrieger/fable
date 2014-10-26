open System
open System.Configuration
open FileSystem
open Fable

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let rootDir = ConfigurationManager.AppSettings.Item("rootDirectory") 
                |> createDirectoryIfItDoesNotExist

    let theme = ConfigurationManager.AppSettings.Item("theme")

    let outputDir = 
        combinePaths [| rootDir; "bin" |]
        |> createDirectoryIfItDoesNotExist

    let pageLayout = LayoutLoader.getPageLayout rootDir theme
    let postLayout = LayoutLoader.getPostLayout rootDir theme

    let pageLayoutParser = PageBuilder.createLayoutParser pageLayout
    let postLayoutParser = PageBuilder.createLayoutParser postLayout

    let commonMarkParser = new CommonMark.CommonMarkParser()

    let formatPages = PostBuilder.buildBlogPostPage postLayoutParser commonMarkParser

    let outputPathCurry = FileSystem.combinePath outputDir

    do (PostLoader.readBlogPostFiles rootDir)
                    |> Array.map (fun x -> x |> PostBuilder.buildBlogPostPartial)
                    |> Array.map (fun x -> x |> formatPages)
                    |> Array.map (fun x -> FileSystem.writeToFile
                                                (x.FileNameForWeb |> outputPathCurry)
                                                x.HtmlPage) 
                    |> ignore


    
    do PageBuilder.buildAllPages rootDir outputDir pageLayoutParser

    do StyleBuilder.buildStyle rootDir outputDir

    do ImageBuilder.buildImages rootDir outputDir

    0 // return an integer exit code
