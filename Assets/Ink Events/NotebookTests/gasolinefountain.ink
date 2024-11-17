INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

-> fountain

== fountain ==
You have slowed down next to what seems to be a fountain, spurting black liquid - oil, you presume - all over the place, at irregular frequencies. They form black puddles on the ground, mixed with dead grass.

* [ Check It Out ] -> check
* [ Leave ] -> leave

== check ==
Once out of the car, a raggedy old man approaches you quickly, already yelling something about ‘5 bucks for the attraction.’ 

Attraction? You just wanted some oil, damn it. What even is this oil-covered scammer yelling in your face doing, demanding money?

* [ Pay Up ] -> pay
* [ Push Past ] -> ignore
* [ Leave ] -> escape

== pay ==
<b>[ Resource Lost: - 5 Currency ]</b>
~causeEvent(12)
After giving you a gruff thank you, he explains that this particular fountain used to be an actual clearwater fountain before some ‘hippie artist’ filled it with motor oil. The point was to show how reliant the people had grown on oil, but it instead limited hitchhikers to clean water.

Sure, there were a couple deaths here and there, too. Poor people were drinking the oil.

The artist was not seen in the area for a great while after that. The old man leans in, and insists that the ‘hippie’ was onto nothing, anyway. 

When he leans away, and you are back in the van, you note that the man’s breath smelled like oil.

-> leave

== ignore ==
~causeEvent(5)
The fountain is still spitting oil, either a little or a lot, but always into its own basin, almost never out of it. The man is trailing after you, shaking his fist at your insolence. 
Before he can even try to forcefully remove you from his ‘attraction,’ the fountain spits motor oil in your eye. Tough luck.

-> escape


== escape ==

The old man starts laughing, a nasty sound, as you rush towards the van drive off. You can still hear the echo of his jeering.

-> END

== leave ==

-> END
