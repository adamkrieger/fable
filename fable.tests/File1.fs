namespace fable.tests

open fable
open NUnit.Framework
open FsUnit
open Foq
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