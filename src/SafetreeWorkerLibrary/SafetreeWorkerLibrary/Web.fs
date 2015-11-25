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

let webRequest (uri : string) requestMethod cookies =
    let request = System.Net.WebRequest.CreateHttp uri
    request.Method <- requestMethod.ToString ()
    request.Proxy <- null;
    request.UserAgent <- userAgent
    request.ContentType <- "application/x-www-form-urlencoded"
    request.CookieContainer <- cookies
    request

let httpGetResponseStream uri cookies =
    let request = webRequest uri "GET" cookies
    use response = request.GetResponse () :?> HttpWebResponse
    response.GetResponseStream ()

let httpGetHtmlDocument uri cookies = 
    use responseStream = httpGetResponseStream uri cookies
    
    let doc = HtmlDocument()
    doc.Load(responseStream)
    doc

let httpPostResponseStream uri parameters cookies =
    let request = webRequest uri "POST" cookies
    request.ServicePoint.Expect100Continue <- false

    let postBytes =
        parameters
        |> Seq.map (fun (a, b : string) -> a + "=" + System.Web.HttpUtility.UrlEncode b)
        |> String.concat "&"
        |> System.Text.Encoding.UTF8.GetBytes

    request.ContentLength <- int64 postBytes.Length

    use postStream = request.GetRequestStream ()
    postStream.Write (postBytes, 0, postBytes.Length)

    use response = request.GetResponse () :?> HttpWebResponse
    cookies.Add response.Cookies
    response.GetResponseStream ()

let httpPostHtmlDocument uri parameters cookies =
    use responseStream = httpPostResponseStream uri parameters cookies

    let doc = HtmlDocument()
    doc.Load(responseStream)
    doc

let login (User(name, password)) cookie =
    let uri = "http://chengdu.safetree.com.cn/"
    let loginStr = "/LoginHandler.ashx?userName=" + name + "&password=" + password + "&type=login&loginType=1"
    let html = httpGetHtmlDocument uri cookie
    //let requst = System.Net.WebRequest.CreateHttp uri
    ()