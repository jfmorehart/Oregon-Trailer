EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> dumpsterdive

== dumpsterdive ==
#speaker ''
The only reason why you’re stopping by this dumpster is because there was a trash pileup in your van, and Leslie had been complaining about it for the past several minutes.

#enter leslie appearfastright or
#spr leslie you
As you get out of the van, trash bag in hand, you notice that one of the dumpsters is open, the other is closed pretty tight, but there is the corner of what seems to be a magazine stuck under the lid.

#move leslie normal right

#enter lady normal ol
#spr lady lady
<b>Leslie</b>: <color=\#10497c>Hurry uuuuup, the smell from the dumpsters is arguably worse! Ugh!</color> //blue



#move lady normal l
<b>Lady</b>: <color=\#A45F8F>Before you do that, could you please get that magazine for me? It looks different from the ones I have.</color> //pink


//#emote leslie pop
<b>Leslie</b>: <color=\#10497c>Why is that important anyway? Don’t you have a lot of them already?</color> //blue
//#emote lady pop
<b>Lady</b>: <color=\#A45F8F><i>Well,</i> at least I’m asking <i>nicely</i>. You could do that for a change, Leslie.</color> //pink
//#emote leslie pop
<b>Leslie</b>: <color=\#10497c>What I could <i>also</i> do is tell Gator here to <i>please</i> throw you in the <i>dumpster.</i></color> //blue

Gatorhead looks in your direction, shaking his head. He does not want to be involved this time.

* [ Throw Trash In the Open Dumpster ] -> leave
* [ Pull Out Magazine ] -> pullmagazine

== leave ==
You throw the trash in the open container and make your way towards the van, completely disregarding the magazine. 
Lady is already staring you down, with killer intent.

<b>Leslie</b>: <color=\#10497c>You pissed the little one off.</color> //blue
#exit lady exitleft
<b>Lady</b>: <color=\#A45F8F><b><i>You shut up.</i></b></color> //pink
#move leslie normal ol
<b>Leslie</b>: <color=\#10497c>Alright, alright… let’s go, before <i>Lady</i> is the one to throw your body in a dump.</color> //blue

-> END

== pullmagazine ==
Per Lady’s wishes, you pull out the magazine, and luckily it isn’t ripped up already. You wave it at Lady, who gives you a big thumbs up.
But when you turn to, perhaps, <i>finally</i> throw the trash out, as Leslie seems like she’s about to blow up soon, you see that the previously closed lid has opened just a little bit.
On the other hand, the dumpster that's been open this entire time is actually not <i>that</i> dirty. You could see what either of them have to offer-

<b>Leslie</b>: <color=\#10497c><i><b>For the love of god. THROW THE TRASH OUT.</b></i></color>

{ player_constitution > 4:
* [ Open Closed Dumpster ] -> closeddump //fitness/strength skill check, >3
}
* [ Dig Through the Open Dumpster ] -> opendump
* [ Throw Trash Away (Leave) ] -> leaveforreal

== closeddump ==
You manage to take the lid off, and it falls to the ground with a clunk. You don’t need to dig too much through it, thankfully, for you to uncover an old bobble-head figurine. 
It has a baseball cap, but instead of a baseball <i>bat</i>, this one has a gun. You turn to show it to the rest, but Leslie’s furious look makes you hurriedly throw the trash in the dumpster and rush to the car.

<b>Leslie</b>: <color=\#10497c><b>Don’t sit next to me, or I <i>will</i> gut you.</b></color> //blue
<b>Lady</b>: <color=\#A45F8F>I think she means it… but yeah, <i>oof, buddy</i>, you need a shower.</color> //pink
<b>Gatorhead</b>: <color=\#3d7524>Cool find. Put the little guy on the dashboard!</color> //green

<b>[ Item Acquired: Bobblehead ]</b>

-> END

== opendump ==
You are almost halfway into the open dumpster, just barely able to hear Leslie’s horrified gasp from way behind you. You emerge from the pile with a big, shiny gear in your hand. 
Leslie, surprisingly, is kind of stunned at the discovery.

<b>Leslie</b>: <color=\#10497c>Ooooh…. I <i>want it.</i></color>
<b>Lady</b>: <color=\#A45F8F>I thought you were freaking out at them being waist-high in a dumpster?</color>
<b>Leslie</b>: <color=\#10497c>Yeah, but… big wrench.</color>
<b>Lady</b>: <color=\#A45F8F>You are a <i>troubled</i> individual.</color>
<b>Leslie</b>: <color=\#10497c>That's <i>my</i> version of a magazine.</color>

Lady rolls her eyes, but she seems to kind of get it.

<b>[ Item Acquired: Big Wrench ]</b>

-> END

== leaveforreal ==
You throw the trash in the open container and make your way towards the van. 
Leslie sighs in relief, tapping Gatorhead on the shoulder to start the van as soon as you're in. Lady's already going through her magazine.

<b>Leslie</b>: <color=\#10497c>Alright… thank <i>god</i> you don’t smell that fucking rancid! You still need a shower, though don’t we all?</color>
<b>Lady</b>: <color=\#A45F8F>Them more than most...</color>
<b>Leslie</b>: <color=\#10497c>Let’s <i>go nowwww.</i></color>

-> END

