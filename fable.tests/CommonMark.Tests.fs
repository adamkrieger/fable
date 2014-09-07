namespace Fable.Tests.CommonMarkTests

open NUnit.Framework
open FsUnit
open Fable

[<TestFixture>]
type ``When the content contains tabs`` () =

    let parser = new CommonMarkParser()
    let output = parser.compile "This is some \tcontent\nAnd\tsome more.\n"

    [<Test>] member x.
     ``The tabs should be replaced with four spaces`` () =
        output |> should equal "This is some     content\nAnd    some more.\n"