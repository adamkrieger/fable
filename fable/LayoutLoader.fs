module Fable.LayoutLoader

open FileSystem

let getLayoutFilePath rootDir themeName =
    combinePaths [| rootDir; "themes"; "default"; "layout.html" |]

let getDefaultLayoutTemplate rootDir =
    getLayoutFilePath rootDir "default" 
        |> getFileContents 

