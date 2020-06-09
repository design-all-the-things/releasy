module Releasy.App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2.ContextInsensitive
open Giraffe
open System.Security.Cryptography
open System.Text
open Releasy.Development.Domain
open Releasy.Development.Model
open Releasy.Development.ACL.Github
open Releasy.FeatureManagement.Model
open Releasy.FeatureManagement.ACL.Trello.Service

// ---------------------------------
// Models
// ---------------------------------

type Message =
    {
        Text : string
    }

// ---------------------------------
// Helpers
// ---------------------------------

let computeGithubSignature (secret: string) (payload: string) : string =
    use hmacSh1 = new HMACSHA1(Encoding.ASCII.GetBytes(secret))
    let hash =
        hmacSh1.ComputeHash(Encoding.ASCII.GetBytes(payload))
                |> Array.map (fun (x : byte) -> System.String.Format("{0:X2}", x))
                |> String.concat String.Empty

    sprintf "sha1=%s" hash

// ---------------------------------
// Handlers
// ---------------------------------

let printAndCallTrello (event : MRLinkedToFeature) = task {
    printfn "MRLinkedToFeature %s" event.featureIdentifier
    let mergeRequest = { url = event.mergeRequest.url }
    let feature = { id = event.featureIdentifier }
    return! (mergeRequest, feature)
        |> linkMergeRequestToFeatureInTrello
        |> Async.StartAsTask
}

let handlePostGithub (secret: string) (next : HttpFunc) (ctx : HttpContext) = task {
    match ctx.GetRequestHeader "X-Hub-Signature" with
    | Error _ ->
        return! RequestErrors.BAD_REQUEST "Missing signature" next ctx
    | Ok signature ->
        let! payload = ctx.ReadBodyFromRequestAsync()

        let computedSignature = computeGithubSignature secret payload

        if computedSignature.Equals(signature, StringComparison.CurrentCultureIgnoreCase) |> not then
            return! RequestErrors.BAD_REQUEST "Invalid signature" next ctx
        else
            let! payload = ctx.BindJsonAsync<WebHookPayload>()
            let mrLinkedEvent = fromPayloadToMergeRequest payload |> linkMergeRequestToFeature
            match mrLinkedEvent with
            | MRLinkedToFeature m -> printAndCallTrello m |> ignore
            | MRNotLinkedToFeature _ -> printfn "HO NO (ノಥ,_｣ಥ)ノ彡┻━┻"

            return! Successful.OK "Ok" next ctx
}

// ---------------------------------
// Web app
// ---------------------------------

let webApp =
    choose [
        subRoute "/api"
            (choose [
            GET >=>
                choose [
                    route "/version" >=> json { Text = "V0.0.0" }
                ]
            POST >=>
                choose [
                    route "/github/webhook" >=> handlePostGithub "un truc en dur"
                ]
            ])
        setStatusCode 404 >=> text "Not Found" ]


// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let port =
        match Environment.GetEnvironmentVariable("PORT") with
        | null -> "8080"
        | port -> port
    let url = sprintf "http://0.0.0.0:%s" port
    WebHostBuilder()
        .UseKestrel()
        .UseUrls(url)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0