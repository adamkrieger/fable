module Fable.PostLoader

let filterFileName (name : string) =
    let x = name.LastIndexOf('.')
    let fileExt = name.Substring(x)
    
    match fileExt with
    | ".md" -> true
    | _ -> false

let getPostNames (getFilesInDirectory) =
    getFilesInDirectory |> List.filter(fun x -> filterFileName x)