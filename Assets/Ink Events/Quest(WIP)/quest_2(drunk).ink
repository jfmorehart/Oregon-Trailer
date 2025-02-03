EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> drunk

== drunk ==
There's some guy sitting by the edge of the highway, arms crossed to his chest. There's a couple cans of beer laying beside him. They're seemingly empty. He however, seems to be unconscious.

#speaker drunk
<color=\#520A0A>ZZZZZZZZZ</color>

* [ Approach ] -> approach
* [ Keep Driving ] -> leave

== leave ==
-> END

== approach ==
Small crop top, big pants, accessories that you'd only see on a gun-toting frat boy...

#enter gatorhead appearleft ml
#spr gatorhead gatorhead
#speaker gatorhead
<color=\#3d7524>SADsOP.</color>

#speaker drunk
<color=\#520A0A><i>ZZZZZZZZZZZZZ</i> (He seems to be snoring louder?)</color>

He shuffles in his sleep, knocking one of the cans over. You can see the edge of a laminated, minuscule stamp peek out from the can's top.

#speaker gatorhead
<color=\#3d7524>... Steal 'em.</color>

#speaker drunk
<color=\#520A0A><i>ZZZZZZZZZZZZZZZZZZZ</i></color>

#state gatorhead flip
#state gatorhead flip
#speaker gatorhead
<color=\#3d7524>The guy <i>is</i> asleep, anyway.</color>

* [ Steal Stamps ] -> steal
* [ Try to Wake Up ] -> wake
* [ Leave ] -> nosteal

== steal ==
~causeEvent(15)
<b>[ Resource Gained: +5 Currency ]</b>

#spr gatorhead gatorhead_laugh
#state gatorhead bounce
#speaker gatorhead
<color=\#3d7524><i>Score. Hel-</i></color>

Shuffling back in your original position, you almost knock a beer can over yourself. The man shuffles in his sleep, turning to his side. 
Under him, you notice a slip of paper. One of its corners is caught on his backside.

#speaker gatorhead
<color=\#3d7524>C'mooon... we don't need to risk our skin for a piece of paper...</color>

* [ Take Paper ] -> take
* [ Leave ] -> nopaper


== wake ==
The drunk man takes a while to wake up, waving a hand in front of their face as if you're a fly.

#speaker drunk
<color=\#520A0A>Ughhhghhghgh... just leave.... alone....</color>

#speaker gatorhead
<color=\#3d7524>What are you doing, man...</color>

* [ Steal Stamps ] -> steal
* [ Leave ] -> escape

== take ==
You carefully try to take the piece of paper away from the sleeping drunk. It is a flyer for a "Grenade Football," held at "Dalton's" every "SAD-DUR-DAY."
"BRING YOU'RE OWN GRENADE OR U CANT JOIN!!!"

{ 
    - has_grenade == true:
    -> extraline
    - has_grenade == false:
    -> continuous
}
== extraline ==
#speaker gatorhead
<color=\#3d7524>Oh shit! That grenade we got earlier? Place your bets, <i>I</i> think it's the same guys.</color>
-> continuous

== continuous ==
However, you can't read for much longer, as you see the stranger is about to wake up - and he does, startled.

#speaker drunk
<color=\#520A0A>Ugh... huh.... hey. <i>HEY!</i></color>

He groggily aims a beer can at you as you and Gatorhead run away. The paper flies away in the wind.

-> END

== nopaper ==
It's not worth stealing <i>further</i> from this man. You got the stamps, and that's enough.

-> escape

== nosteal ==
It's not worth stealing from this man. Plus, he's drunk and stranded on the highway. SADsOP or not, how will he get back without at least <i>some</i> stamps?

#speaker gatorhead
<color=\#3d7524>You're no fun. We could've used those....</color>
-> escape

== escape ==
As you walk away, you hear the man mumbling in his sleep. // need to figure out what

-> END