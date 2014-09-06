namespace Fable

open FParsec
open ParserHelpers

type CommonMarkParser () =

    let preprocessor input =
        let tabToSpacesParser = 
            [
                attempt (tab |>> (fun x -> "    "))
                anyChar |>> (fun x -> x.ToString())
            ]
            |> choice 

        let iterator = manyTill tabToSpacesParser eof

        (run iterator input) |> resolveResult

    member this.compile input = 
        let res = preprocessor input |> List.reduce (fun a b -> a + b)

        res

