module FileSystem

open Fable
open System.IO
open System.Configuration

let createDirectoryIfItDoesNotExist (dir)=
    match Directory.Exists(dir) with
    | false -> Directory.CreateDirectory(dir) |> ignore
    | _ -> None |> ignore
    dir

let addOutputDirectorySuffix dir = Path.Combine(dir, "bin")

let addStyleDir dir = Path.Combine(dir, "css")

let addImageDir dir = Path.Combine(dir, "img")

let addThemesDir dir = Path.Combine(dir, "themes")

let addPostsDir dir = Path.Combine(dir, "posts")

let addSelectedThemeDir dir =
    Path.Combine(dir, ConfigurationManager.AppSettings.Item("theme"))

let getDefaultLayoutTemplate rootDir =
    let pathToLayout = Path.Combine(rootDir, "themes", "default", "layout.html")
    use stream = new StreamReader(pathToLayout)
    stream.ReadToEnd()

let copySourceToDestination pageRecord =
    let {
            Page.SourcePath = source;
            Page.DestinationPath = destination
        } = pageRecord

    File.Copy(source, destination, true)

let getExtension fileName = 
    Path.GetExtension fileName

let getFileName filePath = 
    Path.GetFileName filePath

let getAllFilesInDirectory path =
    Directory.GetFiles(path, "*.*")

let getHtmlFilesInDirectory path = 
    Directory.GetFiles(path, "*.html")

let getFileContents (filePath:string) =
    use reader = new StreamReader(filePath)
    reader.ReadToEnd()

let writeToFile (filePath:string) (contents:string) =
    use writer = new StreamWriter(filePath)
    writer.Write(contents)

let combinePathWithFileName path fileName =
    Path.Combine(path,fileName)