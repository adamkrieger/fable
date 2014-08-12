module Fable.IndexGenerator

let postLink (postName) =
    "<a href=\"\">" + postName + "</a>"

let postLinkList (postList : string list) =
    postList |> List.map(fun postName -> postLink postName)

let postLinkListSerialized (postLinkList) =
    postLinkList |> List.reduce(fun a b -> a + b)

let generateIndex (postList) =
    let postLinkList = postLinkList postList
    let postLinkListSerialized = postLinkListSerialized postLinkList
    "<!DOCTYPE html><html><head></head><body>" + postLinkListSerialized + "</body></html>"

