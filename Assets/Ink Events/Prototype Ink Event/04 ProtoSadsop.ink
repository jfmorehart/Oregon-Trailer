EXTERNAL causeEvent(ID)

-> sadsop

=== sadsop ===

A guy in weird getup is cruising alongside you on a weirdly high-quality scooter and trying to talk at you through your window. 

    * [Watch]
    -> watch
    * [Open Window]
    -> open_window
    * [Speed Up] 
    -> speed_up
    * [Run Him Over]
    -> run_over

=== watch ===
He sees you watching him and starts gesticulating faster, throwing out hand symbols and gestures so wildly you have no clue what he's trying to say. 

    * [Open Window]
    -> open_window
    * [Keep Watching]
    -> keep_watching

-> DONE

=keep_watching
~temp num = RANDOM(0, 100)

{ num > 50:
    -> keep_watching_success
- else:
    -> keep_watching_fail
}
= keep_watching_success
~causeEvent(5)
He gives up on trying to talk to you, cruising away on his overly-decked out bike. Seems your cool unresponsiveness impressed him, though, because he left behind a little gift. 
-> DONE

= keep_watching_fail
~causeEvent(2)
Angered by you blatantly ignoring him, he punches the window! Time to retaliate.
-> DONE

=== open_window ===

"Finally!" He exclaims when you roll down the window. "Even the worst SADsOP initiates aren't as slow as you guys. You know SADsOP? They're—including me—the best." 

He says a bunch more nonsense, then finally ends it with: "Where y'all headed?"

* [Answer Honestly]
-> answer_honestly
* [Answer Dishonestly]
-> answer_dishonestly
-> DONE

= answer_honestly
~causeEvent(7)
~causeEvent(7)
"That's what's up," he says, offering you a handshake and a bag of... something. "I gotta get going now, but this was fun. See ya around!"

-> DONE

= answer_dishonestly
~causeEvent(6)
~causeEvent(6)
~causeEvent(6)
"Hmm. Sounds fake but ok! Keep going where you're going, am I right?" He doesn't give you a chance to respond before nodding sagely, tossing you a bag of... something, and blasting away on his motor-scooter.

-> DONE

=== speed_up ===
He seems to take this as a challenge, increasing his speed to match yours. That must be one highly modded scooter. 

* [Keep Speeding] 
-> keep_speeding
* [Stop Abruptly]
-> stop_abruptly

=keep_speeding
~causeEvent(5)
~causeEvent(5)
You match pace with each other for a couple minutes, blasting across the highway, until finally he yells something completely unintelligible and pulls away. Goodbye, weirdo.  

Looks like he left gift on the back of your RV somehow.  

-> DONE

=stop_abruptly
~causeEvent(6)
You slam the brakes as abruptly as you
can without tipping the car over. The dude seems shocked as he speeds past, but then laughs and continues on at the same pace, leaving you in the dust. 

Looks like he managed to sneak a little 'gift' onto the back of your car at some point.
->DONE

=== run_over ===
~causeEvent(2)
He reacts pretty badly to you trying to run him over. Looks like you'll have to earn the right to do it. 


-> END