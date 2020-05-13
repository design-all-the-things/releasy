module Releasy.FeatureManagement.ACL.Trello.Service

open System
open FsConfig
open Releasy.FeatureManagement.Model
open Hopac
open HttpFs.Client
open FsToolkit.ErrorHandling
open Releasy.FeatureManagement.ACL.Trello.Model
open Thoth.Json.Net
open FSharpPlus
open Releasy.Common.Http
open Releasy.Common.Regex

type TrelloError =
  | RequestError of HttpError
  | ConfigError of ConfigParseError
  | DecodeError of string

[<Convention("RELEASY_TRELLO")>]
type TrelloConfig = {
  ApiKey : string
  Token : string
}

let readConfig =
  EnvConfig.Get<TrelloConfig>()
    |> Result.mapError ConfigError
    |> Async.result

let listCheckLists (config : TrelloConfig) (featureId: CardShortId) =
  sprintf "https://api.trello.com/1/cards/%s/checklists" featureId
    |> getHttp ["key", config.ApiKey; "token", config.Token]
    |> AsyncResult.mapError RequestError
    |> Async.map (Result.bind (Decode.fromString (Decode.array CheckList.Decode) >> Result.mapError DecodeError))

let createFeatureProgressCheckList (config: TrelloConfig) (featureId: CardShortId) =
  sprintf "https://api.trello.com/1/cards/%s/checklists" featureId
    |> postHttp [
        "name", FEATURE_PROGRESS_CHECKLIST_NAME;
        "key", config.ApiKey;
        "token", config.Token
      ]
    |> AsyncResult.mapError RequestError
    |> Async.map (Result.bind (Decode.fromString CheckList.Decode >> Result.mapError DecodeError))

let createCheckItem (config: TrelloConfig) (checkListId: CheckListId) (itemName: CheckItemName) =
  sprintf "https://api.trello.com/1/checklists/%s/checkItems" checkListId
    |> postHttp [
        "name", itemName;
        "key", config.ApiKey;
        "token", config.Token
      ]
    |> AsyncResult.mapError RequestError
    |> Async.map (Result.bind (Decode.fromString CheckItem.Decode >> Result.mapError DecodeError))

let couple a = (a, a)

let extractCardId (cardUri: Uri) =
  match cardUri.OriginalString with
    | Regex @"trello.com/c/([^/]*)" [ cardId ] -> cardId
    | _ -> cardUri.OriginalString

let toTrelloId featureId : CardShortId =
  featureId
    |> Result.protect Uri
    |> either extractCardId (konst featureId)

let makeLink (listCheckLists: CardShortId -> Async<Result<CheckList array, TrelloError>>)
             (createFeatureProgressCheckList: CardShortId -> Async<Result<CheckList, TrelloError>>)
             (createCheckItem: CheckListId -> CheckItemName -> Async<Result<CheckItem, TrelloError>>)
             (mergeRequest: MergeRequest, featureId: CardShortId)
             : Async<Result<unit, TrelloError>> =
  featureId
    |> listCheckLists
    |> AsyncResult.map (Array.tryFind isFeatureProgressCheckList)
    |> AsyncResult.bind (function
                          | Some checkList -> checkList |> AsyncResult.retn
                          | None -> createFeatureProgressCheckList featureId)
    |> AsyncResult.map (couple >> mapItem2 (findCheckItem mergeRequest.url.OriginalString))
    |> AsyncResult.bind (function
                          | _, Some checkItem -> checkItem |> AsyncResult.retn
                          | checkList, None -> createCheckItem checkList.id mergeRequest.url.OriginalString)
    |> AsyncResult.map ignore

let doMakeLink config =
  makeLink
    (listCheckLists config)
    (createFeatureProgressCheckList config)
    (createCheckItem config)

let linkMergeRequestToFeatureInTrello (mergeRequest: MergeRequest, feature: Feature) = asyncResult {
  let! config = readConfig
  do! doMakeLink config (mergeRequest, toTrelloId feature.id)
}
