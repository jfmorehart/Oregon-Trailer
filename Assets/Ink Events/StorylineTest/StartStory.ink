INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)
//spawn in player
#enter you appearright l #spr you you
My brother is dead

I must go out to the secret californian way 

    * [Go to Cali]
    -> calif
===calif ===

#speaker: you #spr: you   #pos:left #emotion: playerReact
Oh it is the famous gatorhead. Hi gatorhead.

#speaker: gatorhead #spr: gatorhead # pos: right
Hi Oregon Trailer. Are you ready to Oregon trail? 

#speaker: you #pos:left #spr: you 
Oregon.
    * [Bring Gatorhead]
    ->friend
    * [Scorn him]
    ->enemy
===friend ===
~ setGatorHeadInParty(true)
 #speaker: gatorhead #emotion: pigReact
Come along friend
 #speaker: gatorhead
I do drugs

->END
===enemy===
~ setGatorHeadInParty(false)
i hate you #speaker: gatorhead #emotion: pigReact
->END