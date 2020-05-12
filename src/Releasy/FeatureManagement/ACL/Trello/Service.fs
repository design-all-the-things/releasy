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

let bodyOn2xxOr3xx (response: Response) =
  match response.statusCode with
  | code when code < 400 -> response |> Response.readBodyAsString |> Job.map Ok
  | _                    -> response |> Response.readBodyAsString |> Job.map (RequestError >> Error)

let listCheckLists (config : TrelloConfig) =
  Request.createUrl Get "https://api.trello.com/1/cards/6QrSHK8z/checklists"
    |> Request.queryStringItem "key" config.ApiKey
    |> Request.queryStringItem "token" config.Token
    |> tryGetResponse
    |> Job.map (Choice.toResult >> (Result.mapError NetworkError))
    |> JobResult.bind bodyOn2xxOr3xx
    |> Job.map (Result.bind (Decode.fromString (Decode.array CheckList.Decode) >> Result.mapError DecodeError))
    |> Job.toAsync

let couple a = (a, a)

let makeLink (listCheckLists: Async<Result<CheckList array, TrelloError>>)
             (createFeatureProgressCheckList: CardShortId -> Async<Result<CheckList, TrelloError>>)
             (createCheckItem: CheckListId -> CheckItemName -> Async<Result<CheckItem, TrelloError>>)
             (mergeRequest: MergeRequest)
             : Async<Result<unit, TrelloError>> =
  listCheckLists
    |> AsyncResult.map (Array.tryFind isFeatureProgressCheckList)
    |> AsyncResult.bind (function
                          | Some checkList -> checkList |> AsyncResult.retn
                          | None -> createFeatureProgressCheckList "6QrSHK8z")
    |> AsyncResult.map (couple >> mapItem2 (findCheckItem mergeRequest.url.OriginalString))
    |> AsyncResult.bind (function
                          | _, Some checkItem -> checkItem |> AsyncResult.retn
                          | checkList, None -> createCheckItem checkList.id mergeRequest.url.OriginalString)
    |> AsyncResult.map ignore

let linkMergeRequestToFeatureInTrello = asyncResult {
  let! config = readConfig
  printfn "API KEY: %s" config.ApiKey
  let! checkLists = listCheckLists config
  printfn "checkList: %s" (checkLists |> Array.head |> (fun c -> c.name))
}
