module Releasy.FeatureManagement.ACL.Trello

open System
open FsConfig
open FSharp.Control.Tasks.V2.ContextInsensitive
open Releasy.FeatureManagement.Model

[<Convention("RELEASY_TRELLO")>]
type TrelloConfig = {
  ApiKey : string
  Token : string
}

let linkMergeRequestToFeatureInTrello = task {
  let result = EnvConfig.Get<TrelloConfig>()
  match result with
  | Ok config -> printfn "API KEY: %s" config.ApiKey
  | Error e -> printfn "Error"
}
