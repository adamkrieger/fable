namespace Fable.Tests.PostBuilderTests

open Fable
open NUnit.Framework
open FsUnit
open Fable.CommonMarkFile
open Fable.BlogPostPartial

[<TestFixture>]
type ``When loading CommonMark files`` () =

    let fileName = "somefile.md"
    let content = "some sample content"

    [<Test>] member x.
     ``The content and filename must match input`` () =
        (CommonMarkFile.create fileName content)
            |> should equal { FileName = fileName; Content = content }

[<TestFixture>]
type ``When building the post partials`` () =

    let postFile = { 
                        FileName = "2014-10-20 Sample Post";
                        Content = "Some sample post content";
                   }

    [<Test>] member x.
        ``The date must be pulled from the file name`` () =
         (BlogPostPartial.create postFile)
            |> should equal 
                    {
                        FileNameForWeb = "20141020_SamplePost.html";
                        Content = "Some sample post content";
                        PublishDate = (DateTime.Parse "2014-10-20");
                    }
           