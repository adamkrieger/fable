namespace Fable    

open FParsec.CharParsers
open FParsec
open ParserHelpers

type ContentParser () =

    let allCharsUntil tag =
        manyCharsTill anyChar (lookAhead (str tag))

    member this.getTitle content =

        let parser =
            allCharsUntil "[#"
            >>.
            between 
                    (str "[# title: ")
                    (str " #]")
                    (allCharsUntil " #]")
        
        let result = run parser content

        match result with
        | Success(output,_,_) -> output
        | Failure(msg,_,_) -> "Untitled"

    member this.sanitizeForOutput content =
        
        let parser =
            [
                attempt 
                    (between 
                        (str "[#") 
                        (str "#]") 
                        (manyCharsTill 
                            anyChar 
                            (lookAhead (str "#]"))
                        )
                    ) |>> (fun x -> "")
                attempt 
                    (manyCharsTill 
                        anyChar 
                        (lookAhead (str "[#"))
                    )
                manyChars anyChar
            ]
            |> choice

        let iterator = manyTill parser eof

        let result = run (parseManyTillEof parser) content

        match result with
        | Success(output,_,_) -> List.reduce (fun a b -> a + b) output
        | Failure(msg,_,_) -> "Parser failed: " + msg