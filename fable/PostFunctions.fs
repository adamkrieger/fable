module Fable.PostFunctions

    open System
    open FileSystem
    open Fable


    let buildSanitizedHtmlFileName (fileTitle:string) =
        let saniTitle = fileTitle.Trim()
                            .Replace(' ','_')
                            .Replace(@"'","")
                            .Replace(':','_')
                            .Replace("__","_")
        saniTitle + ".html"

    type BlogPostFileName = string
    type PublishDate = DateTime

    let parsePostFileName (fileNameBuilder:string -> string) (postFileName:string) =

        let name = FileSystem.getFileNameWithoutExtension postFileName

        let indexOfFirstSpace = name.IndexOf(' ')

        let dateAsString = name.Substring(0, indexOfFirstSpace)
        let datePosted = DateTime.Parse(dateAsString)

        let postName = name.Substring(indexOfFirstSpace, (name.Length - indexOfFirstSpace))
                       |> fileNameBuilder

        (postName:BlogPostFileName), (datePosted:PublishDate)

    let parseFileNameAndSanitize fileName =

        //curry the parser with a filename sanitizer
        let fileNameParser = 
                buildSanitizedHtmlFileName |> parsePostFileName

        fileName |> fileNameParser

    let parseFableContent (content:UnparsedContent) = 

        let contentParser = new ContentParser()

        let title, parsed =
            match content with
            | UnparsedCommonMark x -> 
                (contentParser.getTitle x)
                , (CommonMarkContent (contentParser.sanitizeForOutput x))
            | UnparsedHtml x ->
                (contentParser.getTitle x)
                , (HtmlContent (contentParser.sanitizeForOutput x))

        title, parsed