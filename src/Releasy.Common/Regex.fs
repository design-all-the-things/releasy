module Releasy.Common.Regex

open System.Text.RegularExpressions

let (|Regex|_|) pattern input =
    let regexMatch = Regex.Match(input, pattern, RegexOptions.IgnoreCase)
    if regexMatch.Success then Some(List.tail [ for g in regexMatch.Groups -> g.Value ])
    else None
