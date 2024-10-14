INCLUDE globals.ink
EXTERNAL causeEvent(eventID)

You come across a puppy
    * [Kick the puppy]
        ->kicked
    * [avoid the puppy]
        ->avoided
=== kicked ===
~ puppy_kicked = "kicked"
~ causeEvent(9)
you kick the puppy. 
You monster
->END    
=== avoided ===
~ puppy_kicked = "avoided"
~ causeEvent(9)
You avoid the puppy
->END