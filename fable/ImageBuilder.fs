module ImageBuilder

open fable
open FileSystem
open System.IO

let buildImages sourceFolder destinationFolder =
    let imageFiles = 
        Directory.GetFiles(sourceFolder, "*.*")
        |> Array.filter (fun path -> path.EndsWith(".jpeg")
                                     || path.EndsWith(".jpg")
                                     || path.EndsWith(".gif")
                                     || path.EndsWith(".png")
                                     || path.EndsWith(".ico")
        )
        |> Array.map (fun path -> { 
                                     sourcePath=path;
                                     destinationPath=
                                        Path.Combine(destinationFolder, (Path.GetFileName path));
                                  }
                     )

    do imageFiles 
       |> Array.map (fun page -> copySourceToDestination page)
       |> ignore
