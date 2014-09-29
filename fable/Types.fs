namespace Fable

open System

module Post =
    type T = {
                    Date : DateTime;
                    Title : string;
                    Content : string;
                }

    let create date title content = 
        {
            Date = date;
            Title = title;
            Content = content
        }

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

module PreLayoutPost = 
    type T = 
        {
            FileTitle : string;
            DestinationPath : string;
            PreLayoutContent : string;
        }

    let create fileTitle destinationPath content = 
        {
            FileTitle = fileTitle;
            DestinationPath = destinationPath;
            PreLayoutContent = content;
        }