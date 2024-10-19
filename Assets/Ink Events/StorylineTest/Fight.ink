INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)



The player 

Oh no, I see something weird on the horizon #speaker: you  #spr: you #emotion:playerReact

{
    - has_GatorHead == true:
        `-> gatSpeak
}
#speaker: you
I wish Gatorhead was here to save me 
->gatAppear
===gatSpeak===
 #speaker: gatorhead #spr: gatorhead
I WILL SAVE US
~causeEvent(2)

->END
===gatAppear===
#speaker: gatorhead #spr: gatorhead
I am here to kill 
~causeEvent(2)
->END