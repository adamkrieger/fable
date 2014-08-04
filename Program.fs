// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    use fs = new System.IO.StreamWriter(".\index.html")

    fs.Write("<html></html>")

    let result = System.Console.ReadLine();

    0 // return an integer exit code
