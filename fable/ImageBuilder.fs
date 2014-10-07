module ImageBuilder

    open Fable
    open FileSystem
    open System.IO

    let buildImages rootDir outputDir =

        let sourceImageDir = 
            combinePaths [| rootDir; "img" |]
            |> createDirectoryIfItDoesNotExist

        let outputImageDir = 
            combinePaths [| outputDir; "img" |]
            |> createDirectoryIfItDoesNotExist

        let imageFiles = 
            getAllFilesInDirectory sourceImageDir
            |> Array.filter (fun path -> path.EndsWith(".jpeg")
                                         || path.EndsWith(".jpg")
                                         || path.EndsWith(".gif")
                                         || path.EndsWith(".png")
                                         || path.EndsWith(".ico")
            )
            |> Array.map (fun path -> (
                                         path,
                                         Path.Combine(outputImageDir, (Path.GetFileName path))
                                      )
                         )

        do imageFiles 
           |> Array.map (fun page -> copySourceToDestination page)
           |> ignore
