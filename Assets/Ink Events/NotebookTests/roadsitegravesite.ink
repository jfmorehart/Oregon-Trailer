INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

-> grave

== grave ==
Small, makeshift graves by the side of the highway are not uncommon in this day and age. They’re one of the few things that have not died to the shift in apathy, ironically.
Wilted flowers, weeds and crumpled stamps lie in broken sections of the highway railing, alongside slabs of stone. The photos are what distinguish them as graves: some of them brown and torn, some of them somewhat new. 

The one gravesite that manages to get your attention, happens to be one of the more well-maintained, appearing as a blue dot in the corner of your vision.

* [ Stop ] -> stop
* [ Drive Away ] -> leave

== leave ==

-> END

== stop ==
The ‘blue dot’ was a small bouquet of bright blue flowers right next to a slightly crumpled crayon drawing of stick figures. 
The paper has been yellowed out, making the drawing look almost ancient, but the flowers are fresh. There is an elastic band around the stems.

* [ Examine Drawing ] -> drawing
* [ Examine Flowers ] -> flowers

== flowers ==
The flowers are fresh, but fragile. As you inspect their inner petals and the band that ties them together, one of the larger buds tilts backwards, threatening to cut away from its stem. Smaller petals already start to fall from the poor blue bonnet.

* [ Cut Bud Off ] -> keep
* [ Put Flowers Back ] -> interlude

== keep ==
You snap the bud from its stem, letting it fall back on your hand. It's a very nice color.

You hold it with two hands, as it's very fragile, and with that, make your way to the van. Though it might wilt soon enough, its temporary smell is a nice reprieve from the road.

-> END

== drawing ==
There are names on each stick figure, ranging from smallest to largest, with the final big stick figure being called “granpa Teddy,” in childlike, crayon writing.

As you’re about to put the drawing back in its spot, you notice that it had been hiding a photo this entire time. 

* [ Examine Photo ] -> photo
* [ Put Drawing Back ] -> interlude

== photo ==
It is kind of blurry and yellowed out, but you can make out the image of a man.
There’s a note on the back. “Thanks for the good years, Teddy. I still love you, and I’m sorry. - Berta”

You don’t know anyone by that name, but you think this “Berta” might be the one who has been caring for this place. 
[ It reminds you of someone. ]

-> leave

== interlude ==
This feels too personal to intrude on. Maybe it isn't your place to examine other people's lives. You already shifted things around.

After dusting your knees and putting the photo back in its place, you get up and go back in the van.

-> END