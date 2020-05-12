module Releasy.FeatureManagement.ACL.Trello.Service.Test

open System
open Expecto
open Releasy.FeatureManagement.ACL.Trello.Service
open Releasy.FeatureManagement.ACL.Trello.Model
open Releasy.FeatureManagement.Model
open FsToolkit.ErrorHandling
open System.Threading.Tasks

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

[<Tests>]
let testsLinkProcess =
  testList "makeLink" [
    testCase "should find Feature Progress checklist" <| fun _ ->
      let listCheckLists = [|validCheckList|] |> Result.Ok |> Async.result
      let createCheckList = fun _ -> "createCheckList should not have been called"
                                      |> RequestError |> Result.Error |> Async.result
      let createCheckItem = fun _ _ -> "createCheckItem should not have been called"
                                      |> RequestError |> Result.Error |> Async.result

      mergeRequest1
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result -> Expect.isOk result "The result should not be an error")

    testCase "should create Feature Progress checklist when no checklists in card" <| fun _ ->
      let listCheckLists = [||] |> Result.Ok |> Async.result
      let mutable checkListCreated = false
      let createCheckList = fun _ ->
        checkListCreated <- true
        validCheckList |> Result.Ok |> Async.result
      let createCheckItem = fun _ _ -> mr42CheckItem |> Result.Ok |> Async.result

      mergeRequest42
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result ->
          Expect.isOk result "The result should not be an error"
          Expect.isTrue checkListCreated "check list should have been created"
        )

    testCase "should create Feature Progress checklist when not in card checklists at all" <| fun _ ->
      let listCheckLists = [|dummyCheckList|] |> Result.Ok |> Async.result
      let mutable checkListCreated = false
      let createCheckList = fun _ ->
        checkListCreated <- true
        validCheckList |> Result.Ok |> Async.result
      let createCheckItem = fun _ _ -> mr42CheckItem |> Result.Ok |> Async.result

      mergeRequest42
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result ->
          Expect.isOk result "The result should not be an error"
          Expect.isTrue checkListCreated "check list should have been created"
        )

    testCase "should add MR as a check item to Feature Progress checklist" <| fun _ ->
      let listCheckLists = [|validCheckList|] |> Result.Ok |> Async.result
      let createCheckList = fun _ -> "createCheckList should not have been called"
                                      |> RequestError |> Result.Error |> Async.result
      let mutable checkItemCreated = false
      let createCheckItem = fun _ _ ->
        checkItemCreated <- true
        mr42CheckItem |> Result.Ok |> Async.result

      mergeRequest42
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result ->
          Expect.isOk result "The result should not be an error"
          Expect.isTrue checkItemCreated "check list should have been created"
        )

    testCase "should fail when error on create check item for MR" <| fun _ ->
      let listCheckLists = [|validCheckList|] |> Result.Ok |> Async.result
      let createCheckList = fun _ -> "createCheckList should not have been called"
                                      |> RequestError |> Result.Error |> Async.result
      let createCheckItem = fun _ _ -> "createCheckItem is in error"
                                      |> RequestError |> Result.Error |> Async.result

      mergeRequest42
        |> makeLink listCheckLists createCheckList createCheckItem
        |> Async.RunSynchronously
        |> (fun result -> Expect.isError result "create check item has failed. The result should be in error")
  ]
