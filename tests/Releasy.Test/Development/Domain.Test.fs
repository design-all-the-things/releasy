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
  
  let expectedEvent = MRLinkedToFeature { mergeRequest = mergeRequestWhichClosesAFeature; featureIdentifier = featureIdentifier }
  Expect.equal mrLinkEvent expectedEvent "Wrong identifier"

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

      let expectedEvent = MRNotLinkedToFeature { mergeRequest = mergeRequest }
      Expect.equal mrLinkEvent expectedEvent "Wrong MR"

    testCase "should link MR to feature" <| fun _ ->
      let mergeRequestWhichClosesAFeature =
        {
          title = "Test";
          description = "This PR closes https://trello.com/c/INfJJRKk/1-link-mr-to-close-a-feature-using-closes-or-fixes";
          url = new Uri("https://github.com/xxxx/42")
        }
      let mrLinkEvent = linkMergeRequestToFeature mergeRequestWhichClosesAFeature

      let expectedEvent = MRLinkedToFeature { mergeRequest = mergeRequestWhichClosesAFeature; featureIdentifier = featureIdentifier }
      Expect.equal mrLinkEvent expectedEvent "Wrong MR"

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
