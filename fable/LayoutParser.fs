namespace Fable

open FParsec.CharParsers
open FParsec
open System
open ParserHelpers

type LayoutParser (layout:string) =

    //Fable tags in EBNF
    // fableTagKeyword = "content";
    // nbws = { " " | "\t" }
    // openSquare = "["
    // 
    // fableTag = "[#" + nbws + fableTagKeyword + nbws + "#]";
    // nonFableTag = (nonOpenSquare + { anyChar })
    //               | (openSquare + nonHash + { anyChar })
    // line = { fableTag | nonFableTag }

    let contentKey =
        str "content"

    let titleKey =
        str "title"

    let dateKey =
        str "date"

    let startTagParser =
        str "[#"

    let endTagParser = 
        str "#]"
        
    let fablePostKeys title date content = 
        [
            titleKey |>> (fun _ -> title)
            dateKey |>> (fun _ -> date)
            contentKey |>> (fun _ -> content)
        ]
        |> choice

    let fablePageKeys content = 
        [
            contentKey |>> (fun _ -> content)
        ]
        |> choice

    let fableTagParser keywordParser = 
        startTagParser >>. spaces >>. keywordParser .>> spaces .>> endTagParser

    let upToTagStartParser =
        charsTillString "[#" false 1000

    let parseRouting keywordParser =
        [
            attempt (fableTagParser keywordParser)
            attempt startTagParser
            attempt upToTagStartParser
            restOfLine true
        ]
        |> choice

    member this.compile content =

        let keyParser = fablePageKeys content

        let fableParser = parseManyTillEof 
                            (parseRouting keyParser)

        let result = run fableParser layout
        resolveResult result |> List.reduce (fun a b -> a + "\n" + b)

    member this.compilePost title (date:DateTime) content =

        let keyParser = fablePostKeys 
                            title
                            (date.ToShortDateString())
                            (content.ToString())

        let fableParser = parseManyTillEof 
                            (parseRouting keyParser) 

        let result = run fableParser layout
        resolveResult result |> List.reduce (fun a b -> a + "\n" + b)