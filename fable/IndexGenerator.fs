module Fable.IndexGenerator

open System
open System.IO
open FParsec

let resolveResult result =
        match result with
        | Success(result,_,_) -> result
        | Failure(errorMsg,_,_) -> raise(Exception(errorMsg))

