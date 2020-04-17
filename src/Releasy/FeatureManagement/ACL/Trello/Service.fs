module Releasy.FeatureManagement.ACL.Trello.Service

open System
open FsConfig
open Releasy.FeatureManagement.Model
open Hopac
open HttpFs.Client
open FsToolkit.ErrorHandling

type TrelloError =
  | NetworkError of e:exn
  | RequestError of message:string
  | ConfigError of ConfigParseError

[<Convention("RELEASY_TRELLO")>]
type TrelloConfig = {
  ApiKey : string
  Token : string
}

let readConfig =
  EnvConfig.Get<TrelloConfig>()
    |> Result.mapError ConfigError
    |> Async.result

let bodyOn2xxOr3xx (response: Response) =
  match response.statusCode with
  | code when code < 400 -> response |> Response.readBodyAsString |> Job.map Ok
  | _                    -> response |> Response.readBodyAsString |> Job.map (RequestError >> Error)

let checkOrCreateChecklist (config : TrelloConfig) =
  Request.createUrl Get "https://api.trello.com/1/cards/6QrSHK8z/checklists"
    |> Request.queryStringItem "key" config.ApiKey
    |> Request.queryStringItem "token" config.Token
    |> tryGetResponse
    |> Job.map (Choice.toResult >> (Result.mapError NetworkError))
    |> JobResult.bind bodyOn2xxOr3xx
    |> Job.toAsync

let linkMergeRequestToFeatureInTrello = asyncResult {
  let! config = readConfig
  let! checklist = checkOrCreateChecklist config
  printfn "API KEY: %s, checklist: %s" config.ApiKey checklist
}
