namespace fable

open FParsec.CharParsers
open FParsec
open System

type TagParser(content:string) =

    let contentTag =
        pstring "content"
        
    let fableTagChoice = 
        [
            contentTag |>> (fun tag -> content)
        ]
        |> choice

    let startTag =
        pstring "[#"

    let endTag = 
        pstring "#]"

    let fableTag = 
        startTag >>. spaces >>. (fableTagChoice) .>> spaces .>> endTag

    let parseUntilTag =
        charsTillString "[#" false 100

    let parseRouting fableParser =
        attempt fableParser <|>
        attempt parseUntilTag <|>
        restOfLine true

    //overhead functions for test purposes
    let resolveResult result =
        match result with
        | Success(result,_,_) -> result
        | Failure(errorMsg,_,_) -> raise(Exception(errorMsg))

//    let dummyTest =
//        let x = resolveResult (run startTag "[#")
//        resolveResult (run (compile "") "")

//    let test parser input =
//        resolveResult (run parser input)

    member this.compile =
        let fableParser = fableTagChoice
        manyTill (parseRouting fableParser) eof
