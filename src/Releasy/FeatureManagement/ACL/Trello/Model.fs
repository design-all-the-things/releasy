module Releasy.FeatureManagement.ACL.Trello.Model

open Thoth.Json.Net

type ItemState =
  | Complete
  | Incomplete

  static member Decode : Decoder<ItemState> =
    Decode.string
      |> Decode.andThen (function
          | "complete" -> Decode.succeed Complete
          | "incomplete" -> Decode.succeed Incomplete
          | state -> Decode.fail (sprintf "Unknown state %s" state))

type CheckItemName = string
type CheckItem =
  { id: string
    name: CheckItemName
    state: ItemState }

  static member Decode : Decoder<CheckItem> =
    Decode.object
        (fun get ->
            { id = get.Required.Field "id" Decode.string
              name = get.Required.Field "name" Decode.string
              state = get.Required.Field "state" ItemState.Decode
            }
        )

type CardShortId = string
type CheckListId = string

type CheckList =
  { id: CheckListId
    name: string
    idCard: string
    checkItems: CheckItem[]
  }

  static member Decode =
    Decode.object
      (fun get ->
        {   id = get.Required.Field "id" Decode.string
            name = get.Required.Field "name" Decode.string
            idCard = get.Required.Field "idCard" Decode.string
            checkItems = get.Required.Field "checkItems" (Decode.array CheckItem.Decode)
        }
      )

let FEATURE_PROGRESS_CHECKLIST_NAME = "Feature Progress (🔒 by Releasy)";

let findCheckItem name checkList = Array.tryFind (fun (c: CheckItem) -> c.name = name) checkList.checkItems

let isFeatureProgressCheckList checkList = checkList.name = FEATURE_PROGRESS_CHECKLIST_NAME
