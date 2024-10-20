EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> gasolinefountain

== gasolinefountain ==
There’s a fountain by the side of the road, and you decide to slow down to look at it better. It’s no clear-watered oasis, however, as it’s spurting black liquid - oil, you presume - all over the place, at irregular frequencies.
You’d think that it’s cheaper than a gas station, but the moment you step out of the car to investigate, an old man approaches you, already yelling something about ‘5 bucks for the attraction.’
<i>Attraction?</i> You just wanted some oil, damn it. What even is this oil-covered scammer yelling in your face doing, demanding money? You exchange glances with your van party, who simply shrug.

* [Give Money] -> money
* [Push Through] -> oilineye

== money ==
~causeEvent(12)

You give the old man some money. He smiles, and you notice that many of his teeth are blackened, but you don’t wanna ask why.
After giving you a gruff thank you, he explains that this particular fountain used to be an actual clearwater fountain before some ‘hippie artist’ filled it with motor oil. The point was to show how reliant the people had grown on oil, but it instead limited hitchhikers to clean water. 
Sure, there were a couple deaths here and there, too. The artist was not seen in the area for a great while after that.
The old man then leans in, and insists that the ‘hippie’ was onto nothing, anyway. When he leans away, and you are back in the van, you note that the man’s breath smelled like oil, too.
-> END

== oilineye ==
~causeEvent(5)
You push past the man and towards the fountain. It’s still spitting oil, either a little or a lot, but always into its own basin, almost never out of it. The man is trailing after you, shaking his fist at your insolence.
Before he can even try to forcefully remove you from his ‘attraction,’ the fountain has the final say: a bit of motor oil gets into your eye. Tough luck.
As the old man starts laughing, a nasty sound, you immediately start rushing towards the van and you drive off. Lady is the one to hold your head while Leslie runs cold water over your eye. Gatorhead cannot stop laughing either.

-> END