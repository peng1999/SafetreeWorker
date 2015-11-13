// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Types.fs"
#load "Student.fs"
open SafetreeWorkerLibrary
open SafetreeWorkerLibrary.Student

//open System

// Define my library scripting code here

printfn "SafetreeWorker test:"
printf "input your username:"
let name = System.Console.ReadLine ()
printf "input your password(null as 123456):"
let password =
    let input = System.Console.ReadLine ()
    if System.String.IsNullOrWhiteSpace input then "123456" else input

//开始工作

let user = User(name, password)
let works = getUnfinishedWorks user
works |> List.iter doWork

//停止工作

System.Console.ReadLine ()