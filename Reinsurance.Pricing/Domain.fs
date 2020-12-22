namespace Reinsurance.Pricing

    // NOTES:
    // 1. Have opted to use type alias rather than  single case union for performance (see p.80 of DMMF), although
    //    single case unions are the better option from pure domain modelling perspective

    
    //// *** Simulations, Events and Losses ***    
    
    /// Data types
    type EventId = int option 
    type Loss = float
    type SimYearId = int 

    // Poisson
    type MeanFreq = float
    
    // Binomial
    type Pval = float
    type Ntrials = int

    // Continuous uniform
    type SevMin = float
    type SevMax = float
    
    // Beta
    type Alpha = float
    type Beta = float  
    type Location = float
    type Scale = float

    //
    type Event = { EventId : EventId; Loss : Loss }        
    type SimYear = { SimYearId : SimYearId; Events : Event list }   
    
    // Specific pdf freq funcs (add more later)
    type PoissonFreq = MeanFreq -> int
    type BinomialFreq = Pval -> Ntrials -> int

    // Specific pdf severity funcs
    type UniformSeverity = SevMin -> SevMax -> float
    type BetaSeverity = Alpha -> Beta -> Location -> Scale -> float

    // Generic random pdf sampler funcs
    type RandomFrequency = unit -> int
    type RandomSeverity = unit -> float

    // Generic random simyear generator func
    type SimYearGenerator = RandomFrequency -> RandomSeverity -> SimYearId -> SimYear 

    
    //// *** Layers *** (single case union as a value type see DMMF p.82)
    
    [<Struct>]
    type OccLimit = OccLimit of float option
    [<Struct>]
    type OccExcess = OccExcess of float option
    [<Struct>]
    type AggLimit = AggLimit of float option
    [<Struct>]
    type AggExcess = AggExcess of float option

    type CalcOccLoss = OccLimit -> OccExcess -> Loss -> Loss

    type CalcAggLoss = AggLimit -> AggExcess -> CalcOccLoss -> Loss list -> Loss list

    type CalcLayerLoss = OccLimit -> OccExcess -> AggLimit -> AggExcess -> CalcOccLoss -> CalcAggLoss -> Loss list -> Loss list


    //// *** Layers Old way***    

    //type OccLimit = float
    //type OccExcess = float
    //type AggLimit = float
    //type AggExcess = float
    
    //type OccTerms = { OccLimit : OccLimit;  OccExcess : OccExcess }
    //type AggTerms = { AggLimit : AggLimit; AggExcess : AggExcess }
    //type Layer = { OccTerms : OccTerms;  AggTerms : AggTerms }
    

    



    


    
