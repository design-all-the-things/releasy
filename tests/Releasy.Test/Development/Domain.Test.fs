module Releasy.Development.Domain.Test

open Expecto
open System

open Releasy.Development.Model
open Releasy.Development.Domain

[<Tests>]
let tests =
  testList "linkMergeRequestToFeature" [
    testCase "should not link to any feature" <| fun _ ->
      let mergeRequest =
        {
          title = "Test";
          description = "Test";
          url = new Uri("https://github.com/xxxx/42")
        }
      let mrLinkEvent = linkMergeRequestToFeature mergeRequest
      match mrLinkEvent with 
      | MRNotLinkedToAnyFeature event -> Expect.equal event.mergeRequest mergeRequest "Wrong MR"
      | _ -> Expect.isTrue false "Wrong event type"
  ]
