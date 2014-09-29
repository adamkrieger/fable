module Fable.Tests.TestHelpers

open System

let platformRoot (platformID:PlatformID) =
    match platformID with
    | PlatformID.Unix
    | PlatformID.MacOSX -> @"/"
    | PlatformID.Win32NT
    | PlatformID.Win32S
    | PlatformID.Win32Windows
    | PlatformID.WinCE -> @"C:\"
    | _ -> ""

let sysRoot =
    platformRoot Environment.OSVersion.Platform