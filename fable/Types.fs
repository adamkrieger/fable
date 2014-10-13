namespace Fable

open System

module Page =
    type T = {
        DestinationPath : string;
        Content : string;
    }

    let create destinationPath content =
        {
            DestinationPath = destinationPath;
            Content = content;
        }

module PreLayoutPage =
    type T = 
        {
            FileName : string;
            PreLayoutContent : string;
        }

    let create fileName content =
        {
            FileName = fileName;
            PreLayoutContent = content;
        }

module PostFile = 
    type T = 
        {
            FileTitle : string;
            DatePosted : DateTime;
            Content : string;
        }

    let create fileTitle datePosted content = 
        {
            FileTitle = fileTitle;
            DatePosted = datePosted;
            Content = content;
        }

module Post =
    type T = 
        {
            DatePosted : DateTime;
            Title : string;
            Content : string;
            FileName : string;
        }

    let create datePosted title content fileName = 
        {
            DatePosted = datePosted;
            Title = title;
            Content = content;
            FileName = fileName;
        }


type BlogPostContent = 
    | CommonMarkContent of string 
    | HtmlContent of string

type HtmlPage = string

type UnparsedContent = 
        | UnparsedCommonMark of string
        | UnparsedHtml of string


type BlogPostFilePath = 
        | CommonMarkFilePath of string
        | HtmlFilePath of string

//CommonMarkFile is a *.md file's content loaded from the file system
module BlogPostFile =
    type T = 
        {
            FileName : string;
            Contents : UnparsedContent;
        }

    let create fileName contents =
        {
            FileName = fileName;
            Contents = contents;
        }


//BlogPostPartial is a reference to publication content.
// The content has probably not been sanitized or flattened
module BlogPostPartial =
    type T =
        {
            FileNameForWeb : string;
            Title : string;
            Content : BlogPostContent;
            PublishDate : DateTime;
        }

    let create fileNameForWeb publishDate title content =
        {
            FileNameForWeb = fileNameForWeb;
            Title = title;
            PublishDate = publishDate;
            Content = content;
        }

//BlogPost is a Post that is refined and completely parsed
module BlogPost =
    type T =
        {
            FileNameForWeb : string;
            PublishDate : DateTime;
            Title : string;
            HtmlPage : HtmlPage;
        }

    let create fileNameForWeb publishDate title content =
        {
            FileNameForWeb = fileNameForWeb;
            PublishDate = publishDate;
            Title = title;
            HtmlPage = content;
        }