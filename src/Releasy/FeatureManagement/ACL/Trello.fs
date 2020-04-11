module Releasy.FeatureManagement.ACL.Trello

open System
open FsConfig
open FSharp.Control.Tasks.V2.ContextInsensitive
open Releasy.FeatureManagement.Model
open Hopac
open HttpFs.Client
open System.Threading.Tasks

exception TrelloException of string

[<Convention("RELEASY_TRELLO")>]
type TrelloConfig = {
  ApiKey : string
  Token : string
}

let readConfig : Task<TrelloConfig> =
  match EnvConfig.Get<TrelloConfig>() with
    | Ok config -> Task.FromResult config
    | Error e -> TrelloException (e.ToString()) |> Task.FromException<TrelloConfig>

let checkOrCreateChecklist (config : TrelloConfig) =
  Request.createUrl Get "https://api.trello.com/1/cards/6QrSHK8z/checklists"
    |> Request.queryStringItem "key" config.ApiKey
    |> Request.queryStringItem "token" config.Token
    |> Request.responseAsString
    |> Job.toAsync

let makeTheLink (config : TrelloConfig) = task {
  printfn "coucou !!"
  let! checklist = checkOrCreateChecklist config |> Async.StartAsTask
  printfn "API KEY: %s, checklist: %s" config.ApiKey checklist
}

let linkMergeRequestToFeatureInTrello = task {
  let! config = readConfig
  (makeTheLink config).Wait()
}
