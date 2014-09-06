module StyleBuilder

open Fable
open System.IO
open FileSystem

let buildStyle rootDir outputDir =

    let sourceCssDir = rootDir
                       |> addThemesDir
                       |> addSelectedThemeDir
                       |> addStyleDir
                       |> createDirectoryIfItDoesNotExist

    let outputCssDir = outputDir
                       |> addStyleDir
                       |> createDirectoryIfItDoesNotExist

    let styleSheetFiles = 
        Directory.GetFiles(sourceCssDir, "*.css")
        |> Array.map (fun path -> { 
                                     Page.SourcePath = path;
                                     Page.DestinationPath =
                                        Path.Combine(outputCssDir, (Path.GetFileName path));
                                  }
                     )

    do styleSheetFiles 
       |> Array.map (fun page -> copySourceToDestination page)
       |> ignore


