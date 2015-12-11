// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
//open SafetreeWorkerLibrary
//open SafetreeWorkerLibrary.Student
//open SafetreeWorkerLibrary.Web
open FSharp.Data
open FSharp.Text.RegexProvider
open System.Net

//type IndexPage = HtmlProvider<"..\..\data\CourseList.htm">
type UriRegex = Regex< @"/TestCenter/TestIndex\.aspx\?li=(?<Li>.+)&gid=(?<Gid>.+)">

let siteUri s = "http://chengdu.safetree.com.cn/" + s

[<EntryPoint>]
let main argv = 
    

    printfn "SafetreeWorker test:"
    let name = "dyb"
    let password = "123456"

    // 开始工作

    let cc = CookieContainer()

    // 登录
    Http.RequestString ( siteUri "LoginHandler.ashx", 
        query = ["userName", name; "password", password; "type", "login"; "loginType", "1"], 
        cookieContainer = cc) |> ignore

    // 列表
    let listPage = 
        Http.RequestString ( siteUri "SafeSchool/StuSafeCourse.aspx", cookieContainer = cc )
        |> HtmlDocument.Parse

    //let li = IndexPage.Load("http://chengdu.safetree.com.cn/SafeSchool/StuSafeCourse.aspx")
    //let a = li.Lists
    
    // 解析

    let makeUri s =
        let m = UriRegex().Match(s)
        sprintf "http://cdjt.safetree.com.cn/EscapeSkill/SeeVideo.aspx?li=%s&gid=%s" m.Li.Value m.Gid.Value
        

    let links =
        listPage.Descendants "a"
        |> Seq.choose (
            function
            | x when x.InnerText() = "完成本课作业" ->
                x.TryGetAttribute("href")
                |> Option.map (fun a -> a.Value())
            | _ -> None

        )
    let linkstr = 
        links
        |> Seq.map makeUri

    // 停止工作

    System.Console.ReadLine () |> ignore
    0 // return an integer exit code
