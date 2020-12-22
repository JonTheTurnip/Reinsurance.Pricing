module Tests

open System
open Xunit
open Reinsurance.Pricing
open Reinsurance.Pricing.Layer


[<Fact>]
let When_LossLessThanExcessAndOnlyExcessSupplied_ExpectNoFtLossEvents () =      
    let loss = 100.0
    let excess = OccExcess(Some 200.0)
    let limit = OccLimit(Some infinity) 
    let layerLoss = calcOccLoss limit excess loss
    Assert.Equal(layerLoss, 0.0)
