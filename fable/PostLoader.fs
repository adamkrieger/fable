module Fable.PostLoader

open FileSystem
open System
open FParsec

let str = pstring

let filterPostFiles fileList =
    fileList |> Array.filter (fun x -> 
                                match getExtension x with
                                | ".html" -> true
                                | ".md" -> true
                                |_ -> false)

let getDateFromPostFileName (postFileName:string) =
    let indexOfFirstSpace = postFileName.IndexOf(' ')
    let dateAsString = postFileName.Substring(0, indexOfFirstSpace)
    DateTime.Parse(dateAsString)

let parseTitleFromContent fileContent =

    let parser =
        manyCharsTill anyChar (lookAhead (str "[#"))
        >>.
        between 
                (str "[# title: ")
                (str " #]")
                (manyCharsTill anyChar (lookAhead (str " #]")))
    
    let result = run parser fileContent

    match result with
    | Success(output,_,_) -> output
    | Failure(msg,_,_) -> "Untitled"

let parseContentForOutput fileContent =
    
    let parser =
        [
            attempt 
                (between 
                    (str "[#") 
                    (str "#]") 
                    (manyCharsTill 
                        anyChar 
                        (lookAhead (str "#]"))
                    )
                ) |>> (fun x -> "")
            attempt 
                (manyCharsTill 
                    anyChar 
                    (lookAhead (str "[#"))
                )
            manyChars anyChar
        ]
        |> choice

    let iterator = manyTill parser eof

    let result = run iterator fileContent

    match result with
    | Success(output,_,_) -> List.reduce (fun a b -> a + b) output
    | Failure(msg,_,_) -> "Parser failed: " + msg

let assemblePost filePath getFileContents =
    let fileContent = getFileContents filePath
    let fileName = getFileName filePath
    
    let date = getDateFromPostFileName fileName

    let title = parseTitleFromContent fileContent

    let content = parseContentForOutput fileContent
    
    Post.create date title content

let loadPosts rootDir =
    
    let sourcePostDir = rootDir
                        |> addPostsDir
                        |> createDirectoryIfItDoesNotExist

    let filesInSourceDir = getAllFilesInDirectory sourcePostDir

    let postFiles = filterPostFiles filesInSourceDir

    Array.map (fun postFilePath -> assemblePost postFilePath getFileContents) postFiles