module App

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.JS
open FSharp.Core
open Browser.Dom
open Fetch

let mainDiv = document.createElement("div")
mainDiv.setAttribute("id", "contentDiv")
mainDiv.setAttribute("class","contentCls")

let innerDivForBtn = document.createElement("div")
innerDivForBtn.setAttribute("id","innerDivForBtn")
innerDivForBtn.setAttribute("class","innerDivCls")

let labelTxt = document.createTextNode("Card View")
let labelViewChange = document.createElement("label")
labelViewChange.appendChild(labelTxt)

let chkInput = document.createElement("input")
chkInput.setAttribute("id","viewChk")
chkInput.setAttribute("type","checkbox")
chkInput.setAttribute("data-toggle","toggle")
chkInput.setAttribute("data-size","sm")

labelViewChange.appendChild(chkInput)

//DataTable
let table1 = document.createElement("table")
table1.setAttribute("id", "quotes")
table1.setAttribute("border", "1")
table1.setAttribute("class", "table")

//Inner div for cards
let cardDeckDiv = document.createElement("div")
let innerDiv2 = document.createElement("div")
innerDiv2.setAttribute("class","forContentCls")
innerDiv2.appendChild(cardDeckDiv)
innerDiv2.setAttribute("style","display:none")

innerDivForBtn.appendChild(labelViewChange)
mainDiv.appendChild(innerDivForBtn)|> ignore
mainDiv.appendChild(table1)
mainDiv.appendChild(innerDiv2)
document.body.appendChild(mainDiv) 

let table = document.getElementById("quotes");
let head = document.createElement("thead")
let trow = document.createElement("tr")
let h1 = document.createElement("th")
let h2 = document.createElement("th")
let h3 = document.createElement("th")
let h4 = document.createElement("th")
let h5 = document.createElement("th")
let h6 = document.createElement("th")

let date = document.createTextNode("Date")
let open1 = document.createTextNode("Open")
let high = document.createTextNode("High")
let low = document.createTextNode("Low")
let close = document.createTextNode("Close")
let volume = document.createTextNode("Volume")

h1.appendChild(date)
h2.appendChild(open1)
h3.appendChild(high)
h4.appendChild(low)
h5.appendChild(close)
h6.appendChild(volume)

trow.appendChild(h1)
trow.appendChild(h2)
trow.appendChild(h3)
trow.appendChild(h4)
trow.appendChild(h5)
trow.appendChild(h6)
head.appendChild(trow)
table.appendChild(head) |> ignore

type Quote =
    { open1: decimal;
    high: decimal;
    low: decimal;
    close: decimal;
    date: string;
    volume: decimal;}

let createTableRow (quotes: Quote[]) = 
    let body = document.createElement("tbody")
    quotes
    |> Array.iter(fun quote -> 
        let trow = document.createElement("tr")

        let d1 = document.createElement("td")
        let d2 = document.createElement("td")
        let d3 = document.createElement("td")
        let d4 = document.createElement("td")
        let d5 = document.createElement("td")
        let d6 = document.createElement("td")
        
        let date = document.createTextNode(quote.date.ToString())
        let open1 = document.createTextNode(quote.open1.ToString())
        let high = document.createTextNode(quote.high.ToString())
        let low = document.createTextNode(quote.low.ToString())
        let close = document.createTextNode(quote.close.ToString())
        let volume = document.createTextNode(quote.volume.ToString())
        
        d1.appendChild(date)
        d2.appendChild(open1)
        d3.appendChild(high)
        d4.appendChild(low)
        d5.appendChild(close)
        d6.appendChild(volume)
        
        trow.appendChild(d1)
        trow.appendChild(d2)
        trow.appendChild(d3)
        trow.appendChild(d4)
        trow.appendChild(d5)
        trow.appendChild(d6)

        body.appendChild(trow)
        |> ignore
    )
    table.appendChild(body)

    
let createCardRow (quotes: Quote[]) = 
    innerDiv2.setAttribute("style","display:block")
    let quotesArr = quotes |> Array.splitInto 25
    quotesArr
    |> Array.iter(fun quoteArr -> 
        let cRow = document.createElement("div")
        cRow.setAttribute("class","row")
        quoteArr
        |> Array.iter(fun quote ->
        
        let colDiv = document.createElement("div")
        colDiv.setAttribute("class","col-md-3")

        let cDiv = document.createElement("div")
        cDiv.setAttribute("class","card cardCls")
        let cDiv2 = document.createElement("div")
        cDiv2.setAttribute("class","card-body text-center")

        let d1 = document.createElement("p")
        d1.setAttribute("class","card-text")
        let d2 = document.createElement("p")
        d2.setAttribute("class","card-text")
        let d3 = document.createElement("p")
        d3.setAttribute("class","card-text")
        let d4 = document.createElement("p")
        d4.setAttribute("class","card-text")
        let d5 = document.createElement("p")
        d5.setAttribute("class","card-text")
        let d6 = document.createElement("p")
        d6.setAttribute("class","card-text")
        
        let date = document.createTextNode("Date : " + quote.date.ToString())
        let open1 = document.createTextNode("Open : " + quote.open1.ToString())
        let high = document.createTextNode("High : " + quote.high.ToString())
        let low = document.createTextNode("Low : " + quote.low.ToString())
        let close = document.createTextNode("Close : " + quote.close.ToString())
        let volume = document.createTextNode("Volume : " + quote.volume.ToString())
        
        d1.appendChild(date)
        d2.appendChild(open1)
        d3.appendChild(high)
        d4.appendChild(low)
        d5.appendChild(close)
        d6.appendChild(volume)
        
        cDiv2.appendChild(d1)
        cDiv2.appendChild(d2)
        cDiv2.appendChild(d3)
        cDiv2.appendChild(d4)
        cDiv2.appendChild(d5)
        cDiv2.appendChild(d6)
        cDiv.appendChild(cDiv2)
        colDiv.appendChild(cDiv)
        cRow.appendChild(colDiv)
        cardDeckDiv.appendChild(cRow)
        cardDeckDiv.appendChild(document.createElement("br"))
        |> ignore
    ))

let getWebPageLength url =
    promise {
        let! response =
            fetch url [ ]
        let! item = response.json<Quote[]>()
        item
        |> createTableRow
        |> ignore
        JS.console.log(item)
        return item
    }

let getWebPageLength2 url =
    promise {
        let! response =
            fetch url [ ]
        let! item = response.json<Quote[]>()
        item
        |> createCardRow
        |> ignore
        JS.console.log(item)
        return item
    }  

getWebPageLength "http://localhost:8080/yahoo"
getWebPageLength2 "http://localhost:8080/yahoo"





