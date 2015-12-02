namespace SafetreeWorkerLibrary

open System.Net

type User (name : string, password : string)=
    member __.Name = name
    member __.Password = password
    member val Cookie = CookieContainer ()

type Work () = class end

[<AutoOpen>]
module TypesTool =
    let (|User|) (user : User) = user.Name, user.Password