EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> drunk

== drunk ==
There's some guy sitting by the edge of the highway, arms crossed to his chest. There's a couple cans of beer laying beside him. They're seemingly empty. He however, seems to be unconscious.

<b>Drunk</b>: ZZZZZZZZZ

* [ Approach ] -> approach
* [ Keep Driving ] -> leave

== leave ==
-> END

== approach ==
Small crop top, big pants, accessories that you'd only see on a gun-toting frat boy...

<b>Gatorhead</b>: SADsOP.

<b>Drunk</b>: <i>ZZZZZZZZZZZZZ</i> (He seems to be snoring louder?)

He shuffles in his sleep, knocking one of the cans over. You can see the edge of a laminated, minuscule stamp peek out from the can's top.

<b>Gatorhead</b>: ... Steal 'em.

<b>Drunk</b>: <i>ZZZZZZZZZZZZZZZZZZZ</i>

<b>Gatorhead</b>: The guy <i>is</i> asleep.

* [ Steal Stamps (++ Gatorhead) ] -> steal
* [ Wake Up ] -> wake
* [ Leave ] -> escape

== steal ==
<b>[ Resource Gained: +5 Currency ]</b>

<b>Gatorhead</b>: <i>Score. Hel-</i>

Moving the beer cans around did make a sound though, and you see as the man's about to wake up - and he does, startled.

<b>Drunk</b>: Ugh... huh.... hey. <i>HEY!</i>

He aims a beer can at you as you run away.

-> END

== wake ==
The drunk man takes a while to wake up, waving a hand in front of their face as if you're a fly.

<b>Drunk</b>: Ughhhghhghgh... just leave.... alone.... too warm....

* [ Steal Stamps (++ Gatorhead) ] -> steal
* [ Leave ] -> escape

== escape ==
It's not worth stealing from this man. Plus, he's drunk and stranded on the highway. SADsOP or not, how will he get back without at least <i>some</i> stamps?

<b>Gatorhead</b>: You're no fun. We might've gotten him to wake up for real.

As you walk away, you hear the man mumbling in his sleep. // need to figure out what


-> END