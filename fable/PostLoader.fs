module Fable.PostLoader

open FileSystem
open System

let filterPostFiles fileList =
    fileList |> List.filter (fun x -> 
                                match getExtension x with
                                | ".html" -> true
                                | ".md" -> true
                                |_ -> false)

let assemblePost filePath getFileContents =
    let fileContent = getFileContents filePath

    Post.create DateTime.Now filePath fileContent

let loadPosts rootDir =
    
    let sourcePostDir = rootDir
                        |> addPostsDir
                        |> createDirectoryIfItDoesNotExist

    1