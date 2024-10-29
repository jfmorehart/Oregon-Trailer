INCLUDE ../globals.ink
-> main

{ player_name = "" : ->main | -> alreadyChose}

=== main ===
Which pokemon do you choose?
    + [Charmander]
        -> chosen("Charmander")
    + [Bulbasaur]
        -> chosen("Bulbasaur")
    + [Squirtle]
        -> chosen("Squirtle")
        
=== chosen(pokemon) ===
You chose {pokemon}!
~player_name = pokemon

-> END
=== alreadyChose===
You already chose {player_name}

-> END