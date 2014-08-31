namespace fable

open System

type Post = {
    date : DateTime;
    title : string;
    content : string;
}

type Page = {
    sourcePath : string;
    destinationPath : string;
}