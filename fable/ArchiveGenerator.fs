module Fable.ArchiveGenerator

let interpretFableTags content postList =
    content

let generateArchive content postList =
    content |> interpretFableTags postList