module Fable.LayoutLoader

    open FileSystem

    let getLayoutFilePath rootDir themeName layoutName =
        combinePaths [| rootDir; "themes"; themeName; layoutName |]

    let getPageLayout rootDirectory themeName =
        getLayoutFilePath rootDirectory "default" "layout.html"
            |> getFileContents 

    let getPostLayout rootDirectory themeName = 
        getLayoutFilePath rootDirectory "default" "post.html"
            |> getFileContents