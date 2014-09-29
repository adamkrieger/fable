namespace Fable.Tests.PostLoaderTests

open NUnit.Framework
open FsUnit
open Fable.PostLoader
open Fable
open System
open System.IO
open Fable.Tests.TestHelpers
        
[<TestFixture>]
type ``When the posts directory file list is filtered`` ()=
    let getFilesInDirectory = [|
                                Path.Combine(@"C:\", "temp", "posts","test1.md")
                                Path.Combine(@"C:\","temp","posts","test2.html")
                                Path.Combine(@"C:\","temp","posts","test3.txt")
                              |]
    let postNames = filterPostFiles getFilesInDirectory

    [<Test>] member x.
     ``Non-md or html files should be filtered out`` ()=
        postNames |> should equal [|
                                    Path.Combine(@"C:\","temp","posts","test1.md")
                                    Path.Combine(@"C:\","temp","posts","test2.html")
                                  |]

[<TestFixture>]
type ``When the post content is pulled from the file`` ()=
    let fileName = Path.Combine(@"C:\","temp","posts","2014-08-13 Cat Post.md")
    let fileContents = (fun x->  " [# title: I love cats #]\n" +
                                 "This is a post about cats.")

    [<Test>] member x.
     ``The date should be pulled from the file name`` ()=
        (assemblePost fileName fileContents).Date |> should equal (DateTime.Parse("2014-08-13"))

    [<Test>] member x.
     ``The title is set via the title tag`` ()=
        (assemblePost fileName fileContents).Title |> should equal ("I love cats")

    [<Test>] member x.
     ``The content is set as expected`` ()=
        (assemblePost fileName fileContents).Content |> should equal (" \nThis is a post about cats.")

[<TestFixture>]
type ``When preparing to write the post file`` ()=

    let publishDate = DateTime.Parse("2014-09-15")
    let postTitle = "A post about cats: thing and such"

    let rootOutputDir = 
        Path.Combine(sysRoot, @"temp")

    [<Test>] member x.
     ``The output filename must be sanitized`` ()=
        getPostOutputFileName postTitle |> should equal "A_post_about_cats_thing_and_such.html"

    [<Test>] member x.
     ``The output directory is nested by date`` ()=
        getPostOutputDir rootOutputDir publishDate 
        |> should equal 
            (Path.Combine(sysRoot, @"temp", @"posts", @"2014", @"09", @"15"))