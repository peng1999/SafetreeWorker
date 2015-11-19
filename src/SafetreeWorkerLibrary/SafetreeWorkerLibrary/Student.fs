//[<AutoOpen>]
module SafetreeWorkerLibrary.Student

let getUnfinishedWorks user = 
    printfn "done getUnfinishedWorks"
    [Work(); Work()]
    //raise 

let doWork work = 
    printfn "done doWork"