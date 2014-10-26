
module Tennis.Test

open NUnit.Framework
open Tennis.Main
open Tennis.parser

// I suspect my tests could be better formatted than this, this is what comes from not knowing the language
let parseInt str =
     match IntegerParser (str |> Seq.toList) with
     | Success(i, rest ) -> Assert.Pass()
     | _ -> Assert.Fail()
     

let parseFloat v str =
   match FloatParser (str |> Seq.toList) with
     | Success(f, rest ) -> Assert.AreEqual(0.000001, System.Math.Abs(f - v))
     | _ -> Assert.Fail()
     
let parseOperator str =
    match OperatorParser (str |> Seq.toList) with
     | Success(i, rest ) -> Assert.Pass()
     | _ -> Assert.Fail()


[<Test>]
let ``Parses an int``() = 
     parseInt "5"
     
[<Test>]
let ``Passing a non integer to ParseInteger fails``() =
    Assert.Throws(typeof<AssertionException>, (fun () -> parseInt "a")) |> ignore

[<Test>]
let ``Parses a float``() =
    parseFloat 5.1 "5.1"
    
[<Test>]
let ``Parses a negative float``() =
    parseFloat -5.1 "-5.1"
    
[<Test>]
let ``Parses a float with an EE``() =
    parseFloat -5.123e3 "-5.123e3"

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
    
[<Test>]
let ``Tokenize an integer``() =
    let s = tokenize "5"
    Assert.AreEqual( seq{ yield Integer(5) }, s )
    
[<Test>]
let ``Tokenize a 5*4``() =
    let s = tokenize "5*4"
    Assert.AreEqual( seq{ yield Integer(5); yield Operator('*'); yield Integer(4) }, s )