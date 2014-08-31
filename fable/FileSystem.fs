module FileSystem

open System.IO

let createDirectoryIfItDoesNotExist (dir)=
    match Directory.Exists(dir) with
    | false -> Directory.CreateDirectory(dir) |> ignore
    | _ -> None |> ignore
    dir

let addOutputDirectorySuffix dir = Path.Combine(dir, "bin")

let getDefaultLayoutTemplate rootDir =
    let pathToLayout = Path.Combine(rootDir, "themes", "default", "layout.html")
    use stream = new StreamReader(pathToLayout)
    stream.ReadToEnd()