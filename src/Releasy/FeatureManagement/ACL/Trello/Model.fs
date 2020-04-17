module Releasy.FeatureManagement.ACL.Trello.Model

open Thoth.Json

type ItemState =
  | Complete
  | Incomplete

  static member Decode : Decoder<ItemState> =
    Decode.string
      |> Decode.andThen (function
          | "complete" -> Decode.succeed Complete
          | "incomplete" -> Decode.succeed Incomplete
          | state -> Decode.fail (sprintf "Unknown state %s" state))

type CheckItem =
  { id: string
    name: string
    state: ItemState }

  static member Decode : Decoder<CheckItem> =
    Decode.object
        (fun get ->
            { id = get.Required.Field "id" Decode.string
              name = get.Required.Field "name" Decode.string
              state = get.Required.Field "state" ItemState.Decode
            }
        )

type CheckList =
  { id: string
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
