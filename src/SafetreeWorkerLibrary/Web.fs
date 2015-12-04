module SafetreeWorkerLibrary.Web

open System.Net
open HtmlAgilityPack

type HttpRequest =
    | HttpGet
    | HttpPost of (string * string) seq

    override this.ToString () =
        match this with
        | HttpGet -> "GET"
        | HttpPost _ -> "POST"

let userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0"
let siteUri = "http://chengdu.safetree.com.cn"


let webRequest (uri : string) requestMethod cookies =
    let request = System.Net.WebRequest.CreateHttp uri
    request.Method <- requestMethod.ToString ()
    request.Proxy <- null;
    request.UserAgent <- userAgent
    request.ContentType <- "application/x-www-form-urlencoded"
    request.CookieContainer <- cookies

    match requestMethod with
    | HttpPost parameters ->
        let postBytes =
            parameters
            |> Seq.map (fun (a, b : string) -> a + "=" + System.Web.HttpUtility.UrlEncode b)
            |> String.concat "&"
            |> System.Text.Encoding.UTF8.GetBytes

        request.ContentLength <- int64 postBytes.Length
    | _ -> ()

    request

let responseStream (request : HttpWebRequest) =
    use response = request.GetResponse () :?> HttpWebResponse
    let ret = response.GetResponseStream ()
    request.CookieContainer.Add(response.Cookies)
    ret

let httpHtmlDocument requestMethod uri cookies =
    use responseStream =
        webRequest uri requestMethod cookies
        |> responseStream

    
    let doc = HtmlDocument()
    doc.Load(responseStream)
    doc

let login ((User(name, password)) as user) =
    let loginStr = "/LoginHandler.ashx?userName=" + name + "&password=" + password + "&type=login&loginType=1"
    let loginUri = siteUri + loginStr
    use responseStream =
        webRequest loginUri HttpGet user.Cookie
        |> responseStream
    ()

let downloadIndexPage user =
    let indexPageUri = siteUri + "/SafeSchool/StuSafeCourse.aspx"
    login user
    let page = httpHtmlDocument HttpGet indexPageUri user.Cookie
    let listNodes = page.DocumentNode.SelectNodes "//div//a[@target]"
    let uris = 
        listNodes 
        |> Seq.map (fun a -> a.GetAttributeValue("href", null))
        |> Seq.filter ((<>) null)
    uris
