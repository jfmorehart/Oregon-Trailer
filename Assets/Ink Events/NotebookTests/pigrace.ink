INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

VAR roo = 1
VAR bee = 2
VAR too = 3

-> race

==race==
There is a lot of oinking to be heard in this section of the road. Upon driving closer, you notice a crowd of 7 or so people huddled around a small race track, where pigs are racing each other to run a full loop. 
Someone from the crowd notices your van pulling up, and waves you over. Once you’re close enough, they explain to you the rules, regardless of whether you’re actually interested or not: you pick a hog, if they win, you win. It loses, you lose. Simple. 
The stranger also gives you a description of each hog:

<i>Roo</i> is the smallest, the fastest, but gets the most distracted. It’s very likely that Roo will stop at one point to gnaw at rocks. Has tripped out the other hogs before.
<i>Beebee</i> is the biggest, and also the slowest. There’s been multiple occasions where Beebee has almost run over the other hogs. Beebee’s a bulldozer.
<i>Toot</i> is about average-sized. Farts a lot. They’re deadly.


    *[Bet on Roo]
        -> betRoo
    *[Bet on Beebee]
        -> betBeebee
    *[Bet on Toot]
        -> betToot

~ temp bet = RANDOM(1, 3)

{ bet == roo:
                -> betRoo 
}

{ bet == bee:

                -> betBeebee
}

{ bet == too:
                -> betToot
}

== betRoo ==
The race begins! You watch as Roo zips past the two others before stopping, as predicted. It is distracted by some shiny trinkets that some stranger has dropped not far from it. 
Its eyes glow with the desire to lick metal... and in the process, trips out Beebee. The big pig falls on its behind with its legs in the air. 
Toot on the other hand... has barely left the start line, rubbing its butt against it. Everyone turns their heads up at the stench.
Roo gets bored eventually, having not noticed what just happened behind it, and rolls its way to the finish line, earning oohs and aahs.
But, still, you won! You didn’t expect your chosen hog-steed to win the race, but you’re all the more better for it. 
~causeEvent(14)
You can hear whoop-whoops as you return to the van. #spr pigrace2
-> END

== loss ==
You've lost. You see the stranger who'd spoken to you earlier grin with glee, extending an open hand to you. Not a handshake, but a <i>pay up</i> sort of gesture. You begrudgingly oblige.
~causeEvent(13)
You get a few shoulder pats as you return to the van, and a haughty grin from Leslie. She would've probably bet on something different. #spr pigpace2
->END


=== betBeebee ===
The race begins! You see Beebee slowly but surely inch its way from the start line, almost crushing Toot in the process, who has barely moved its legs. 
As Toot lets out a toot, Beebee is getting closer to the speedster Roo, but the moment Roo stops, Beebee <i>rams</i> into it, sending the big pig barrelling backwards with its feet in the air. 
You let out a sigh. Roo casually trots its way to the finish line, and Beebee hasn't had a moment to roll around. Unfortunate.

-> loss

=== betToot ===
The race begins! You're waiting for Toot to move just as the other two hogs are but... what is Toot doing?
Toot is dragging its butt in place, spreading a deadly, nose-scrunching stench <i>everywhere.</i> 
While not deadly for the pigs, it's deadly for the betters around, who you can see are growing increasingly paler. You hold your nose with your hand. 
The other two pigs are almost neck-to-neck, up until Beebee barrels into Roo after a sudden stop from the latter. You let out a sigh as Roo crosses the finish line, having licked the equivalent of pennies and all.

-> loss

