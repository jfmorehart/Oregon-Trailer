INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

-> capsule

== capsule ==
A cylindral, blue object shines from roadside shrubbery. It looks like a keg, but it clearly is small enough to hold on your lap. Judging by the way it's fallen and  rusted over, it's been here for a while.

* [ Get Up Close ] -> approach
* [ Drive Away ] -> leave

== leave ==
-> END

== approach ==
It's sealed shut, but if you can sneak a piece of paper in between the lid and the body, you could probably loosen it. It would mean losing a paper stamp, though.

* [ Loosen Lid (-1 Currency) ] -> open //maybe u don't need to waste anything if ur strong enough?
* [ Abandon It ] -> abandon

== open ==
<b>[ Resource Lost: -1 Currency ]</b>

After opening the lid, you notice there's a multitude of small objects inside. Mostly colored marbles, old coins, greyed-out photos, along with a small letter.

"--2075-- time capsule-- favorite small things-- memories-- eca Parker."

At the bottom of the letter, there is a crude map, luckily not wiped away by time. "Go West, past Diner." Chances are you might hit upon this object on the road.

-> END

== abandon ==
If you can't open it with your own hands, why bother opening it at all? There must be a reason why it's sealed so tight. You leave the cylinder alone and return to the van.

-> END