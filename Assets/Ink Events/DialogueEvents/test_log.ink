EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> log

== log ==
//imagine a more forested area, lol
Trees aren't very common on this part of the journey, <i>tree logs</i>. This log has been cast to the side, tied by a rope that's cut on the other end. 

<b>Gatorhead</b>: Want me to lift it? Maybe there's shinies underneath.

* [ Let Him ] -> helifts
* [ Lift It Yourself (Fitness > 4) ] -> youlift
* [ Abandon It ] -> leave

== leave ==
<b>Gatorhead</b>: Dang, nevermind then. I'd be hella curious myself...
-> END

== helifts ==
<b>Gatorhead</b>: WOOHOO! SHINIES-

-> discovery

== youlift ==
<b>Gatorhead</b>: Yo, you're strong! Respect. And now... them shinies-

-> discovery

== discovery ==
But there are no "shinies." Indeed, what slowly rolls out of the log as it shifts to one side is-

<b>Gatorhead</b>: ... What's a grenade doing here?

It's pinned.

<b>Gatorhead</b>: ... You want it?

* [ Take It ] -> take
* [ Leave It ] -> escape

== take ==
<b>[ Item Acquired: ... Grenade? ]</b>
Gatorhead nods and holds the grenade. You need to find a place to put this in. You're thinking about it as you head back.

-> END

== escape ==
Yeah, no. Who knows who left this here. Could be a SADsOP guy... because how would a grenade find its way inside a leftover log?

-> END