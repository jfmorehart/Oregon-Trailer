EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> birdfactory

== birdfactory ==
A big, dusty structure of steel casts a shadow over your van as you drive past it. 
Just the idea of such a building being not far from the edge of the highway, where you can safely park, is enticing enough to get your party members almost running out of the van. 
The building’s door, however, is locked. There is a card reader next to it, with a red light blinking from its center.

* [ Give Up ] -> leave
* [ Force Your Way Through It ] -> bestrong
* [ Use Cardreader ] -> keycard

== leave ==
Try as you may, you cannot, for the life of you, get any kind of look inside the building. 
The windows are so incredibly dirty, in disrepair even, that the inside remains a stranger to you. 
You drag your way to the van, throwing frustrated glances back at the structure, which looms over you like a weird omen.
-> END

== keycard ==
You don't have access to the card reader, silly! No such item is in your inventory :)

-> END

== bestrong ==
//skill check for strength/fitness here
You give the big door a good up and down, and you conclude that you can open it. 
The others are skeptical, probably because they haven’t seen the kind of fit god you’ve become, but who cares! Time to show them! 
You pace backwards before ramming into the door full force. It budges a little, and it takes a few more times for it to fall down. Your teammates are impressed, but soon start coughing from the dust buildup coming from inside.

-> inside

== inside ==
The card reader at the front, the steel beams, the machinery that you can already make out from the entrance… This is a factory. And based on the multiple mechanical birds lying around, you can tell what’s being manufactured.
Surrounded by the tin corpses of avians, you and your party are now blessed (or cursed?) with the knowledge that birds, indeed, are not real.
Gatorhead, for one, is breathing so heavily to the point where it feels like he’s going to faint. Lady throws concerned glances in his direction.
Leslie, on the other hand, having soundlessly split from the team, comes back with a box of wrenches, other tools and small parts. Her smile is mischievous.
You can, at least, make use of these abandoned bird parts. Can’t have them lying around, serving no one, right?

<b> [ Parts Acquired (Van). You can now build a giant bird head on top of your van. ]
[ New Dialogue Options for Bird Conspiracy ] </b>


-> END
