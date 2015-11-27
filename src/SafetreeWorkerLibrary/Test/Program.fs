// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open SafetreeWorkerLibrary
open SafetreeWorkerLibrary.Student
open SafetreeWorkerLibrary.Web
[<EntryPoint>]
let main argv = 
    

    printfn "SafetreeWorker test:"
    let name = "dyb"
    let password = "123456"

    //开始工作

    let user = User(name, password)
    login user
    //let works = getUnfinishedWorks user
    //printfn "%A" works
    //works |> List.iter doWork

    //停止工作

    System.Console.ReadLine ()
    0 // return an integer exit code
