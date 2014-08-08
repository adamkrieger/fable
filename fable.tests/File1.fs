namespace fable.tests

open NUnit.Framework
open FsUnit
open Foq
open Fable.IndexGenerator

[<TestFixture>]
type ``When generating the index`` ()=
    let postList = ["2014-07-01 First Post";"2014-08-01 Second Post"]

    let indexHtml = generateIndex postList

    [<Test>] member x.
     ``A link to every post is included`` ()=
        indexHtml |> should contain "<a href"