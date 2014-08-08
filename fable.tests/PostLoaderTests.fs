namespace fable.tests

open NUnit.Framework
open FsUnit
open Fable.PostLoader
open Foq
        
[<TestFixture>]
type ``Given an empty directory`` ()=
    let getFilesInDirectory = []
    let postNames = getPostNames getFilesInDirectory

    [<Test>] member x.
     ``when we request post names, it should return 0 posts`` ()=
        postNames.Length |> should equal 0

[<TestFixture>]
type ``Given two good filenames`` ()=
    let getFilesInDirectory = ["2014-07-04 The first post.md";
        "2014-08-03 The second post.md"]
    let postNames = getPostNames getFilesInDirectory

    [<Test>] member x.
     ``when we request post names, we should get 2 posts`` ()=
        postNames.Length |> should equal 2

[<TestFixture>]
type ``Given a bad file extension`` ()=
    let getFilesInDirectory = ["2014-07-03 The first post.md~"]
    let postNames = getPostNames getFilesInDirectory

    [<Test>] member x.
     ``when we request post names, we should get 0 posts`` ()=
        postNames.Length |> should equal 0
