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

    let parser = PageBuilder.createLayoutParser pageLayout

    let postContent = PostLoader.loadPosts rootDir outputDir

    let postPages = postContent 
                    |> Array.map (fun post -> PageBuilder.buildPageFromPost rootDir post)

    do PageBuilder.applyLayoutAndWritePages postPages parser

    //do PostBuilder.buildPosts postContent outputDir
    
    do PageBuilder.buildAllPages rootDir outputDir parser

    do StyleBuilder.buildStyle rootDir outputDir

    do ImageBuilder.buildImages rootDir outputDir

    0 // return an integer exit code
