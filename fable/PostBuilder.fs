namespace Fable

open System

type PostBuilder () =

    static member createFromFilePath filePath =

        let content = FileSystem.getFileContents filePath
        let fileName = FileSystem.getFileName filePath

        let parsedPostFileName = Fable.PostFunctions.parsePostFileName fileName

        PostFile.create
            (fst parsedPostFileName)
            (snd parsedPostFileName)
            content
            
    static member createFromPostFile (postFile:PostFile.T) =

        let contentParser = new ContentParser(postFile.Content)

        let title = contentParser.getTitleFromContent

        let content = contentParser.sanitizeContentForOutput

        let pathRelative = PostFunctions.buildOutputPath 
                                postFile

        Post.create 
            postFile.DatePosted
            title
            content
            pathRelative