module Releasy.FeatureManagement.ACL.Trello.Service.Test

open System
open Expecto
open Releasy.FeatureManagement.ACL.Trello.Service
open FsToolkit.ErrorHandling
open System.Threading.Tasks

[<Tests>]
let tests =
  testList "linkMergeRequestToFeatureInTrello" [
    testCase "should print related env vars" <| fun _ ->
      linkMergeRequestToFeatureInTrello
        |> Async.RunSynchronously
        |> (fun result -> Expect.isOk result "The result should not be an error")
  ]