EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> argument

== argument ==
<b>Stranger 1:</b> Shut it, Daisy! You know very well that <i>I</i> deserved to be Dalton's girl!
<b>Stranger 2:</b> You're <i>delusional!</i> You <i>know</i> that he didn't like you like that! It all started when you gave him that super salty lemonade...
<b>Stranger 1:</b> <i>I DIDN'T PUT ANY SALT IN IT, IT'S LEMONADE!</i>

* [ Approach ] -> listenin
* [ Drive Away ] -> drive

== drive ==
-> END

== listenin ==
Their hair is up in puffy updos, and they're wearing aprons that they occasionally have to wipe desert dust off of. These women seem out of place under what used to be an old bus station, wrecked-apart by time.

<b>Stranger 1:</b> That <i>Emily</i> can't even count backwards from 10! <i>10!</i>
<b>Stranger 2:</b> As if Dalton is any smarter, girl!... Come on, he thought your name was actually Olive Oil for years!
<b>Stranger 1:</b> That's an easy mistake to make!... Olivia, Olive Oil... He's got so much on his plate, I can't blame him!
<b>Stranger 2:</b> Sure... what, he drinks olive oil instead of a protein shake?
<b>Stranger 1:</b> Yeah, yeah... sure- wait. They're staring at us.

* [ Drive Away ] -> drive
* [ Join Conversation ] -> join

== join ==
<b>Stranger 2:</b> Uhh. Hi. What, did you hear the whole thing?!
<b>Stranger 1:</b> You might as well help us settle this, since you're here <i>stalking</i> us.

(i need to figure this out better, WIP!)
-> END


