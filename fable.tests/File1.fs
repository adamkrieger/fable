namespace fable.tests

open Fable
open NUnit.Framework
open FsUnit
open Foq
open Fable.ArchiveGenerator
open System

[<TestFixture>]
type ``When generating the archive`` ()=
    let terribleArchiveContent = "Not a very good archive."

    let staticArchiveContent = "[# foreach post #]output[# end foreach #]"

    let postList = [
        { 
            date = DateTime.Parse("2014-07-01");
            title = "First Post";
            content = "The first post."
        };
        { 
            date = DateTime.Parse("2014-08-01");  
            title = "Second Post";
            content = "The second post."
        };
        { 
            date = DateTime.Parse("2014-09-01");
            title = "Third Post";
            content = "The third post."
        }
    ]

    [<Test>] member x.
     ``The archive output will contain the input`` ()=
        (generateArchive terribleArchiveContent postList) |> should contain "Not a very good archive."

    [<Test>] member x.
     ``Output will filter out tags`` ()=
        (generateArchive staticArchiveContent) |> should not' (contain "{% foreach post %}")