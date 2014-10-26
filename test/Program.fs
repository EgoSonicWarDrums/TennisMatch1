
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
open Tennis.parser

type Tokens =
    | Integer of int
    | Float of float
    | Operator of char
    | Whitespace of char

// Some notes.  I change parsing integers to use a parser combinator so that it would
// be a little more challenging and interesting.  I also did this to solve a problem
// which we would have run into with the current design: how do we tokenize the stream
// of characters, that is given the string "2 + 3" we'd need to break it down into "2",
// "+", and "3" before we could call |Integer| or |Operator| on it.  With this, you pass
// in a list of chars and it greedily converts the characters in the list into an integer
// until it hits a non digit char.  The IntegerParser returns a tuple of (int,remaining chars)
//
// parse is a Computational Expression
// <|> is an operator that represents Either.  So for s it will try to parse a '+' or an '-'
//
// There is an example use of IntegerParser in the main function
let IntegerParser = 
    parse{ 
        let! s = ( CharParser '+' <|> CharParser '-' ) 
                 <|> Return '+'
        let! l = parse { 
                    let! l = Many1 DigitParser
                    return l }
        return Integer(int( new System.String( s::l |> List.toArray ) )) }
        
let OperatorParser =
    parse{ 
        let! s = ( CharParser '+' <|> CharParser '-'  <|> CharParser '*' <|> CharParser '/' ) 
        return Operator(s) }
        
let WhitespaceParser =
    parse{ 
        let! s = ( CharParser ' ' <|> CharParser '\n'  <|> CharParser '\t' <|> CharParser '\r' ) 
        return Whitespace(s) }

let FloatParser = 
    parse{ 
        let! s = ( CharParser '+' <|> CharParser '-' ) 
                 <|> Return '+'
        let! l = parse { 
                    let! l = Many1 DigitParser
                    return l }
        return float( new System.String( s::l |> List.toArray ) ) }
        
let Parse clist =
    match IntegerParser clist with
    | Success(t,l) -> Success(t,l)
    | Failure -> match OperatorParser clist with
                 | Success(t,l) -> Success(t,l)
                 | Failure -> WhitespaceParser clist
        
let tokenize str = 
    let rec tokenize clist = 
        seq{
            match Parse clist with
            | Success(t,l) -> yield t
                              yield! tokenize l
            | Failure -> ()
        }
    tokenize (str |> Seq.toList)
        
// The first thing I wanted to do was unit test this.  I peered into nuget,
// I think this works /Library/Frameworks/Mono.framework/Versions/3.6.0/bin/nuget install FsUnit 
// I discovered the way to add tests was with a new project.  Also I super hate that this project is
// called test, but I don't hate it enough to rename it.  

// actually maybe xamarin already had an option for an nunit project and I missed it?  regardless I added the project
// Xamarin crashed several times but eventually complied.

// more shameless msdn copying, but I want something to test
let (|Float|_|) (str: string) =
   let mutable floatvalue = 0.0
   if System.Double.TryParse(str, &floatvalue) then Some(floatvalue)
   else None

let (|Operator|_|) (str: string) =
    match str.[0] with
    | '+' -> Some("+")
    | '-' -> Some("-")
    | '*' -> Some("*")
    | '/' -> Some("/")
    | '^' -> Some("^")
    | _ -> None


[<EntryPoint>]
let main args = 
    let i = IntegerParser ( "12345 + 12345" |> Seq.toList )
    match i with
    | Success(i, rest) -> printfn "%A\nRest of string is: %A" i rest
    | _ -> printfn "Hmmm"
    0
