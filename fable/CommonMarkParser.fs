namespace Fable

open FParsec
open ParserHelpers

type CommonMarkParser () =

    let iterator parser = manyTill parser eof

    let zeroToThreeSpaces = 
        [
            pstring "   "
            pstring "  "
            pstring " "
            pstring ""
        ]
        |> choice

    let horizontalRuleSequence =
        [
            pstring "---"
            pstring "___"
            pstring "***"
        ]
        |> choice

    let preprocessor input =
        let tabToSpacesParser = 
            [
                attempt (tab |>> (fun x -> "    "))
                anyChar |>> (fun x -> x.ToString())
            ]
            |> choice
            |> iterator

        (run tabToSpacesParser input) 
            |> resolveResult

    let horizontalRule input =
        let horizontalRuleParser =
            [
                attempt zeroToThreeSpaces >>. horizontalRuleSequence .>> restOfLine true
                    |>> (fun x -> "<hr />")
                restOfLine true
            ]
            |> choice
            |> iterator

        (run horizontalRuleParser input)
            |> resolveResult

    member this.compile input = 
        let res = preprocessor input |> List.reduce (fun a b -> a + b)

        let res = horizontalRule res |> List.reduce (fun a b -> a + b)

        res

