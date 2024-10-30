INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

-> birdcargo

== birdcargo ==


Finding a warehouse by the side of the highway is kind of like a jackpot. Your party also agrees. Who knows what you could find in there! 

So, you all go out to investigate, after having Gatorhead break down the door, of course. You are all met with the sight of a bunch of crates, some stacked on top of one another and some scattered across the floor. 
There’s a crate immediately to your left that’s half opened. As you’re about to head for it, Leslie bumps against a tower of crates, which sends one of them spilling over right in front of you.

* [ Look at Crate ] ->left
* [ Look at Spillage ] ->right

== left ==
You dig your hand around, and what you find among the package filling is a triangular, metallic object. 
It has a few small nuts and bolts, an opening that looks like a mouth, but nothing other than that. Is this scrap?

-> group

== right ==
You crouch down next to the spillage, and start to look for anything particularly shiny and interesting among more regular-looking trinkets. 
Eventually, what you find is a flat, misshapen metallic object. It has a few dents, some nuts and bolts, and a hook that would probably connect this object to another. 
There’s another one just like it at the bottom of the crate. Nothing else other than that. Is this scrap?

-> group

== group ==
At the same time, your party members are also questioning their findings.
Apparently, Leslie found something that looked like a small, spherical metal head. Lady found two thinner, stick-like pieces of metal. 
Gatorhead found a bigger version of what Leslie described, but instead with two hooks at the sides, and a few holes.
You come together, and the pieces reveal… a bird?
You can hear Gatorhead swear under his breath.

It seems that bird parts are kept in this warehouse. There isn’t anything else to be understood from this.

The others are already suggesting to go back to the van, as Gatorhead looks - somehow - increasingly more disturbed. Something something, <i>‘birds aren’t real?</i>

* [ Stay and Search Further ] -> search
* [ Go Back ] -> leave

== search ==
//card_obtained variable?
You decide to stay and look some more through the crates. The others half-heartedly join the search.
Eventually, you pull out a flat, rectangular object from a crate in the back of the room. It reads: <b>B1RDM4N_0956…</b> and a series of numbers. The chip on the back tells you that it’s readable.

<b> [ Item Received: Keycard. Provides access to a secret location. ] </b>

{ has_keycard == false:

~ has_keycard = true
}

-> leave

== leave ==
You decide to go back to the van. Gatorhead is still mumbling to himself. 
You can’t help but feel bad that you exposed an animal lover (of sorts) to this kind of cold truth, but Leslie says he’ll be alright once he has a beer or something. Lady’s nod indicates doubt in that sentiment.
-> END
