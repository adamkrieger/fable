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
            post.FileName
            post.Content

    let fitContentInLayout (page:PreLayoutPage.T) (parser:LayoutParser) =
        parser.compile page.PreLayoutContent

    let getOutputPath (page:PreLayoutPage.T) outputRoot =
        let outputPath =
            outputRoot
                |> createDirectoryIfItDoesNotExist

        combinePath
            outputPath
            page.FileName

    let writePage path content =
        printfn "%A" ("Writing file: " + path)

        writeToFile path content

    let applyLayoutAndWritePages (pages:PreLayoutPage.T[]) (parser:LayoutParser) outputRoot =
        do pages 
           |> Array.map (fun page -> 
                                    writePage
                                        (getOutputPath page outputRoot)
                                        (fitContentInLayout page parser)
                           )
           |> ignore