namespace Fable

open FParsec
open ParserHelpers

type CommonMarkParser () =

    let space = pstring " "
    let nothingAtAll = pstring ""

    let parseManyTillEof parse = 
        parse |> manyTill <| eof
    
    let iterateParserUntilEndAndResolve parser input =
        parser
        |> parseManyTillEof
            |> run <| input
                |> resolveResult

    let zeroToThreeSpaces = 
        [
            space >>. space >>. space
            space >>. space
            space
            nothingAtAll
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
        let horizontalRuleSequence =
                zeroToThreeSpaces 
            >>.
                ([
                    pstring "---"
                    pstring "___"
                    pstring "***"
                ]
                |> choice)
            .>>
                restOfLine true

        [
            attempt horizontalRuleSequence
                |>> (fun x -> "<hr />")
            restOfLine true
        ]
        |> choice
            |> iterateParserUntilEndAndResolve <| input

    let wrapInTag tag input =
        "<" + tag + ">" + input + "</" + tag + ">"

    let atxHeader input =
        let h1 = pstring "#"
        let h2 = pstring "##"
        let h3 = pstring "###"
        let h4 = pstring "####"
        let h5 = pstring "#####"
        let h6 = pstring "######"

        let attemptHeader hN =
            attempt hN >>. space >>. restOfLine true

        [
            attemptHeader h6
                |>> wrapInTag "h6"
            attemptHeader h5
                |>> wrapInTag "h5"
            attemptHeader h4
                |>> wrapInTag "h4"
            attemptHeader h3
                |>> wrapInTag "h3"
            attemptHeader h2
                |>> wrapInTag "h2"
            attemptHeader h1
                |>> wrapInTag "h1"
            restOfLine true
        ]
        |> choice
            |> iterateParserUntilEndAndResolve <| input

    let codeBlockLanguage =
        [
            pstring "sql"
            pstring "ruby"
        ]
        |> choice

    let wrapInCodeTag lang code =
        match lang with
        | "" -> wrapInTag "code" code
        |_ -> "<code class=\"language-" + lang + "\">" + code + "</code>"

    let fencedCodeBlock input =
        let fence = pstring "```"
        
        let codeBlock = 
                fence
            >>.
                ([
                    skipMany space >>. codeBlockLanguage .>> restOfLine true
                    nothingAtAll
                ]
                |> choice)
            .>>.
                manyCharsTill anyChar (lookAhead fence)
            .>>
                fence

        [
            attempt codeBlock
                |>> (fun output -> wrapInCodeTag (fst output) (snd output))
                |>> wrapInTag "pre"
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

        let res = fencedCodeBlock res |> rejoinLines

        res

