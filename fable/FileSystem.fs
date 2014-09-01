module FileSystem

open fable
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

let addSelectedThemeDir dir =
    Path.Combine(dir, ConfigurationManager.AppSettings.Item("theme"))

let getDefaultLayoutTemplate rootDir =
    let pathToLayout = Path.Combine(rootDir, "themes", "default", "layout.html")
    use stream = new StreamReader(pathToLayout)
    stream.ReadToEnd()

let copySourceToDestination pageRecord =
    let {
            sourcePath = source;
            destinationPath = destination
        } = pageRecord

    File.Copy(source, destination, true)