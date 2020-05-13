module Releasy.Common.Http

open HttpFs.Client
open Hopac

let bodyOn2xxOr3xx (toError: string -> 'a) (response: Response) =
  match response.statusCode with
  | code when code < 400 -> response |> Response.readBodyAsString |> Job.map Ok
  | _                    -> response |> Response.readBodyAsString |> Job.map (toError >> Error)
