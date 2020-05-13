module Releasy.FeatureManagement.ACL.Trello.Service.Test

open System
open Expecto
open Releasy.FeatureManagement.ACL.Trello.Service
open Releasy.FeatureManagement.ACL.Trello.Model
open Releasy.FeatureManagement.Model
open FsToolkit.ErrorHandling

[<Tests>]
let testsLinkMRToFeature =
  testList "linkMergeRequestToFeatureInTrello" [
    testCase "should print related env vars" <| fun _ ->
      linkMergeRequestToFeatureInTrello
        |> Async.RunSynchronously
        |> (fun result -> Expect.isOk result "The result should not be an error")
  ]

let mergeRequest42 = { url = Uri "https://github.com/design-all-the-things/test-repo/pull/42" }
let mergeRequest1 = { url = Uri "https://github.com/design-all-the-things/test-repo/pull/1" }

let validCheckList : CheckList = {
  id = "5dd7d1be5af79c3e6b709a1e";
  idCard = "5d67bdb6e6fe5a5fa40339c2";
  name = "Feature Progress (🔒 by Releasy)";
  checkItems = [|
    {
      state = ItemState.Incomplete;
      id = "5dd7d48f8596301dece0edbd";
      name = "https://github.com/design-all-the-things/test-repo/pull/1";
    }
  |]
}

let mr42CheckItem = {
  state = ItemState.Incomplete;
  id = "42";
  name = "https://github.com/design-all-the-things/test-repo/pull/42";
}

let dummyCheckList : CheckList = {
  id = "dummy";
  idCard = "";
  name = "Dummy";
  checkItems = [||];
}

let cardShortId = "INfJJRKk"
let featureId = sprintf "https://trello.com/c/%s/1-link-mr-to-close-a-feature-using-closes-or-fixes" cardShortId

[<Tests>]
let testsLinkProcess =
  testList "makeLink" [
    testCase "should find Feature Progress checklist" <| fun _ ->
      let listCheckLists = fun _ -> [|validCheckList|] |> Result.Ok |> Async.result
      let createCheckList = fun _ -> "createCheckList should not have been called"
                                      |> RequestError |> Result.Error |> Async.result
      let createCheckItem = fun _ _ -> "createCheckItem should not have been called"
                                      |> RequestError |> Result.Error |> Async.result

      (mergeRequest1, cardShortId)
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result -> Expect.isOk result "The result should not be an error")

    testCase "should create Feature Progress checklist when no checklists in card" <| fun _ ->
      let listCheckLists = fun _ -> [||] |> Result.Ok |> Async.result
      let mutable createdCheckListCard = ""
      let createCheckList = fun cardId ->
        createdCheckListCard <- cardId
        validCheckList |> Result.Ok |> Async.result
      let createCheckItem = fun _ _ -> mr42CheckItem |> Result.Ok |> Async.result

      (mergeRequest42, cardShortId)
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result ->
          Expect.isOk result "The result should not be an error"
          Expect.equal createdCheckListCard cardShortId "check list should have been created"
        )

    testCase "should create Feature Progress checklist when not in card checklists at all" <| fun _ ->
      let listCheckLists = fun _ -> [|dummyCheckList|] |> Result.Ok |> Async.result
      let mutable checkListCreated = false
      let createCheckList = fun _ ->
        checkListCreated <- true
        validCheckList |> Result.Ok |> Async.result
      let createCheckItem = fun _ _ -> mr42CheckItem |> Result.Ok |> Async.result

      (mergeRequest42, cardShortId)
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result ->
          Expect.isOk result "The result should not be an error"
          Expect.isTrue checkListCreated "check list should have been created"
        )

    testCase "should add MR as a check item to Feature Progress checklist" <| fun _ ->
      let listCheckLists = fun _ -> [|validCheckList|] |> Result.Ok |> Async.result
      let createCheckList = fun _ -> "createCheckList should not have been called"
                                      |> RequestError |> Result.Error |> Async.result
      let mutable checkItemCreated = false
      let createCheckItem = fun _ _ ->
        checkItemCreated <- true
        mr42CheckItem |> Result.Ok |> Async.result

      (mergeRequest42, cardShortId)
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result ->
          Expect.isOk result "The result should not be an error"
          Expect.isTrue checkItemCreated "check list should have been created"
        )

    testCase "should fail when error on create check item for MR" <| fun _ ->
      let listCheckLists = fun _ -> [|validCheckList|] |> Result.Ok |> Async.result
      let createCheckList = fun _ -> "createCheckList should not have been called"
                                      |> RequestError |> Result.Error |> Async.result
      let createCheckItem = fun _ _ -> "createCheckItem is in error"
                                      |> RequestError |> Result.Error |> Async.result

      (mergeRequest42, cardShortId)
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result -> Expect.isError result "create check item has failed. The result should be in error")
  ]

[<Tests>]
let testsToTrelloId =
  testList "toTrelloId" [
    testCase "should return the id if it is not a URI" <| fun _ ->
      let featureId = "toto"
      let cardId = featureId |> toTrelloId
      Expect.equal cardId featureId "Wrong cardId extraction"

    testCase "should return the feature id if it is not a trello card URI" <| fun _ ->
      let featureId = "https://trello.com/b/efb4q0WW/releasy"
      let cardId = featureId |> toTrelloId
      Expect.equal cardId featureId "Wrong cardId extraction"

    testCase "should return the card id if it is a trello URI" <| fun _ ->
      let featureId = "https://trello.com/c/INfJJRKk/1-link-mr-to-close-a-feature-using-closes-or-fixes"
      let cardId = featureId |> toTrelloId
      Expect.equal cardId "INfJJRKk" "Wrong cardId extraction"
  ]
