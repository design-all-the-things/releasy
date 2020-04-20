module Releasy.FeatureManagement.ACL.Trello.Model.Test

open Expecto
open Releasy.FeatureManagement.ACL.Trello.Model
open Thoth.Json.Net

[<Tests>]
let testCheckItem =
  testList "CheckItem#Decode" [
    testCase "should decode a CheckItem from strict json" <| fun _ ->
      let checkItemJson = """{
            "state": "complete",
            "id": "5dd7d1da55b75a5e289f7a9e",
            "name": ":smile: PR #34 : Mise à jour "
         }"""
      let expectedCheckItem = {
        id = "5dd7d1da55b75a5e289f7a9e";
        name =  ":smile: PR #34 : Mise à jour ";
        state = ItemState.Complete;
      }
      let checkItem = Decode.fromString CheckItem.Decode checkItemJson

      Expect.equal checkItem (Ok expectedCheckItem) "Wrong strict CheckItem decode"
  
    testCase "should decode a CheckItem from json with other properties" <| fun _ ->
      let checkItemJson = """{
            "idChecklist": "5dd7d1be5af79c3e6b709a1e",
            "state": "incomplete",
            "idMember": null,
            "id": "5dd7d1da55b75a5e289f7a9e",
            "name": ":smile: PR #34 : Mise à jour ",
            "nameData": null,
            "pos": 16631,
            "due": null
      }"""
      let expectedCheckItem = {
        id = "5dd7d1da55b75a5e289f7a9e";
        name =  ":smile: PR #34 : Mise à jour ";
        state = ItemState.Incomplete;
      }
      let checkItem = Decode.fromString CheckItem.Decode checkItemJson

      Expect.equal checkItem (Ok expectedCheckItem) "Wrong CheckItem decode"
  ]

[<Tests>]
let testsCheckList =
  testList "CheckList#Decode" [
    testCase "should decode a CheckList from json" <| fun _ ->
      let checkListJson = """{
        "id":"5dd7d1be5af79c3e6b709a1e",
        "name":"Testing Checklist",
        "idCard":"5d67bdb6e6fe5a5fa40339c2",
        "pos":16384,
        "idBoard":"5d67bac0e7cb6f518a05518f",
        "checkItems":[
           {
              "idChecklist":"5dd7d1be5af79c3e6b709a1e",
              "state":"complete",
              "idMember":null,
              "id":"5dd7d1da55b75a5e289f7a9e",
              "name":":smile: PR #34 : Mise à jour ",
              "nameData":null,
              "pos":16631,
              "due":null
           },
           {
              "idChecklist":"5dd7d1be5af79c3e6b709a1e",
              "state":"incomplete",
              "idMember":null,
              "id":"5dd7d1e4b2d29c8783fa61ea",
              "name":"PR #45 : Delete",
              "nameData":null,
              "pos":33347,
              "due":null
           },
           {
              "idChecklist":"5dd7d1be5af79c3e6b709a1e",
              "state":"incomplete",
              "idMember":null,
              "id":"5dd7d48f8596301dece0edbd",
              "name":"https://github.com/design-all-the-things/test-repo/pull/1",
              "nameData":null,
              "pos":50313,
              "due":null
           }
        ]
      }"""
      let expectedCheckList: CheckList = {
        id = "5dd7d1be5af79c3e6b709a1e";
        idCard = "5d67bdb6e6fe5a5fa40339c2";
        name = "Testing Checklist";
        checkItems = [|
          {
            state = ItemState.Complete;
            id = "5dd7d1da55b75a5e289f7a9e";
            name = ":smile: PR #34 : Mise à jour ";
          };
          {
            state = ItemState.Incomplete;
            id = "5dd7d1e4b2d29c8783fa61ea";
            name = "PR #45 : Delete";
          };
          {
            state = ItemState.Incomplete;
            id = "5dd7d48f8596301dece0edbd";
            name = "https://github.com/design-all-the-things/test-repo/pull/1";
          }
        |]
      }
      let checkLists = Decode.fromString CheckList.Decode checkListJson

      Expect.equal checkLists (Ok expectedCheckList) "Wrong CheckList decode"

    testCase "should decode an array CheckList from json" <| fun _ ->
      let checkListsJson = """[{
        "id":"5dd7d1be5af79c3e6b709a1e",
        "name":"Testing Checklist",
        "idCard":"5d67bdb6e6fe5a5fa40339c2",
        "pos":16384,
        "idBoard":"5d67bac0e7cb6f518a05518f",
        "checkItems":[
           {
              "idChecklist":"5dd7d1be5af79c3e6b709a1e",
              "state":"complete",
              "idMember":null,
              "id":"5dd7d1da55b75a5e289f7a9e",
              "name":":smile: PR #34 : Mise à jour ",
              "nameData":null,
              "pos":16631,
              "due":null
           },
           {
              "idChecklist":"5dd7d1be5af79c3e6b709a1e",
              "state":"incomplete",
              "idMember":null,
              "id":"5dd7d1e4b2d29c8783fa61ea",
              "name":"PR #45 : Delete",
              "nameData":null,
              "pos":33347,
              "due":null
           },
           {
              "idChecklist":"5dd7d1be5af79c3e6b709a1e",
              "state":"incomplete",
              "idMember":null,
              "id":"5dd7d48f8596301dece0edbd",
              "name":"https://github.com/design-all-the-things/test-repo/pull/1",
              "nameData":null,
              "pos":50313,
              "due":null
           }
        ]
      }, {
      "id": "5dfcb2e2b084d947fb3f3047",
      "name": "Empty Checklist",
      "idCard": "5d67bdb6e6fe5a5fa40339c2",
      "pos": 32768,
      "idBoard": "5d67bac0e7cb6f518a05518f",
      "checkItems":[]
   }]"""
      let expectedCheckLists: CheckList[] = [|
        {
          id = "5dd7d1be5af79c3e6b709a1e";
          idCard = "5d67bdb6e6fe5a5fa40339c2";
          name = "Testing Checklist";
          checkItems = [|
            {
              state = ItemState.Complete;
              id = "5dd7d1da55b75a5e289f7a9e";
              name = ":smile: PR #34 : Mise à jour ";
            };
            {
              state = ItemState.Incomplete;
              id = "5dd7d1e4b2d29c8783fa61ea";
              name = "PR #45 : Delete";
            };
            {
              state = ItemState.Incomplete;
              id = "5dd7d48f8596301dece0edbd";
              name = "https://github.com/design-all-the-things/test-repo/pull/1";
            }
          |]
        };
        {
          id = "5dfcb2e2b084d947fb3f3047";
          name = "Empty Checklist";
          idCard = "5d67bdb6e6fe5a5fa40339c2";
          checkItems = [||];
        };
      |]
      let checkLists = Decode.fromString (Decode.array CheckList.Decode) checkListsJson

      Expect.equal checkLists (Ok expectedCheckLists) "Wrong CheckLists decode"
  ]
