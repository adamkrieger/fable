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

[<TestFixture>]
type ``When parsing a leaf block`` () =

    let parser = new CommonMarkParser()

//    A line consisting of 0-3 spaces of indentation, 
//    followed by a sequence of three or more matching -, _, or * characters, 
//    each followed optionally by any number of spaces, forms a horizontal rule.

    [<Test>] member x.
     ``Zero spaces and three underscores results in a horizontal rule`` () =
        "___"
            |> parser.compile
            |> should equal "<hr />"