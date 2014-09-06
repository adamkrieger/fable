namespace fable.tests

open NUnit.Framework
open FsUnit
open Fable.PostLoader
open Fable
open System
        
[<TestFixture>]
type ``When the posts directory file list is filtered`` ()=
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

[<TestFixture>]
type ``When the post content is pulled from the file`` ()=
    let fileName = "2014-08-13 Cat Post.md"
    let fileContents = (fun x->  "This is a post about cats.")

    let post = assemblePost fileName fileContents

    [<Test>] member x.
     ``The date should be pulled from the file name`` ()=
        post.Date |> should equal (DateTime.Parse("2014-08-13"))
