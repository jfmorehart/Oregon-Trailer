

-> main

===main===
You encounter an old man lying in the middle of the road
    + [Try to Wake Him Up]
        ->WakeUp
    + [Run him over]
        ->RunOver
    + [Honk with car]
        ->HonkCar
    + [Travel from the side]
        ->Avoid


=== RunOver ===
You must choose an option now.

    + [a]
        ->playerchoice("a")
    + [b]
        ->playerchoice("b")
    + [c]
        ->playerchoice("c")
        
===playerchoice(pc)===
you chose {pc}
->DONE
===WakeUp===
You must choose an option now.
    + [a]
        ->playerchoice("a")
    + [b]
        ->playerchoice("b")
    + [c]
        ->playerchoice("c")
    + [d]
        ->playerchoice("d")
        
->DONE
===HonkCar===
displaytext
text to display
text here as well
text
->DONE
===Avoid===
texthere
->DONE