module Releasy.Development.Domain

open System.Text.RegularExpressions
open Releasy.Development.Model

let (|Regex|_|) pattern input =
    let regexMatch = Regex.Match(input, pattern, RegexOptions.IgnoreCase)
    if regexMatch.Success then Some(List.tail [ for g in regexMatch.Groups -> g.Value ])
    else None

let linkMergeRequestToFeature (mergeRequest: MergeRequest) : MRLinkedEvent =
    match mergeRequest.description with
    | Regex @"clos(?:es?|ing)\s+(\S*)" [ featureIdentifier ] ->
        MRLinkedToFeature { mergeRequest = mergeRequest; featureIdentifier = featureIdentifier }
    | _ -> MRNotLinkedToAnyFeature { mergeRequest = mergeRequest }
 