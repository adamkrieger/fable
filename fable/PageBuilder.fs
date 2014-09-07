module PageBuilder

open Fable
open FileSystem

let getLayoutFilePath rootDir themeName =
    combinePaths [| rootDir; "themes"; "default"; "layout.html" |]

let getDefaultLayoutTemplate rootDir =
    getLayoutFilePath rootDir "default" 
        |> getFileContents 

let createLayoutParser pageLayout =
    new LayoutParser(pageLayout)
    
let compilePage fileName contents (parser:LayoutParser) destinationRoot =
    
    let destinationPath = combinePath destinationRoot fileName

    let output = parser.compile contents

    Page.create fileName destinationPath output

let buildAllPages rootDir destinationDir =
    
    let pageLayout = getDefaultLayoutTemplate rootDir

    let parser = createLayoutParser pageLayout

    let htmlFiles = getHtmlFilesInDirectory rootDir

    let compiledPages = Array.map (fun htmlFilePath -> 
                                            compilePage 
                                                (getFileName htmlFilePath)
                                                (getFileContents htmlFilePath)
                                                parser 
                                                destinationDir
                                   )
                                   htmlFiles

    do compiledPages
       |> Array.map (fun page -> writeToFile page.DestinationPath page.Content)
       |> ignore
