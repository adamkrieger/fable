namespace  fable.tests.TagParserTests

open NUnit.Framework
open FsUnit
open Fable

[<TestFixture>]
type ``When parsing page content`` () =

    let parser = new TagParser "<html>\n[# content #]\n\n</html>"

    [<Test>] member x.
     ``The content tag should be replaced with content`` () =
        parser.compile "foo!\nbar!!" |> should equal "<html>\n\nfoo!\nbar!!\n\n\n</html>"