// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System

let secrets = ["game"; "life"; "fsharp"; "hangman"]

let maskWord secret (word: string) (c: Option<char>) =
    String.map (fun x -> if(match c with Some _c -> _c = x || word.Contains(x.ToString()) | _ -> false) then x else '_') secret

type Game(secret: string, ?word0: string, ?hangman0: int) =     
    let word = defaultArg word0 (maskWord secret "" None)
    let hangman = defaultArg hangman0 6 
    do printfn "game: %s %i" word hangman 
    member this.Secret = secret
    member this.Word =  word
    member this.Hangman = hangman 
    member this.isWon = this.Word = this.Secret
    member this.isLost = this.Hangman = 0


let guess (game: Game) (c: char) = 
    match game.Secret.Contains(c.ToString()) with 
        | true -> Game(game.Secret, maskWord game.Secret game.Word (Some(c)), game.Hangman)
        | _ -> Game(game.Secret, game.Word, game.Hangman - 1)

let continueWith (game: Game) = 
    match game with
        | g when g.isLost -> printf "You loose, the secret was %s" game.Secret; None
        | g when g.isWon -> printf "You won"; None
        | x -> Some x    

[<EntryPoint>]
let main argv = 
    let index = Random().Next(secrets.Length)
    let secret: string = List.nth secrets index
    let rec play = fun (game: Game) -> 
        Console.ReadKey(true).KeyChar |> 
        guess game |>
        continueWith |>        
        Option.iter play

    play(Game(secret))
    let wait = Console.ReadLine()
    0 // return an integer exit code
