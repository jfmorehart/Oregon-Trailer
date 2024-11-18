EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> hitchhiker

== hitchhiker ==
A hitchhiker holds their thumb out to any car that might pass them by and help them out. He's expectantly looking towards your van.

* [ Slow Down ] -> slowdown
* [ Keep Driving ] -> leave

== leave ==
-> END

== slowdown ==
<b>Hitchiker:</b> You got 5 stamps? Everyone who slows down asks for 5 stamps exactly and I don't have any!... I can't get anywhere either.

<b>Hitchiker:</b> I can see your van is full, so I won't ask to be let in... But please just spare a few stamps.

* [ Give 5 Stamps (-5 Currency) ] -> give
* [ Refuse ] -> refuse

== give ==
<b>[ Resource Lost: -5 Currency ]</b>

<b>Hitchiker:</b> Oh, dang, thanks so much! I'll definitely get back to Arizona at this rate. Here, take this.

<b>[ Item Gained: Arizona Postcard ]</b>

<b>Hitchiker:</b> Now that it's very likely I'm going home, I won't need this to stare at anymore. Take care!

-> END

== refuse ==
<b>Hitchiker:</b> Ah... that's too bad. Hopefully the next person over can spare some. But I've already asked the last five cars... Ah, well. Take care.

-> END