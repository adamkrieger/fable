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
            DestinationPath : string;
            PreLayoutContent : string;
        }

    let create destinationPath content =
        {
            DestinationPath = destinationPath;
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
            PathRelative : string;
        }

    let create datePosted title content pathRelative = 
        {
            DatePosted = datePosted;
            Title = title;
            Content = content;
            PathRelative = pathRelative;
        }