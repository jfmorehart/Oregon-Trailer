EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> grenade

== grenade ==
There's a lot of shouting coming from a trio of men passing a beach ball around. They're shouting and cheering, even though the beach ball is floating very slowly in the air, from one pair of hands to another.

They stop at one point, however, and let the ball fall right between them. You can't really hear what they're saying, but the cheerful shouting soon turns into a <i>shouting match.</i>

* [ See What The Problem Is ] -> approach
* [ Leave ] -> leave

== leave ==
SADsOP men are kind of scary when you're not mentally prepared for it. It's best that you leave, anyway.

-> END

== approach ==
You slow the van down next to them. Stuck in their little spat, they don't notice you.

#enter fratbro1 fastappearleft ml
#spr fratbro1 fratbro1
<color=\#BE3D3D>Why are we playing with this stupid ball, bro? This is <b>so</b> boring...</color>
#enter fratbro2 fastappearright mr
#spr fratbro2 fratbro2
#state fratbro2 flip
<color=\#C6713F>Bros, bros, it's not even SADURDAY, like... we can try new stuff... this is all we have-</color>
#exit fratbro1 fastexitleft
#enter fratbro3 fastappearleft ml
#spr fratbro3 fratbro3
<color=\#7C1313>But the grenade <i>falls</i> better. It <i>fits</i> better in our hands, dude! It doesn't... float off!</color>
#exit fratbro3 fastexitleft
#enter fratbro1 fastappearleft ml
#spr fratbro1 fratbro1
<color=\#BE3D3D>Yea... how can we play Hot Grenade without a grenade?</color>

#exit fratbro1 fastexitleft
#exit fratbro3 fastexitright
... They use a grenade for a ball?

{
    - has_grenade == true:
    -> toldya
    - else:
    -> caught

}
    
== toldya ==
The grenade under the log, the "Grenade Football" flyer... it's all coming together.
#enter gatorhead appearleft ml
#spr gatorhead gatorhead
<color=\#3d7524>(Told ya. Plus... only the SADsOP weirdos would use a grenade for a ball.)</color>*/
-> caught

== caught ==
#enter fratbro1 fastappearleft ml
#spr fratbro1 fratbro1
<color=\#BE3D3D>Yo, what are you staring at us for?</color>
#enter fratbro3 fastappearright mr
#spr fratbro3 fratbro3
#state fratbro3 flip
<color=\#7C1313>Yea! Are you laughin' at us?</color>

//if you have the grenade, you get the below choice, but for now ill just provide it
{ 
    - has_grenade == true :
    -> choices
    - has_grenade == false:
    -> choice
}

== choices ==
* [ Give Grenade ] -> give
* [ Drive Off ] -> drive

== choice ==
* [ Escape ] -> drive

== drive ==
You've been spotted eavesdropping. You have nothing to offer, so you hit the gas.

-> leave

== give ==

#state fratbro1 bounce
<color=\#BE3D3D>WOW NO WAY! YOU HAVE ONE ON YOU?!</color>
#state fratbro3 bounce
<color=\#7C1313>THAT'S SICK!</color>
#exit fratbro1 fastexitleft
#enter fratbro2 fastappearleft ml
#spr fratbro2 fratbro2
<color=\#C6713F>Bros, this even has a label... it's probably ours anyway! They probably stole it-</color>
#exit fratbro2 fastexitleft
#enter fratbro1 fastappearleft ml
#spr fratbro1 fratbro1
#state fratbro1 bounce
<color=\#BE3D3D>WHO CARESSS, WE HAVE A GRENADE!</color>

The one who you gave the grenade to waves it in the air carelessly. The other two look up at it like it's a holy artifact.

<color=\#BE3D3D>Here dude, reward time. You totally saved our ass.</color>
<color=\#7C1313>If you ever meet a dude named Dalton... tell him we sent ya. He'll know.

~causeEvent(14)
~causeEvent(14)
<b>[Resource Gained: +20 Currency]</b>

#exit fratbro1 exitleft
#exit fratbro3 exitright
They go back to playing, kicking the beach ball aside. Now they seem like they're actually playing football with the grenade, tossing it at each other.

-> leave2

== leave2 ==
With that, you drive off.

-> END



