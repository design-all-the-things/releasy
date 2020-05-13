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
  | NetworkError of e:exn
  | RequestError of message:string
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
  Request.createUrl Get (sprintf "https://api.trello.com/1/cards/%s/checklists" featureId)
    |> Request.queryStringItem "key" config.ApiKey
    |> Request.queryStringItem "token" config.Token
    |> tryGetResponse
    |> Job.map (Choice.toResult >> (Result.mapError NetworkError))
    |> JobResult.bind (bodyOn2xxOr3xx RequestError)
    |> Job.map (Result.bind (Decode.fromString (Decode.array CheckList.Decode) >> Result.mapError DecodeError))
    |> Job.toAsync

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

let linkMergeRequestToFeatureInTrello = asyncResult {
  let! config = readConfig
  printfn "API KEY: %s" config.ApiKey
  let! checkLists = listCheckLists config "6QrSHK8z"
  printfn "checkList: %s" (checkLists |> Array.head |> (fun c -> c.name))
}
