module Releasy.Development.ACL.Github

open System
open Releasy.Development.Model

type PullRequest = {
    url: string
    title: string
    body: string
}

type WebHookPayload = {
    pull_request: PullRequest
}

let fromPayloadToMergeRequest (payload: WebHookPayload): MergeRequest =
    {
        title = payload.pull_request.title;
        url = new Uri(payload.pull_request.url);
        description = payload.pull_request.body
    }
