
module Tennis.Test

open NUnit.Framework
open Tennis.Main

// I suspect my tests could be better formatted than this, this is what comes from not knowing the language
let parseInt str =
   match str with
     | Integer i -> Assert.Pass()
     | Float f -> Assert.Fail()
     | _ -> Assert.Fail()

let parseFloat str =
   match str with
     | Integer i -> Assert.Fail()
     | Float f -> Assert.Pass()
     | _ -> Assert.Fail()
     
let parseOperator str =
    match str with
    | Operator o -> Assert.Pass()
    | _ -> Assert.Fail()


[<Test>]
let ``Parses an int``() = 
     parseInt "5"

[<Test>]
let ``Parses a float``() =
    parseFloat "5.1"

[<Test>]
let ``Parses a +``() =
    parseOperator "+"
    
[<Test>]
let ``Parses a -``() =
    parseOperator "-"
    
[<Test>]
let ``Parses a *``() =
    parseOperator "*"
    
[<Test>]
let ``Parses a /``() =
    parseOperator "/"
    
[<Test>]
let ``Parses a ^``() =
    parseOperator "^"