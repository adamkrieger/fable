namespace Fable

open System
open Fable

type PostBuilder () =

    static member createFromPostFile (postFile:PostFile.T) =

        let contentParser = new ContentParser()

        let title = contentParser.getTitle postFile.Content

        let content = contentParser.sanitizeForOutput postFile.Content

        Post.create 
            postFile.DatePosted
            title
            content
            postFile.FileTitle

    //Takes the file reference and pulls details from it
    //  Publish Date
    //  Output Filename
    //  Title
    //  Content after 'Fable' tags have been resolved
    static member buildBlogPostPartial (blogPostFile:BlogPostFile.T) =

        let fileName_publishDate = blogPostFile.FileName
                                   |> PostFunctions.parseFileNameAndSanitize

        let title_content = blogPostFile.Contents
                            |> PostFunctions.parseFableContent

        BlogPostPartial.create
            (fst fileName_publishDate)
            (snd fileName_publishDate)
            (fst title_content)
            (snd title_content)

    //Builds the partial into a full page with layout and style
    static member buildBlogPostPage 
                    (layoutParser:LayoutParser) 
                    (commonMarkParser:CommonMark.CommonMarkParser) 
                    (blogPostPartial:BlogPostPartial.T) = 

        let htmlWithLayout = 
                match blogPostPartial.Content with
                        | CommonMarkContent x -> commonMarkParser.compile x
                        | HtmlContent x -> x
                |> (layoutParser.compilePost 
                        blogPostPartial.Title
                        blogPostPartial.PublishDate
                   )


        BlogPost.create 
            blogPostPartial.FileNameForWeb
            blogPostPartial.PublishDate
            blogPostPartial.Title
            htmlWithLayout