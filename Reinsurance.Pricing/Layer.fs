namespace Reinsurance.Pricing

module Layer =

    open System

    /// OccTerms function

    let calcOccLoss: CalcOccLoss = 
        fun occLimit occExcess loss ->
        // Mapping our optional values for the purpose of calculation
        let limit =
            match occLimit with
            | OccLimit None -> infinity
            | OccLimit (Some x) -> x
        let excess =
            match occExcess with
            | OccExcess None -> 0.0
            | OccExcess (Some x) -> x
        // After mapping inputs we have a succinct logic to calc loss
        match loss with
        | loss when loss < excess -> 0.0
        | loss when loss < excess + limit -> loss - excess
        | loss when loss >= excess + limit -> limit
        | _ -> 0.0


    /// AggTerms function

    let calcAggLoss: CalcAggLoss =
        fun aggLimit aggExcess calcOccLoss losses ->
        
        // Mapping our optional values for the purpose of calculation
        let mutable limRemain =
            match aggLimit with
            | AggLimit None -> infinity
            | AggLimit (Some x) -> x
        
        let mutable xsRemain =
            match aggExcess with
            | AggExcess None -> 0.0
            | AggExcess (Some x) -> x
                
        // Calc losses
        let aggApply loss =                
            let newLoss = calcOccLoss (OccLimit (Some limRemain)) (OccExcess (Some xsRemain)) loss
            xsRemain <- xsRemain - Math.Max(0.0, loss - newLoss)
            limRemain <- limRemain - Math.Max(0.0, newLoss)
            newLoss
        ([], losses) ||> List.fold (fun acc elem -> (aggApply elem)::acc) |> List.rev  


    /// Layer function

    let calcLayerLoss: CalcLayerLoss =
        fun occLimit occExcess aggLimit aggExcess calcOccLoss calcAggLoss losses -> 
        losses 
        |> List.map(fun loss -> calcOccLoss occLimit occExcess loss)
        |> calcAggLoss aggLimit aggExcess calcOccLoss

    //let layerLoss calcAggLoss calcOccLoss losses =
        


    
    /// Old Domain

    //let calcOccLoss loss limit excess =        
    //    match loss with
    //         | loss when loss < excess -> 0.0
    //         | loss when loss < excess + limit -> loss - excess
    //         | loss when loss >= excess + limit -> limit
    //         | _ -> 0.0


    //let calcAggLoss losses aggTerms =
    //        let mutable xsRemain = aggTerms.AggExcess
    //        let mutable limRemain = aggTerms.AggLimit
    //        let aggApply loss =                
    //            let newLoss = calcOccLoss loss limRemain xsRemain
    //            xsRemain <- xsRemain - Math.Max(0.0, loss - newLoss)
    //            limRemain <- limRemain - Math.Max(0.0, newLoss)
    //            newLoss
    //        ([], losses) ||> List.fold (fun acc elem -> (aggApply elem)::acc) |> List.rev  
            
