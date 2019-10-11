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

    testCase "should link MR to feature" <| fun _ ->
      let mergeRequestWhichClosesAFeature =
        {
          title = "Test";
          description = "This PR closes https://trello.com/c/INfJJRKk/1-link-mr-to-close-a-feature-using-closes-or-fixes";
          url = new Uri("https://github.com/xxxx/42")
        }
      let mrLinkEvent = linkMergeRequestToFeature mergeRequestWhichClosesAFeature
      match mrLinkEvent with 
      | MRLinkedToFeature event ->
          Expect.equal event.mergeRequest mergeRequestWhichClosesAFeature "Wrong MR"
      | _ -> Expect.isTrue false "Wrong event type"

    testCase "should define the feature identifier" <| fun _ ->
      let featureIdentifier = "https://trello.com/c/INfJJRKk/1-link-mr-to-close-a-feature-using-closes-or-fixes"
      let mergeRequestWhichClosesAFeature =
        {
          title = "Test";
          description = "This PR closes " + featureIdentifier;
          url = new Uri("https://github.com/xxxx/42")
        }
      let mrLinkEvent = linkMergeRequestToFeature mergeRequestWhichClosesAFeature
      match mrLinkEvent with 
      | MRLinkedToFeature event ->
          Expect.equal event.featureIdentifier featureIdentifier "Wrong Identifier"
      | _ -> Expect.isTrue false "Wrong event type"
  ]
