namespace fable.tests.PageBuilderTests

open fable
open PageBuilder
open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``When the input file and output directory are known`` () =

    let destinationPath = transformSourcePathToDestination "C:\\output\\whatever" "C:\\test\\blog\\input.html"

    [<Test>] member x.
     ``The output path is the combined value`` () =
        destinationPath |> should equal "C:\\output\\whatever\\input.html"