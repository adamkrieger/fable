open Fable
open System
open System.Configuration
open FileSystem

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let rootDir = ConfigurationManager.AppSettings.Item("rootDirectory") 
                |> createDirectoryIfItDoesNotExist

    let outputDir = rootDir 
                 |> addOutputDirectorySuffix 
                 |> createDirectoryIfItDoesNotExist

    let postContent = PostLoader.loadPosts rootDir

    //do PostBuilder.buildPosts postContent outputDir
    
    do PageBuilder.buildAllPages rootDir outputDir

    do StyleBuilder.buildStyle rootDir outputDir

    do ImageBuilder.buildImages rootDir outputDir

    0 // return an integer exit code
