module Releasy.Development.Model

open System

type MergeRequest = 
    {
        title: string
        description: string
        url: Uri
    }

type MRLinkedEvent =
    | MRLinkedToFeature of MRLinkedToFeature
    | MRNotLinkedToFeature of MRNotLinkedToFeature

and MRLinkedToFeature =
    {
      mergeRequest: MergeRequest
      featureIdentifier: string
    }

and MRNotLinkedToFeature =
    {
      mergeRequest: MergeRequest
    }
    