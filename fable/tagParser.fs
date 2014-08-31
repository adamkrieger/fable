namespace fable

open FParsec.CharParsers
open FParsec
open System

type TagParser (layout:string) =
    let _content = ""

    let contentKeywordParser =
        pstring "content"
        
    let fableKeywordParser content = 
        [
            contentKeywordParser |>> (fun tag -> content)
        ]
        |> choice

    let startTagParser =
        pstring "[#"

    let endTagParser = 
        pstring "#]"

    let fableTagParser content = 
        startTagParser >>. spaces >>. (fableKeywordParser content) .>> spaces .>> endTagParser

    let upToTagStartParser =
        charsTillString "[#" false 1000

    let parseRouting content =
        [
            attempt (fableTagParser content)
            attempt startTagParser
            attempt upToTagStartParser
            restOfLine true
        ]
        |> choice

    let resolveResult result =
        match result with
        | Success(result,_,_) -> result
        | Failure(errorMsg,_,_) -> raise(Exception(errorMsg))

    member this.compile content =
        let _content = content
        let fableParser = manyTill (parseRouting content) eof
        let result = run fableParser layout
        resolveResult result |> List.reduce (fun a b -> a + "\n" + b)