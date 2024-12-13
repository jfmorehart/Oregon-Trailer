EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> log

== log ==
Trees aren't very common on this part of the journey, and especially not lone <i>tree logs</i>. This one in particular has been cast to the side, tied by a rope that's cut on the other end. 

//stage tags!
#enter gatorhead fastappearleft ml
#spr gatorhead gatorhead
#speaker gatorhead
<color=\#3d7524>Yo yo yo! Want me to lift it? Maybe there's shinies underneath.</color>

* [ Let Him ] -> helifts
// * [ Lift It Yourself (Fitness > 4) ] -> youlift
* [ Abandon It ] -> leave

== leave ==

#state gatorhead flip
#exit gatorhead exitleft
<color=\#3d7524>Dang, nevermind then. I'd be hella curious myself...
-> END

== helifts ==
#emote gatorhead pop
#speaker gatorhead
<color=\#3d7524>WOOHOO! I SEE SOMETHING SHINY-

-> discovery

== youlift ==
#spr gatorhead gatorhead_laugh
<color=\#3d7524>Yo, you're strong! Respect. And now... them shinies-

-> discovery

== discovery ==
#spr gatorhead gatorhead
But there are no "shinies." What slowly rolls out of the log as it shifts to one side is-

#spr gatorhead gatorhead_qmark
<color=\#3d7524>Uhhh... What's a grenade doing here?</color>

It's pinned. On its side, there's a worn-out label, with the name scratched out.

<color=\#3d7524>... You want it?</color>

* [ Take It ] -> take
* [ Leave It ] -> escape

== take ==
~ setGrenade(true)
<b>[ Item Acquired: ... Grenade? ]</b>
#spr gatorhead gatorhead
#exit gatorhead exitleft
Gatorhead nods and holds the grenade cautiously. You need to find a place to put this in. You're thinking about the grenade as you head back.

-> conclusion

== escape ==
#exit gatorhead exitleft
Yeah, no. Who knows who left this here. 

-> conclusion

== conclusion ==
Could be a SADsOP guy that did this... because how would a grenade find its way inside a leftover log?

#enter leslie appearright mr
#spr leslie leslie
<color=\#2673B6>Big man, give the grenade to me, you're holding it like a hot potato.</color>
//#enter gatorhead appearleft ml
#spr gatorhead gatorhead_qmark
<color=\#3d7524>Ain't it one?</color>
#exit gatorhead exitleft
#exit leslie exitright

-> END