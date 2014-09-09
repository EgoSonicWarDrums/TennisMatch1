
// We'll start simple:
// This project will take algebraic statements (e.g. 2 + 2, x^3 - 3, x * y)
// and evaluate them.  This will only take integers and text variables for input.  A variable is [A-Za-z][alphnum]*
// A user will enter:
//     2 + 2 <enter> and get "=> 4"
//     2 + x with x = 3 and get "=> 5"
//
// Round 1: I'm adding active patterns to handle parsing Integers and writing the BNF rules
//    Integer := 0-9,[{0-9}]
//    Variable := a-z, { a-z | 0-9}
//    Op := + | - | * | / | ^
//    Expression := (NUM | VAR), { OP, NUM|VAR}
//    Statement := EXPR [ "with" { VAR "=" NUM}+ ] 

module Tennis.Main

open System
open System.Text.RegularExpressions

// This is shamelessly copied from the MSDN example.  But I needed something
// to put code in here to get the ball rolling and show what an Active Pattern
// is.  This partitions an input space:  here it will partition the space of strings
// into either "Integer" or None
let (|Integer|_|) (str: string) =
   let mutable intvalue = 0
   if System.Int32.TryParse(str, &intvalue) then Some(intvalue)
   else None

[<EntryPoint>]
let main args = 
    Console.WriteLine("Hello world!")
    
    0

