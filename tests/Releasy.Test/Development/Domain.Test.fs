module Releasy.Development.Domain.Test

open Expecto
open System

open Releasy.Development.Model
open Releasy.Development.Domain

let  featureIdentifier = "https://trello.com/c/INfJJRKk/1-link-mr-to-close-a-feature-using-closes-or-fixes"

let linkMergeRequestToFeatureShouldLinkMRToFeature (mrDescription: string): Unit =
  let mergeRequestWhichClosesAFeature =
    {
      title = "Test";
      description = mrDescription;
      url = new Uri("https://github.com/xxxx/42")
    }
  let mrLinkEvent = linkMergeRequestToFeature mergeRequestWhichClosesAFeature
  match mrLinkEvent with 
  | MRLinkedToFeature event ->
      Expect.equal event.featureIdentifier featureIdentifier "Wrong Identifier"
  | _ -> Expect.isTrue false ("Wrong event type for " + mrDescription)

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

    testCase "should link MR to feature with `xxx closes <id>`" <| fun _ -> 
      linkMergeRequestToFeatureShouldLinkMRToFeature ("This PR closes " + featureIdentifier)

    testCase "should link MR to feature with `closes <id> xxx`" <| fun _ -> 
      linkMergeRequestToFeatureShouldLinkMRToFeature ("closes " + featureIdentifier + " :tada")

    testCase "should link MR to feature with `close <id> xxx`" <| fun _ -> 
      linkMergeRequestToFeatureShouldLinkMRToFeature ("close " + featureIdentifier + " :tada")

    testCase "should link MR to feature with `close 	 <id> xxx`" <| fun _ -> 
      linkMergeRequestToFeatureShouldLinkMRToFeature ("close 	" + featureIdentifier + " :tada")

    testCase "should link MR to feature with `closing <id>`" <| fun _ -> 
      linkMergeRequestToFeatureShouldLinkMRToFeature ("closing " + featureIdentifier)

    testCase "should link MR to feature with `CLose <id>`" <| fun _ -> 
      linkMergeRequestToFeatureShouldLinkMRToFeature ("CLose " + featureIdentifier)
]
