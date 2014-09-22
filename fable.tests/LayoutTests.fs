namespace Fable.Tests.LayoutTests

open NUnit.Framework
open FsUnit
open Fable

[<TestFixture>]
type ``When getting the default layout file path`` () =

    let rootDir = "C:\\temp\\site"
    let themeName = "default"

    [<Test>] member x.
     ``The content tag should be replaced with content`` () =
        (PageBuilder.getLayoutFilePath rootDir themeName) |> should equal "C:\\temp\\site\\themes\\default\\layout.html"