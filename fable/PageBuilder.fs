module PageBuilder

    open Fable
    open FileSystem

    let createLayoutParser pageLayout =
        new LayoutParser(pageLayout)
        
    let compilePage fileName contents (parser:LayoutParser) destinationRoot =
        
        let destinationPath = combinePath destinationRoot fileName

        let output = parser.compile contents

        Page.create destinationPath output

    let buildAllPages rootDir destinationDir (parser:LayoutParser) =
        
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

    let buildPageFromPost rootDir (post:Post.T) =

        PreLayoutPage.create
            (combinePaths [| rootDir; post.PathRelative |])
            post.Content

    let applyLayoutAndWritePages (pages:PreLayoutPage.T[]) (parser:LayoutParser) =
        do pages 
           |> Array.map (fun page -> 
                                    writeToFile
                                        page.DestinationPath
                                        (parser.compile page.PreLayoutContent)
                           )
           |> ignore