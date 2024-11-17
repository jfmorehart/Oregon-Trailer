INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

-> wolfmeet

== wolfmeet ==
As you approach what seems to be a broken section of the highway guard rails, a stranger comes into view, in a scuffed wolf-like fursuit, sitting in the empty space. The stranger waves weakly towards the van.


* [ Approach ] -> approach
* [ Leave ] -> drive

== drive ==

-> END

== approach ==
Upon a closer look, the wolf seems to be breathing heavily. Their suit is muddied in some places, and has lost color in others. You can make out only one word in between all their heaving, ‘water.’

* [ Give Water (-5 Rations) ] -> givewater
* [ Drive Away ] -> nowater

== givewater ==
<b>[ Resource Lost: - 5 Rations ]</b>

The wolf turns away from your party, and you can hear them chugging the water. The sigh at the end was one of contentment. They turn around, fixing their headpiece.

<b>‘Wolf’</b>: <color=\#6082B6>Thank you…………… Oh my gosh, that water was all I needed. Thank you, thank you. My goodness.</color>

<b>‘Wolf’</b>: <color=\#6082B6>I’ve been out here for a good few days now….. just thinking about it makes me nauseous.</color>

* [ Ask About Situation ] -> exposition
* [ Leave ] -> noexpo

== exposition ==
<b>‘Wolf’</b>: <color=\#6082B6>I worked at a local gas station as a mascot. The SADsOP boys manage that particular area. But…. I spilled a glass of Coke on one of their <i>Bigs</i> or whatever they’re called and they stranded me here… no water…</color>

<b>‘Wolf’</b>: <color=\#6082B6>Hold on, though… I still have this on me.</color>

<b>[ Item Received: Coupon. 20% off any beverages at the next available gas station. ]</b>

<b>‘Wolf’</b>: <color=\#6082B6>Hope this helps. It won’t be much use for me until I get out of here but… tell Doug I say hello.</color>

-> END

== nowater ==
Out of your rearview mirror, you can see the man slump forward, hands outstretched towards the van. And then, a pretty haunting,

<b>‘Wolf’</b>: <b><i><color=\#6082B6>NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO</color></i></b>

-> END

== noexpo ==
<b>‘Wolf’</b>: <color=\#6082B6>Wait, but- Hold on!-</color>

-> END
