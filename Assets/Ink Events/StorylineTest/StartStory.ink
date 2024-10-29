INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)
//spawn in player
#enter you appearleft ol #spr you you #speaker you
My brother is dead
#move you normal r
I will collect data
I must go out to the secret californian way 

    * [Go to Cali]
    -> calif
===calif ===

#speaker you #move you l fast 
Oh it is the famous gatorhead. Hi gatorhead.

#enter gatorhead appearleft ol #speaker gatorhead #spr gatorhead gatorhead 
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