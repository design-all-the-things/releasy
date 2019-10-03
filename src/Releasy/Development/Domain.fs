module Releasy.Development.Domain

open System
open Releasy.Development.Model

let linkMergeRequestToFeature (mergeRequest: MergeRequest) : MRLinkedEvent = 
  MRNotLinkedToAnyFeature { mergeRequest = mergeRequest }
