module ParserHelpers

open FParsec
open System

let resolveResult result =
    match result with
    | Success(result,_,_) -> result
    | Failure(errorMsg,_,_) -> raise(Exception(errorMsg))
