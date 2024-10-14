INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)
You stumble across a pig race happening in the field. You can bet on one of them #spr: PigRace1

    *[bet on pig one]
        -> win
    *[bet on pig two]
        -> lose
    *[bet on pig three]
        -> lose
    *[bet on pig four]
        ->lose
=== win===
this one wins #spr: PigRace2

->END
=== lose ===
this one loses #spr: PigRace3
->END
=== ending ===
You enjoy the festivities #spr: PigRace4
->END
