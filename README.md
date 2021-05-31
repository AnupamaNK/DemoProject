# Demo App

This is a Fable app to retrieve data from a website and display the same on UI in the form of table and UI cards with a switch to toggle between the two views.

## Requirements

* [dotnet SDK](https://www.microsoft.com/net/download/core) 3.0 or higher
* [node.js](https://nodejs.org) with [npm](https://www.npmjs.com/)
* An F# editor like Visual Studio, Visual Studio Code with [Ionide](http://ionide.io/) or [JetBrains Rider](https://www.jetbrains.com/rider/).
* Bootstrap 4.5.2
* JQuery 3.5.1

## Building and running the app

* Install JS dependencies: `npm install`
* Go into src: `dotnet build`
* Go to root folder: `npm start`

## Specifications
* The application uses fable F# to accomplish the task
* However, the URL was not accessible and threw CORS error
* This needed a proxy to help access the URL and fetch data, using Suave
* F# type providers, not being compatible with Fable, Fable.fetch has been used to extract data from website
* For the front end, Bootstrap data table and cards have been used
