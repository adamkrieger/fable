module Fable.PostLoader

    open FileSystem
    open System
    open FParsec

    type Extension = string

    let matchesExtension (extension:Extension) (filePath:string) =
        match filePath |> FileSystem.getExtension with
            | @".md" -> true
            |_ -> false

    let readBlogPostFile (filePath:string) =

        let fileName = FileSystem.getFileName

        BlogPostFile.create 

    let mapToBlogPostFileName filePath =

        let ext = filePath 
                    |> FileSystem.getExtension

        let ret = match ext with
                    | @".md" -> Some (CommonMarkFilePath filePath)
                    | @".html" -> Some (HtmlFilePath filePath)
                    |_ -> None
        
        ret

    let isNotNone input =
        match input with
        | Some x -> true
        |_ -> false
            
    //Look for all html files in posts
    //create file references for them
    //Look for all md files in posts
    //create file references for them

    let loadPostFile blogFilePath = 

        let fileName, contents =
            match blogFilePath with
            | CommonMarkFilePath x -> 
                    (x |> FileSystem.getFileName)
                    , (UnparsedCommonMark (x |> FileSystem.getFileContents))
            | HtmlFilePath x -> 
                    (x |> FileSystem.getFileName)
                    , (UnparsedHtml (x |> FileSystem.getFileContents))

        BlogPostFile.create 
            fileName
            contents

    let readBlogPostFiles rootDir =
        
        let sourcePostDirectory = 
            (combinePaths [| rootDir; @"posts" |])
                |> createDirectoryIfItDoesNotExist

        let filesInPostDirectory = sourcePostDirectory
                                    |> getAllFilesInDirectory

        filesInPostDirectory
            |> Array.choose
                    (fun filePath -> filePath 
                                     |> mapToBlogPostFileName)
            |> Array.map
                    (fun blogFile -> blogFile
                                     |> loadPostFile)
                            