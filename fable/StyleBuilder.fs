module StyleBuilder

open fable
open System.IO
open FileSystem

let buildStyle sourceStyleSheetFolder destinationStyleSheetFolder =
    let styleSheetFiles = 
        Directory.GetFiles(sourceStyleSheetFolder, "*.css")
        |> Array.map (fun path -> { 
                                     sourcePath=path;
                                     destinationPath=
                                        Path.Combine(destinationStyleSheetFolder, (Path.GetFileName path));
                                  }
                     )

    do styleSheetFiles 
       |> Array.map (fun page -> copySourceToDestination page)
       |> ignore


