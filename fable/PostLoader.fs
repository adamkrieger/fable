module Fable.PostLoader

    open FileSystem
    open System
    open FParsec

    let filterPostFiles fileList =
        fileList |> Array.filter (fun x -> 
                                    match getExtension x with
                                    | ".html" -> true
                                    | ".md" -> true
                                    |_ -> false)

    let loadPosts rootDir destinationRootDir =
        
        let sourcePostDir = 
            (combinePaths [| rootDir; "posts" |])
                |> createDirectoryIfItDoesNotExist

        let postFiles = 
            getAllFilesInDirectory sourcePostDir
            |> filterPostFiles

        let posts = 
                postFiles 
                |> Array.map (fun postFilePath -> 
                                    PostBuilder.createFromFilePath postFilePath
                                                |> PostBuilder.createFromPostFile)

        posts