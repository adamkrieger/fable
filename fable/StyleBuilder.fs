module StyleBuilder

    open Fable
    open System.IO
    open FileSystem

    let buildStyle rootDir outputDir =

        let sourceCssDir = 
            (combinePaths [| rootDir; "themes"; "default"; "css" |])
                |> createDirectoryIfItDoesNotExist

        let outputCssDir = 
            (combinePaths [| outputDir; "css" |])
                |> createDirectoryIfItDoesNotExist

        let styleSheetFiles = 
            Directory.GetFiles(sourceCssDir, "*.css")
            |> Array.map (fun path -> ( 
                                         path,
                                         Path.Combine(outputCssDir, (Path.GetFileName path))
                                      )
                         )

        do styleSheetFiles 
           |> Array.map (fun page -> copySourceToDestination page)
           |> ignore


