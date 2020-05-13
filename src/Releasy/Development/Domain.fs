module Releasy.Development.Domain

open Releasy.Development.Model
open Releasy.Common.Regex

let linkMergeRequestToFeature (mergeRequest: MergeRequest) : MRLinkedEvent =
    match mergeRequest.description with
    | Regex @"clos(?:es?|ing)\s+(\S*)" [ featureIdentifier ] ->
        MRLinkedToFeature { mergeRequest = mergeRequest; featureIdentifier = featureIdentifier }
    | _ -> MRNotLinkedToFeature { mergeRequest = mergeRequest }
