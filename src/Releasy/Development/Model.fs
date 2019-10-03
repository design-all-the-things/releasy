module Releasy.Development.Model

open System

type MergeRequest = 
    {
        title: string
        description: string
        url: Uri
    }

type MRLinkedToFeatureEvent =
    {
      mergeRequest: MergeRequest
      featureIdentifier: string
    }

type MRNotLinkedToAnyFeatureEvent =
    {
      mergeRequest: MergeRequest
    }

type MRLinkedEvent =
    | MRLinkedToFeature of MRLinkedToFeatureEvent
    | MRNotLinkedToAnyFeature of MRNotLinkedToAnyFeatureEvent
