module Tennis.parser

type ParserResult<'a> =
    | Success of 'a * list<char>
    | Failure

type Parser<'a> = list<char> -> ParserResult<'a>
    
let Either (p1: Parser<'a>) (p2: Parser<'a>) : Parser<'a> =
    let p stream =
        match p1 stream with
        | Failure -> p2 stream
        | res -> res
    in p
    
let (<|>) = Either
    
let Apply (p: Parser<'a>) (f: 'a -> 'b) : Parser<'b> =
    let q stream =
        match p stream with
        | Success(x, rest) -> Success(f x, rest)
        | Failure -> Failure
    in q
    
let rec Many (p: Parser<'a>) : Parser<List<'a>> =
    let q stream =
        match p stream with
        | Failure -> Success([], stream)
        | Success(x, rest) ->  (Apply (Many p) (fun xs -> x::xs)) rest
    in q
    
let CharParser (c: char) : Parser<char> =
    let p stream =
        match stream with
        | x::xs when x = c -> Success(x, xs)
        | _ -> Failure
    in p    

let DigitParser : Parser<char> =
    ['0'..'9']
    |> List.map CharParser
    |> List.reduce Either

let Return (x: 'a): Parser<'a> =
    let p stream = Success(x, stream)
    in p

let Bind (p: Parser<'a>) (f: 'a -> Parser<'b>) : Parser<'b> =
    let q stream =
        match p stream with
        | Success(x, rest) -> (f x) rest
        | Failure -> Failure
    in q

type ParserBuilder() =
    member x.Bind(p, f) = Bind p f
    member x.Return(y) = Return y

let parse = new ParserBuilder()

let Many1 p : Parser<list<'a>> =
    parse {
        let! x = p          // Applies p
        let! xs = (Many p)  // Applies (Many p) recursively
        return x :: xs      // returns the cons of the two results
    }