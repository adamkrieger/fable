namespace Fable

open FParsec
open ParserHelpers

type CommonMarkParser () =

    let iterator parser = manyTill parser eof

    let space = pstring " "

    let iterateParserUntilEndAndResolve parser input =
        parser
        |> iterator
            |> run <| input
                |> resolveResult

    let zeroToThreeSpaces = 
        [
            space >>. space >>. space
            space >>. space
            space
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
        [
            attempt (tab |>> (fun x -> "    "))
            anyChar |>> (fun x -> x.ToString())
        ]
        |> choice
            |> iterateParserUntilEndAndResolve <| input

    let horizontalRule input =
        [
            attempt zeroToThreeSpaces >>. horizontalRuleSequence .>> restOfLine true
                |>> (fun x -> "<hr />")
            restOfLine true
        ]
        |> choice
            |> iterateParserUntilEndAndResolve <| input

    let atxHeader input =
        let h1 = pstring "#"
        let h2 = pstring "##"

        [
            attempt h1 >>. space >>. restOfLine true
                |>> (fun x -> "<h1>" + x + "</h1>")
            restOfLine true
        ]
        |> choice
            |> iterateParserUntilEndAndResolve <| input

    member this.compile input = 
        let rejoin = List.reduce (fun a b -> a + b)
        let rejoinLines = List.reduce (fun a b -> a + "\n" + b)

        let res = preprocessor input |> rejoin

        let res = horizontalRule res |> rejoinLines

        let res = atxHeader res |> rejoinLines

        res

