module SafetreeWorkerLibrary.Web

open System.Net
open HtmlAgilityPack

let userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0"

let httpPostHtmlDocument (uri : string) parameters cookies =
    let request = System.Net.WebRequest.CreateHttp uri
    request.Method <- "POST"
    request.Proxy <- null;
    request.ServicePoint.Expect100Continue <- false
    request.UserAgent <- userAgent
    request.ContentType <- "application/x-www-form-urlencoded"
    request.CookieContainer <- cookies

    let postBytes =
        parameters
        |> Seq.map (fun (a, b : string) -> a + "=" + System.Web.HttpUtility.UrlEncode b)
        |> String.concat "&"
        |> System.Text.Encoding.UTF8.GetBytes

    request.ContentLength <- int64 postBytes.Length

    use postStream = request.GetRequestStream ()
    postStream.Write (postBytes, 0, postBytes.Length)

    use response = request.GetResponse () :?> HttpWebResponse
    use responseStream = response.GetResponseStream ()
    let htmlStream = responseStream
    cookies.Add response.Cookies

    let doc = HtmlDocument()
    doc.Load(htmlStream)
    doc, cookies


let login (User(name, password)) cookie =
    let uri = "http://chengdu.safetree.com.cn/"
    //let requst = System.Net.WebRequest.CreateHttp uri
    ()