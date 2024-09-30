EXTERNAL causeEvent(ID)
VAR bonus = 0

-> inNOut

===inNOut===
A fabled restaurant of the olden days towers before you. 

    * [Scout Perimeter]
        -> inNOut_scout
    * [Go Inside]
        -> inNOut_goInside
    * [Leave]
        -> inNOut_leave

=== inNOut_scout === 
~ bonus = 15
Scouting the perimeter reveals evidence of a faction taking settlement in the building. Most revealing is the giant patch of graffiti on the back west wall showcasing the SADSOP logo in fresh paint.

Do you still want to go inside? 

    * [Yes]
        -> inNOut_goInside
    * [No]
        -> inNOut_leave
        
=== inNOut_goInside ===
~ temp num = RANDOM(1, 100)

{ num + bonus > 50: 
    ~ bonus = 0 
    -> inNOut_goInsideSuccess
- else: 
    ~ bonus = 0
    ->inNOut_goInsideFail
}

=== inNOut_leave ===
~ temp num = RANDOM (1, 100)

{ num > 50: 
    -> inNOut_leaveSuccess 
    - else:
    ->inNOut_leaveFail
}

=== inNOut_goInsideSuccess ===
~causeEvent(6)
~causeEvent(6)
~causeEvent(6)
You skirt around the inside of the building, evading patrolmen stationed around its interior. Your van is a day's rations richer once you come out and unload your stolen finds.

->END
=== inNOut_goInsideFail ===
~causeEvent(2)
“HA!” Someone barks. You freeze in place, frozen food in hand. “Another bastard thinks they can just waltz in an’ out of here without consequence. Get ‘em!”

-> END

=== inNOut_leaveSuccess ===
~causeEvent(5)
Better not to disturb the relics, you tell the squad, and you beat a hasty retreat off into the surrounding desert. One of your crew members manages to scrounge up a little something from the foot of the building before catching up to the rest of you.
 -> END

=== inNOut_leaveFail === 
~causeEvent(2)
These types of places always attract people, and usually not the pleasant kind. You know that all too well. Unfortunately, knowledge doesn’t stop them from jumping your van as you’re pulling out from the parking lot. 


->END