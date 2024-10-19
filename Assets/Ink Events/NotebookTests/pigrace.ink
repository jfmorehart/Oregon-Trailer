INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

~ causeEvent(10)
You stumble across a pig race happening in the field. You can bet on one of them 

    *[bet on pig one]
        -> win
    *[bet on pig two]
        -> lose
    *[bet on pig three]
        -> lose
    *[bet on pig four]
        ->lose
=== win===
this one wins #spr pigrace2

->END
=== lose ===
this one loses #spr pigpace2
->END
=== ending ===
You enjoy the festivities #spr pigRace2
->END
