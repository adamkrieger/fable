module Fable.PostLoader

open FileSystem

let filterPostFiles fileList =
    fileList |> List.filter (fun x -> 
                                match getExtension x with
                                | ".html" -> true
                                | ".md" -> true
                                |_ -> false)

let loadPosts rootDir =
    
    let sourcePostDir = rootDir
                        |> addPostsDir
                        |> createDirectoryIfItDoesNotExist

    1