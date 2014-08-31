namespace fable.tests

open fable
open PageBuilder
open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``When the input file and output directory are known`` () =

    let destinationPath = transformSourcePathToDestination "C:\\test\\blog\\input.html" "C:\\output\\whatever"

    [<Test>] member x.
     ``The output path is the combined value`` () =
        destinationPath |> should equal "C:\\output\\whatever\\input.html"