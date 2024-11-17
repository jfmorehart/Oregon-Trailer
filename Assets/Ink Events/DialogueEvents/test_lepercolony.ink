EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> church

== church ==
In the South, it’s not uncommon to see churches, at this point half-destroyed, not far off from the side of the highway. Usually, they are abandoned, but this one isn’t: there is smoke ascending from somewhere inside. 

* [ Approach ] -> approach
* [ Drive Off ] -> leave

== leave ==

-> END

== approach == 
The church is half-eroded; the roofing is almost certainly gone. The only thing to indicate that it’s a church is a left-leaning cross at the entrance. Inside, the rather unhappy - to be run into - campers are a small colony of lepers, sitting cautiously around a small fire. //visual rep?

You can see varying stages of leprosy all over each person. One of them doesn’t have an ear, another one still has both their ears but part of their face has started to rot, a reddish-brown hue.

You’ve been told about old depictions of leprosy but… is there even a vaccine for this thing? What if there isn’t and you’re doomed? What if they get dangerous? What if, what if…

* [ Give Food ] -> give
* [ Leave ] -> escape
* [ Steal Food ] -> steal

== give ==
~causeEvent(4)
<b>[ Resources Lost: - 7 Rations ]</b> (tbd)
// reputation gained with van as a whole?

Giving the food does make you feel better, even if the food was lost. The leper ‘colony’ is grateful, even if they’re still a bit standoffish towards you.
Returning to the van, the party seems to be in better spirits, feeling good for having done a good deed.

-> END

== steal ==
<b> [ Resources Acquired: + 7 Rations ]</b> (tbd)

In a spur of the moment decision, you grab the food cans next to the lepers’ fire and hurry your way out. The lepers start mumbling among themselves distressingly, a couple of them even standing up to follow after you, but you’re gone before they can muster a couple steps. 

Returning to the van, the others comment on how horrible that was. You should definitely not take part in a charity initiative.

-> END

== escape ==
You turn around to leave. The sight seems to unsettle the rest of your party. Returning to the van, the others are mostly silent. You drive away and leave the church and the unfortunate colony behind.

-> END
