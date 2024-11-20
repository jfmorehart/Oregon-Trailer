INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

-> mascot


== mascot ==
As you approach what seems to be a broken section of the highway guard rails, a blur of blue comes into view, sitting in the empty space. 

Eventually, you realize that the blur isn’t a something, but a <i>someone,</i> in a scuffed wolf-like fursuit. Their nametag is about to fall off their chest. The stranger waves weakly towards the van.

<b>Leslie:</b> <i>Ain’t no way</i>… Gator, do you have a brother?
<b>Gatorhead:</b> Har har. Real funny. My brother ain’t a dog.
<b>Leslie:</b> So close! That's not even a dog. Still, I’m curious now… let’s see what this guy wants.

Upon a closer look, the <i>wolf</i> seems to be breathing heavily. Their suit is muddied in some places, and has lost color in others. You can make out only one word in between all their heaving, ‘water.’

<b>Leslie:</b> Oh, their suit looks a bit… <i>eugh.</i>
<b>Gatorhead:</b> That don’t matter. Didn’t they say they wanted water?
<b>Leslie:</b> …Man, you can practically see their tongue lolling out.
<b>Gatorhead:</b> Well, let’s give it to them!
<b>Leslie:</b> I don’t know about this one, big guy… What if they have a disease? Plus they need water alone to wash that suit… that’s a lot of water.
<b>Gatorhead:</b> Gimme the damn bottle, Les! Not letting my man turn into roadkill over here.

* Give Water (Gatorhead) -> water
* Drive Away (Leslie) -> drive

== water ==
~causeEvent(4)
~causeEvent(15)
The <i>wolf</i> turns away from your party, and you can hear them chugging the water. The sigh at the end was one of contentment. They turn around, fixing their headpiece.

<b>‘Wolf’:</b> Thank you…………… Oh my god, that water was all I needed. Thank you, thank you. Oh, god. I'm saved.
<b>Leslie:</b> <i>Wow!</i> You just turned a brother religious, Gator.
<b>Gatorhead:</b> Yer welcome, doggy-dog-man.
<b>‘Wolf’:</b> Er… anyways…. Here's all I have on me.

<b>[ + 15 Currency! For being an upstanding citizen. ]</b>

<b>‘Wolf’:</b> Hope this helps. Oh... and if you ever see someone named Doug... please tell him I say hello.
<b>Gatorhead:</b> Doug… Dog… there’s another one…
<b>Leslie:</b> Ugh, you’re sounding dehydrated yourself. Let’s <i>go.</i>

-> END

== drive ==
You decide to floor it. Out of your rearview mirror, you can see the wolf-man slump forward, hands outstretched towards the van. And then, a pretty haunting,

<b>‘Wolf’:</b> <i><b>NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO</i></b>

<b>Gatorhead:</b> Yeah, see? Roadkill.
<b>Leslie:</b> Uhhhhh….. Well, I stand by it! Besides, listen to him, that ain’t even a <i>good</i> wolf howl!
<b>Gatorhead:</b> Oh, <i>fuck off,</i> Les.
<b>Lady:</b> … You guys are idiots.
-> END
