EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> church

== church ==

In the South, it’s not uncommon to see churches, at this point half-destroyed, not far off from the side of the highway. Usually, they are abandoned, but this one isn’t: there is smoke ascending from somewhere inside.
you are skeptical, but Lady’s way too chipper about it. You don’t even think that she’s religious, probably just morbidly curious.
As you approach it, you note that the church is half-eroded. The roofing is almost certainly gone. The only thing to indicate that it’s a church is a left-leaning cross at the entrance.
Lady rushes in before anyone else, and is the first to encounter whoever is camping there.

<b>Lady</b>: … <i>Oh my.</i> //pink
 

Everyone else follows suit, and they have similar reactions. The rather unhappy - to be run into - campers are a small colony of lepers, sitting cautiously around a small fire. Some of them are sitting on the pews, some of them on the ground.
You can see varying stages of leprosy all over each person. One of them doesn’t have an ear, another one still has both their ears but part of their face has started to rot, a reddish-brown hue. There’s also coughing.

<b>Lady</b>: Oh, we <i>have</i> to help them out. Look at them… having to find shelter in the ruins… //pink
<b>Leslie</b>: Yeah, this is… this is tough luck. I do not envy the poor fuckers. //blue
<b>Gatorhead</b>: Mhm. That’s bad. //green
<b>Lady</b>: I vote to give them food, I’m sure we have enough! It would also make us feel better... We get to actually <i>help</i> people! //pink
<b>Leslie</b>: … <i>Fake bitch.</i> //blue

Still, you can’t help but think. You don’t know what leprosy entails.
You’ve been told about old depictions of leprosy but… is there even a vaccine for this thing? What if there isn’t and you’re doomed? What if they get dangerous? <i>What if, what if…</i>

* [ Give Food (<b>++ with Lady</b>) ] -> food
* [ Leave ] -> nofoodforthem
* [ Steal Food ] -> foodforyou

== food ==
~causeEvent(4)
Giving the food does make you feel better, even if the food was lost. The leper ‘colony’ is grateful, even if they’re still a bit standoffish towards you. They don’t come any closer, but they bow their heads forward.
Your party does the same, with Gatorhead saluting instead. The gator cannot curtsy.
Returning to the van, the party seems to be in better spirits, feeling good for having done a good deed. Suppose Lady was right.

-> END

== nofoodforthem ==
//rationality ++
You turn around to leave, refusing to give the lepers any of your food. You can hear Lady apologizing to the lepers, and the lepers themselves are nodding solemnly. You did not want to help them, and they are not surprised.
The sight seems to unsettle the rest of your party. Returning to the van, the others are mostly silent, and Lady says that it’s sad that you didn’t want to contribute to people’s livelihood in a positive way.
Leslie and Gatorhead exchange glances, and boot up the van, leaving the church behind.

-> END

== foodforyou ==
~causeEvent(6)
//spirituality --
You glance to a pile near the campfire, eyeing the food cans stacked on top of it. In a spur of the moment decision, you grab the food cans and hurry your way out.
Your party is speechless, and so is the leper colony. The lepers start mumbling among themselves, shooting your friends angry glares, a couple of them even standing up to follow after you, but you’re gone before they can muster a couple steps.
Returning to the van, the others comment on how fucked up that was, and that you definitely shouldn’t be part of any charity initiative. Lady even said that you were probably on the naughty list.
You can only hope this hasn't changed their opinion of you forever, because the rest of the ride is absolutely silent.

-> END



