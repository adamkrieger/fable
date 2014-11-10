namespace Fable.CommonMark

open FParsec
open ParserHelpers

type Tag = string
type Input = string

type CommonMarkParser () =

    let space = pstring " "
    let nothingAtAll = pstring ""
    let eof_toStr = (eof |>> (fun x -> ""))
    
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



    let wrapInTag (tag:Tag) (input:Input) =
        "<" + tag + ">" + input + "</" + tag + ">"

    let horizontalRule =
        let parser = zeroToThreeSpaces 
                    >>.
                        ([
                            pstring "---"
                            pstring "___"
                            pstring "***"
                        ]
                        |> choice)
                    .>>
                        restOfLine true
        
        parser
        |>> (fun input -> @"<hr />")

    let attemptHeader hN =
        attempt hN >>. space >>. restOfLine true

    let atxHeader =

        let h1 = pstring "#"
        let h2 = pstring "##"
        let h3 = pstring "###"
        let h4 = pstring "####"
        let h5 = pstring "#####"
        let h6 = pstring "######"

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
        ]
        |> choice

    let wrapInCodeTag lang code =
        match lang with
        | "" -> wrapInTag "code" code
        |_ -> "<code class=\"language-" + lang + "\">" + code + "</code>"

    let oneWord =
        manyCharsTill anyChar
            (lookAhead
                ([
                    pchar ' '
                    newline
                ]
                |> choice)
            )

    let codeBlockFence = pstring "```"
    
    let codeBlock = 

        let parser = codeBlockFence
                        >>.
                            ([
                                oneWord .>> restOfLine true
                                nothingAtAll
                            ]
                            |> choice)
                        .>>.
                            manyCharsTill anyChar (lookAhead codeBlockFence)
                        .>>
                            codeBlockFence

        parser                                
        |>> (fun output -> wrapInCodeTag (fst output) (snd output))
        |>> wrapInTag "pre"

    let blankLine = 
        manyCharsTill (anyOf " \t") newline

    let startOfUnorderedListItem = 
        zeroToThreeSpaces >>. pstring "*"

    let endOfParagraph =
        [
            attempt blankLine 
            attempt startOfUnorderedListItem
            attempt eof_toStr
        ]
        |> choice

    let unorderedListItem =
        startOfUnorderedListItem >>. space >>. restOfLine true
        |>> wrapInTag "li"

    let endOfUnorderedList =
        [
            attempt blankLine
            attempt eof_toStr
        ]
        |> choice

    let unorderedList = 
        many1Till unorderedListItem endOfUnorderedList
            |>> List.reduce (fun a b -> a + "\n" + b)
            |>> (fun input -> "\n" + input + "\n")
            |>> wrapInTag "ul"

    let paragraph =
        many1Till (restOfLine true) (lookAhead endOfParagraph)
            |>> List.reduce (fun a b -> a + "\n" + b)
            |>> wrapInTag "p"

    let preprocessor input =
        [
            attempt (tab |>> (fun x -> "    "))
            anyChar |>> (fun x -> x.ToString())
        ]
        |> choice
        |> iterateParserUntilEndAndResolve <| input

    let mainRouting input = 
        [
            attempt blankLine
            attempt horizontalRule
            attempt atxHeader
            attempt codeBlock
            attempt unorderedList
            attempt paragraph
            restOfLine true
        ]
        |> choice
        |> iterateParserUntilEndAndResolve <| input

    member this.compile input = 
        let rejoin = List.reduce (fun a b -> a + b)
        let rejoinLines = List.reduce (fun a b -> a + "\n" + b)

        let res = preprocessor input 
                    |> rejoin

        let res2 = mainRouting res

        let res3 = res2 |> rejoinLines

        res3

