module Releasy.FeatureManagement.ACL.Trello.Test

open System
open Expecto
open Releasy.FeatureManagement.ACL.Trello

[<Tests>]
let tests =
  testList "linkMergeRequestToFeatureInTrello" [
    testCase "should print related env vars" <| fun _ ->
      printfn "HEYYYYYY Rodriguez !!!!"
      let link = linkMergeRequestToFeatureInTrello
      link.Wait()
  ]