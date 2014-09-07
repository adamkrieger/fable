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

    let create fileName destinationPath content =
        {
            DestinationPath = destinationPath;
            Content = content;
        }