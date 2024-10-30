EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> lostpledges

== lostpledges ==

You drive has been oddly uneventful for a while now. And almost as if someone'd heard you voice that thought out loud, you spot two figures by the side of the road playing catch with one another. 

Getting closer, you hear faint laughter, when suddenly the football flies right above your windshield, even scraping the side of whatever's at the top of your van. Your party members nearly jolt awake.

Right after that, one of the figures rushes onto the highway. You're about to hit the brakes when the stranger, instead of advancing any further in the direction of the well-identified flying object, passes out heaving right in front of the car. 
The other stranger is soon sprinting over - of course after going to get the football, because, <i>priorities</i> - kneeling and breathing heavily over the fainted.

"Bro... Bro wake up! <i>Bro..........</i>"

* [ Help Out ] -> help
* [ Stay Put ] -> stay

== help ==
Partially because of someone passing out <i>right there</i> - and <i>obviously</i> you don't want to drive them over - and partially because of your companions' sudden interest in this near-death scenario that had just unfolded, you step out of the car.

"Right here!" The stranger yells, even though you're not that far from him. "Please help him. I don't know why he passed out...." He turns to the fainted then, dejected. "Bro..."

You look back to your party members. Lady is shaking her head. Gatorhead on the other hand is trying to act out what to do, taking Leslie's shoulders and pretending he's slapping her, but theatrically. Leslie is grinning the entire time.

<i>Slap him? Is that what he's suggesting?</i>

{ player_constitution > 3:

-> awake 

- else:

-> uhoh 
}

== awake ==
~ player_charisma ++
You crack your knuckles, and ignore the guy as he asks what you're about to do. You kneel against the fainted guy and ready your hand. You don't know it, but Gatorhead and Leslie are cheering, for whatever reason.
<b><i>SLAP!!</b></i> It even echoes. You're awfully proud of that slap.

The guy wakes up, startled.
"WOAH."
"Bro!" The other one exclaims. "You passed out!"
"The heat, bro... It got me..."
"It got got you. For real."

What even is happening? You step back as they get up. You finally note their clothing. Cropped jerseys with huge numbers at the front, short shorts... These are SADsOP members. But judging by the lack of any other signifier, they could be pledges. Or wannabes really.

The one - who has just awoken - salutes you.
"Thanks, dude. For not driving over us and stuff."
The other one acts similarly. "Yeah. And for not driving over the football. Sorry."
"Yah, sorry."
-> exposition

== exposition ==
"We <i>just</i> came back from an all-nighter. It was <i>fuuuuucking</i> crazy, dude."
"Yah dude."
"One of the Bigs held a massive party last night and it carried on all night. Lotsa cool people."
"Lots."
-> nice

== nice ==
"For helping us out, we'll put in a good word for you. For <i>sure.</i>"
Instead of continuing the chain of conversation like he had been, the one who'd been speaking in one or two word sentences shook his hand in the air. What is that, code for something? You shrug.

You go back to the van, and you brief your teammates. Lady doesn't seem exactly thrilled, but Gatorhead <i>is</i>. Leslie's Leslie.
-> END

== uhoh ==
~ player_moxie --
You ready your hands and fists, and as much as the guy can yell at you asking what you're doing, he cannot stop what's coming.
You ready for a good slap square on the cheek.
But somehow, you miss. Horribly. And you slap the other guy. Centrifugal force wasn't working with you here.
The guy is <i>pissed.</i> He punches you right back, square on the nose. You fall back, and that's all Gatorhead needs to run outside and drag you out of there. Swears and yells are exchanged, which in turn wake the fainted up. He starts yelling incoherent stuff soon after, but it's well into getaway drive that you hear a <b>"FUCKING IDIOTS! WE'LL TELL OUR BIGS FOR THIS! YOU'LL PAAAA A A A A A A Y-"</b>
-> END

== stay ==
The guy's still yelling, but you're not moving a muscle. Eventually, he gets irritated enough to come all the way to your van, banging on the window repeatedly. Lady stares you down, really trying to get you not to help them, for whatever reason. And you might oblige, before Leslie loudly whispers that cracks in the glass are starting to show.

* [ Don't Open Window ] -> noglass
* [ Open Window ] -> window

== noglass ==
Leslie eventually ends up being right even though she didn't want to be. The glass eventually starts to crack, and at that moment you whip your head around to face the guy, who has also stopped. He grins nonchalantly.

"Ooops."
You open the door just a slit.
"... Please help us out."
But now that he's cracked the glass, you <i>really</i> don't want to help him. And he probably recognizes that.
He slips a couple of stamps through the slit in the door. "Here. Compensayshun."

You close the door shut after taking the stamps, and drive off. Where will you even fix the window with this 'compensayshun?'
-> END

== window ==
You roll the window open before he can seriously damage it. The man's back to whining for help, somehow louder even without the window pane being an obstacle. He rushes back to his friend soon after.

-> help