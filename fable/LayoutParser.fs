namespace Fable

open FParsec.CharParsers
open FParsec
open System
open ParserHelpers

type LayoutParser (layout:string) =
    let _content = ""

    //Fable tags in EBNF
    // fableTagKeyword = "content";
    // nbws = { " " | "\t" }
    // openSquare = "["
    // 
    // fableTag = "[#" + nbws + fableTagKeyword + nbws + "#]";
    // nonFableTag = (nonOpenSquare + { anyChar })
    //               | (openSquare + nonHash + { anyChar })
    // line = { fableTag | nonFableTag }

    let contentKeywordParser =
        str "content"
        
    let fableKeywordParser content = 
        [
            contentKeywordParser |>> (fun tag -> content)
        ]
        |> choice

    let startTagParser =
        str "[#"

    let endTagParser = 
        str "#]"

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

    member this.compile content =
        let _content = content
        let fableParser = manyTill (parseRouting content) eof
        let result = run fableParser layout
        resolveResult result |> List.reduce (fun a b -> a + "\n" + b)
