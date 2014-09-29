namespace Fable.Tests.CommonMarkTests

open NUnit.Framework
open FsUnit
open Fable

[<TestFixture>]
type ``When the content contains tabs`` () =

    let parser = new CommonMarkParser()
    let output = parser.compile "This is some \tcontent\nAnd\tsome more."

    [<Test>] member x.
     ``The tabs should be replaced with four spaces`` () =
        output |> should equal "This is some     content\nAnd    some more."

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

    [<Test>] member x.
     ``One hash followed by words results in an h1 tag`` () =
        "# the header"
            |> parser.compile
            |> should equal "<h1>the header</h1>"

    [<Test>] member x.
     ``An SQL code block results in an attributed code block`` () =
        @"```sql and this should get ignored
SELECT
    *
FROM
    tbl_people
```"
            |> parser.compile
            |> should equal (@"<pre><code class=""language-sql"">SELECT
    *
FROM
    tbl_people
</code></pre>".Replace("\r\n","\n"))

    [<Test>] member x.
     ``A code block results in a code block`` () =
        "```\nstuff\n```"
            |> parser.compile
            |> should equal ("<pre><code>stuff\n</code></pre>".Replace("\r\n","\n"))