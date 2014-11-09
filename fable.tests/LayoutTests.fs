namespace Fable.Tests.LayoutTests

open NUnit.Framework
open FsUnit
open Fable
open System.IO
open Fable.Tests.TestHelpers

[<TestFixture>]
type ``When getting the default page layout file path`` () =

    let rootDir = Path.Combine(sysRoot, @"temp", @"site")
    let themeName = "default"

    [<Test>] member x.
     ``The content tag should be replaced with content`` () =
        (LayoutLoader.getLayoutFilePath rootDir themeName @"layout.html") 
        |> should equal 
            (Path.Combine(sysRoot, @"temp", @"site", @"themes", @"default", @"layout.html"))