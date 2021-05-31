open System
open System.Threading
open Suave
open Suave.Filters
open Suave.Operators
open Newtonsoft.Json
open Newtonsoft.Json.Serialization


type Quote = {
    Open1: decimal;
    High: decimal;
    Low: decimal;
    Close: decimal;
    Date: string;
    //AdjClose: decimal;
    Volume: decimal;
}

let defQuote = {
    Open1 = 0M
    High = 0M
    Low = 0.0M
    Close = 0.0M
    Date = ""
    //AdjClose = 0.0M
    Volume = 0.0M
}

[<EntryPoint>]
let main argv = 

    let allRows (quotes: string) =
        let rows = quotes.Split("<tr")
        let rowValues =
            rows 
            |> Array.tail // removes extra HTML DOcument header
            |> Array.tail // removes table header
            |> Array.rev // below three lines removes last item
            |> Array.tail
            |> Array.rev
            |> Array.map(fun x -> 
                let quotes = 
                    x.Split("</span>")
                    |> Array.map(fun y -> 
                        y
                        |> String.split '>'
                        |> List.rev
                        |> List.head
                    )
                {defQuote with
                    Date = quotes.[0]
                    Open1 = Convert.ToDecimal(quotes.[1])
                    High = Convert.ToDecimal(quotes.[2])
                    Low = Convert.ToDecimal(quotes.[3])
                    Close = Convert.ToDecimal(quotes.[4])
                    //AdjClose = Convert.ToDecimal(quotes.[5])
                    Volume = Convert.ToDecimal(quotes.[6])
                }                    
            )
        rowValues

    let JSON (json: obj) =
        let jsonSerializerSettings = new JsonSerializerSettings()
        jsonSerializerSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()
        JsonConvert.SerializeObject(json,jsonSerializerSettings)
        
       

    let getAsync (url:string) = 
        async {
            let httpClient = new System.Net.Http.HttpClient()
            let! response = httpClient.GetAsync(url) |> Async.AwaitTask
            response.EnsureSuccessStatusCode () |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            allRows content |> ignore
            return allRows content
        }

    let setCORSHeaders =
        Suave.Writers.addHeader  "Access-Control-Allow-Origin" "*"
        >=> Suave.Writers.addHeader "Access-Control-Allow-Headers" "content-type"

    let allow_cors : WebPart =
        choose [
            OPTIONS >=>
                fun context ->
                    context |> (
                        setCORSHeaders
                        >=> Suave.Successful.OK "CORS approved" )
        ]

    let app =
        choose
            [   
                allow_cors
                GET >=> 
                    choose [ 
                        path "/hello" >=> Suave.Successful.OK "Hello GET"
                        path "/goodbye" >=> Suave.Successful.OK "Good bye GET"
                        path "/yahoo" 
                            >=> Suave.Successful.OK (getAsync "https://finance.yahoo.com/quote/%5EGSPC/history?p=EGSPC" |> Async.RunSynchronously |> JSON)
                            >=> Suave.Writers.addHeader "Access-Control-Allow-Origin"  "*"
                            >=> Writers.setMimeType "application/json"
                            
                    ]
                
            ]

    let cts = new CancellationTokenSource()
    let conf = { defaultConfig with cancellationToken = cts.Token }
    let listening, server = startWebServerAsync conf app
    
    Async.Start(server, cts.Token)
    printfn "Make requests now"
    Console.ReadKey true |> ignore
    
    cts.Cancel()

    0 // return an integer exit code