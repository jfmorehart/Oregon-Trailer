EXTERNAL causeEvent(ID)
INCLUDE  ../globals.ink

-> exhibit

== exhibit ==
//this is more of a fun, meta event
It isn't every day that you come across giant bananas. No, really.
The giant bananas, even from this distance, look so real that it's confusing. You can see the creases on these artificial fruits as if they've been tossed around. They're a pale greenish yellow, like <i>actual</i> bananas.

* [ Come Closer ] -> observe
* [ Drive Away ] -> leave

== leave ==

-> END

== observe ==
There's a man standing in a broken section of the highway railing, holding a book. He looks silly, but his moustache is... incredible.

<b>Morteemurr</b>: <color=\#8E1600>Ahhh, welcome! To the Moo-sa Acu-mee-natah exhibit!</color>

Before you can even get a word in, he starts to speak, gesturing wildly towards the bananas.

<b>Morteemurr</b>: <color=\#8E1600>You see, the Moo-sa Acu-mee-natah used to be fruits our ancestors consoomed on the daily!</color>
<color=\#8E1600>Yes, yes of course! A very famed artist from <i>years</i> ago went on a perilous journey to acquire such avantgarde imagery for us to peroose upon in our day and age, and they <i>remain to this day</i>!</color> 
<color=\#8E1600>Hm! Isn't that <i>something?</i> Just take a look at the-</color>

* [ Interrupt ] -> attempt
* [ Leave ] -> escape

== escape ==
You hastily walk the other way while the man is speaking. By the time he's stopped talking for good, you're long gone.

-> END

== attempt ==
<b>Morteemurr</b>: <color=\#8E1600>-the colors, my the colors! You'd never have thought that they could convey such <i>softness</i> such textuure-</color> 

<i>He just won't stop.</i>

* [ Try Again ] -> anotherattempt
* [ Yell ] -> yell
* [ Leave ] -> escape

== anotherattempt ==
<b>Morteemurr</b>: <color=\#8E1600>It is <i>simply</i> incredible! Words can't describe the weight that this exhibit has on me! Ever since my wife-</color>

* [ Zone Out ] -> finalattempt
* [ Yell ] -> yell
* [ Leave ] -> escape

== finalattempt ==
<b>Morteemurr</b>: <color=\#8E1600>-And that is why I'd definitely use this famed 'ba-na-na' as an example for why we need a 'Health Renaissance.' Maybe those cultists were onto something! Hm! Anyway.</color>

* [ Escape While He's Distracted? ] -> escape
* [ Maybe This Time... ] -> listenin

== listenin ==

<color=\#8E1600>My name is Mr. Morteemurr. But you can call me... Mr. Morteemurr.</color>
<color=\#8E1600>We've made our ack-wein-tance now! I'll find you again at one of these <i>wondrous</i> exhibit-shuns... Don't worry....</color>

He's trailing off, already entranced by the huge fruits. You pace slowly backwards before rushing to the car and driving off.

Does anyone else even care about bananas this much?

-> END

== yell ==
<b>Morteemurr</b>: <color=\#8E1600>What... did you say?</color>

Before you can say anything, he throws his book at you. yelling something about how he's "spreading knowledge for free" and you're "unappre-shee-ayting." You leave before he can hit you again. 

-> END



