module Releasy.Common.Http

open FsToolkit.ErrorHandling
open HttpFs.Client
open Hopac

let bodyOn2xxOr3xx (toError: string -> 'a) (response: Response) =
  match response.statusCode with
  | code when code < 400 -> response |> Response.readBodyAsString |> Job.map Ok
  | _                    -> response |> Response.readBodyAsString |> Job.map (toError >> Error)

type HttpError =
  | NetworkError of e:exn
  | WrongStatusError of message:string

let withQueryString (queryParams: List<string * string>) (request: Request) =
  List.foldBack (fun (key, value) -> Request.queryStringItem key value) queryParams request

let makeRequest method queryParams url =
  Request.createUrl method url
    |> withQueryString queryParams
    |> tryGetResponse
    |> Job.map (Choice.toResult >> (Result.mapError NetworkError))
    |> JobResult.bind (bodyOn2xxOr3xx WrongStatusError)
    |> Job.toAsync

let getHttp = makeRequest Get
let postHttp = makeRequest Post
