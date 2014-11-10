namespace Fable.Tests.CommonMarkTests

open NUnit.Framework
open FsUnit
open Fable

[<TestFixture>]
type ``When the content contains tabs`` () =

    let parser = new CommonMark.CommonMarkParser()
    let output = parser.compile "This is some \tcontent\nAnd\tsome more."

    [<Test>] member x.
     ``The tabs should be replaced with four spaces`` () =
        output |> should equal "<p>This is some     content\nAnd    some more.</p>"

[<TestFixture>]
type ``When parsing a leaf block`` () =

    let parser = new CommonMark.CommonMarkParser()

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

[<TestFixture>]
type ``When parsing a paragraph`` () =

    let parser = new CommonMark.CommonMarkParser()

    [<Test>] member x.
     ``Blank lines do not turn into paragraphs`` () =
        @"

"
            |> parser.compile
            |> should equal @"
"

    [<Test>] member x.
     ``Content lines turn into paragraphs`` () =
        @"This is a paragraph"
            |> parser.compile
            |> should equal @"<p>This is a paragraph</p>"

    [<Test>] member x.
     ``Multiline with space returns two paragraphs`` () =
        @"p1

p2"
            |> parser.compile
            |> should equal @"<p>p1</p>

<p>p2</p>"
        
type ``When parsing a list`` () =

    let parser = new CommonMark.CommonMarkParser()

    [<Test>] member x.
     ``One unordered list item compiles correctly`` () =
        @"* First Item"
            |> parser.compile
            |> should equal @"<ul>
<li>First Item</li>
</ul>"