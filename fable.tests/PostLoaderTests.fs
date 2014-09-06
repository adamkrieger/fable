namespace fable.tests

open NUnit.Framework
open FsUnit
open Fable.PostLoader
        
[<TestFixture>]
type ``Given the posts directory file list`` ()=
    let getFilesInDirectory = [
                                "C:\\temp\\posts\\test1.md";
                                "C:\\temp\\posts\\test2.html";
                                "C:\\temp\\posts\\test3.txt"
                              ]
    let postNames = filterPostFiles getFilesInDirectory

    [<Test>] member x.
     ``Non-md or html files should be filtered out`` ()=
        postNames |> should equal [
                                    "C:\\temp\\posts\\test1.md";
                                    "C:\\temp\\posts\\test2.html"
                                  ]

