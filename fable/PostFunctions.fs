module Fable.PostFunctions

    open System
    open FileSystem
    open Fable


    let buildOutputFileName (fileTitle:string) =
        let saniTitle = fileTitle.Trim()
                            .Replace(' ','_')
                            .Replace(':','_')
                            .Replace("__","_")
        saniTitle + ".html"

    let parsePostFileName (postFileName:string) =

        let name = FileSystem.getFileNameWithoutExtension postFileName

        let indexOfFirstSpace = name.IndexOf(' ')

        let dateAsString = name.Substring(0, indexOfFirstSpace)
        let datePosted = DateTime.Parse(dateAsString)

        let postName = name.Substring(indexOfFirstSpace, (name.Length - indexOfFirstSpace))
                       |> buildOutputFileName

        postName, datePosted


    let buildOutputPath (post:PostFile.T) =
        FileSystem.combinePaths 
                    [|
                        "posts";
                        post.DatePosted.ToString("yyyy");
                        post.DatePosted.ToString("MM");
                        post.DatePosted.ToString("dd");
                        post.FileTitle
                    |]