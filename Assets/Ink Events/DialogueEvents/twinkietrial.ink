EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> trial

== trial ==

//this is an exceptionally large event. usually this would be a dialogue sequence. i'm also setting this up to be a segway into the main event, the derby. next sprint ;)

There usually aren't any giant structures towering over you as you drive. <i>Usually.</i> So you're kind of startled by the sudden big shadows cast over the van. Looking up and out of your windshield, you spot a giant banner, which reads "TRAIL" in big letters. You think it could be random signage, but then you see "The People vs. Twinkie Thief."

Huh. So it's misspelled. Though judging by the tank sitting right underneath it, you really shouldn't have been surprised. It's a SADsOP event.

//Gatorhead bumps on your shoulder repeatedly to stop the car, and you pump the brakes hurriedly before the big man can "accidentally" ram your face into the steering wheel. He's excited. The women glance at each other before looking out of the window.

You roll both windows down - at the apparent interest of your teammates, as well - and pull the curtains back in order to get a better look and listen in.

There's a podium covered in tattered flags and empty cups, and around it are over thirty buff men, all in cropped tees and baggy shorts, and with weapons hoisted on their shoulders. A man just like them is yelling on the podium.

<b>"I DIDN'T STEAL THE TWINKIE YOU GUYS!!!! WHY WOULD I DO THAT!!!"</b>

You can hear someone from the audience yell, <i>"Because you're a beta cuck, Jared!"</i> And then a <i>"Shut up, dude!"</i> Those are what <i>you</i> can hear anyway, but you're sure that there's more where that came from, seeing as the "people" look <i>pressed.</i>
This twinkie was a big deal.

<b>"BUT I DIDN'T DO IT...."</b>

Another man perks up from the corner of the podium, accompanied by two others - his Littles - who are each holding a big glove. "We saw you leave the Kitchen even though you said you were going to Electrical, Jar."

Jared is about to pass out from how red in the face he is. "I-I'm embarrassed that I like sweets, okay?! But I didn't eat that damn twinkie!"

His eyes scour the audience for help, but instead of finding friendly faces in the audience, he finds them with you. He squints.

* [ Stare Back ] -> stare
* [ Hurry Away ] -> leave

== leave ==
~causeEvent(3)
Before this Jared can even say anything, you hit the gas so hard that all of your party members fly backwards and off of their seats. You ignore their shouting - apparently Gatorhead has fallen on Leslie's foot and Lady narrowly escaped - and drive away. 
Out of your rearview, you see Jared being manhandled back onto the podium. He <i>really</i> wanted you as his audience. In the end, you lose extra gas from all of that rushing.
-> END

== stare ==
<b>"YOU!"</b> He points to your van, which causes the whole squadron to whip around to face you. You point to yourself, mouthing "Me?" Before a "GET THEM" is unleashed. Your party members leave the van sooner than you, and drag you with them before the SADsOP-s can lay their hands on either of you. //lol

You drag your feet to the trial. The people have left some space for you, in the spirit of "democracy" and such, and the four of you stand there, silently glancing at each other. Lady is the only one who's throwing nasty glances outward, her hand on one of her holsters.

-> trialtime

== trialtime ==

"Thanks for coming," the other man - not Jared - grins. "Jared here is accused of eating the last Twinkie we had. It was our ... very last..." His expression falls. The others put their hands on their chest in solemnity. Jared does too, until he reminds himself.

"Hold on now! I didn't eat it! Why are you all saying I did!"

"You definitely did."

"Bobby, what the <i>fuck</i> dude?" Jared turns towards the other. "We literally were chatting when the Twinkie was reported missing!"

"Were we? I don't remember...."

"<i>UGHHHHH H H H H H</i>" Jared turns to you. "Please help me. They're framing me! It's deffo because I said I didn't like orange-flavored alcohol!!!!!"

You hear someone from the audience say, "Who the hell doesn't like orange-flavored alcohol?" And then a "Heretic! Burn him!" before they started clanking their weapons together, making a racket. Someone even shoots a gun in the air.

"Not liking orange-flavored anything is a <i>sin</i>," Bobby says, shaking his head like a disappointed father. The Littles beside him nodded fervently. "I'm allergic to oranges," one of them says before being kicked in the knee by the other one.

Lady finally looks at you after so long. Her look, just like your thoughts, mean, <i>What the fuck are they yapping about?</i> Leslie on the other hand is trying really hard to hold her laughter in. She looks red.

* [ Side with the People ] -> orange
* [ Side with Jared ] -> twinkie

== orange ==
~causeEvent(14)
~causeEvent(14)
~player_gumption --
The crowd cheers and screeches. Jared holds his face in his hands, initially not noticing two "jailers" come up to him and grab him by the elbows, dragging him backwards.

<b>"NO!"</b> He yells. <b>"I DIDN'T DO IT, I SWE A A A A A A -"</b> His yelling is obstructed by a punch to the nose.

Another guy approaches you and hands you crumpled up stamps, which you look at, confuddled. Gatorhead takes them from you and stuffs them in his pants' pocket. No questions asked. <i>It's like a reward.</i> (From judging someone?)

"What are you gonna do to him?" Someone from the audience asked.
"Kill him," Bobby said, without missing a beat.

The crowd went silent, and so did your party. Leslie suddenly doesn't want to laugh anymore.
... Crickets.

"That's too much," you hear, and Bobby sighs as well. "Alright, <i>fine</i>, just a 1000 push-ups."

<i>Oh, that's too little, that's nothing,</i> are what you hear from the clamoring that has resumed, and before you can react, Gatorhead grabs you and the ladies and you rush your way to the van. Unable to deal with the consequences of whatever they're going to put poor Jared through.

Poor Jared. He definitely ate the twinkie though.
Right?
-> END

== twinkie ==
~causeEvent(6)
<b>"THANKS, THANKS, THANKS,"</b> Jared nearly starts convulsing on the podium, shaking his head back and forth. There's jeering, but there's cheers too. It seems that the People are easily swayed.

Bobby sighs. "Alright, <i>fiiiine</i>, just because the Outsiders helped you out. Relax. You're scot free this time. Now you-"

Jared is barely listening, instead bounding over to your party, hugging you tight. You feel like your bones are going to be crushed. Just as you think he's about to let go, not only does he hug you tighter, but he also slides something in your pocket.
"They actually mis-call-kuh-layted. There were <i>two</i> twinkies," the man whispers. He grins as he lets you go, and walks backwards, spreading his arms.

<b>"I STILL FUCKING HATE ORANGES!!!! RAAAHHHHHH!!!!!"</b>

<i>That's unfair, what a heretic</i> are what you hear from the clamoring that has resumed, and before you can react, Gatorhead grabs you and the ladies and you rush your way to the van. Leslie bursts into laughter the moment you're in the van, and just for that, you give her the Twinkie to shut her up.
-> END