EXTERNAL causeEvent(ID)

-> hog

=== hog===
A sizable, succulent hog stands on the pavement ahead, posing a challenge. Are you up to take it? 

    * [Yes]
    -> hogYes
    * [No thanks]
    -> hogNo
    
=== hogYes ===
~ temp num = RANDOM(1, 100)

{ num > 50:
->hogYesSuccess
 - else: 
 ->hogYesFail
}

=== hogYesSuccess===
You lunge at the pig faster than it can react, securing your crew several days’ worth of rations. Another opportunity well taken! 

… At the cost of half a day spent bickering over who deserved which cut as you prepped and loaded the pig into the van. 
~causeEvent(6)
~causeEvent(6)
~causeEvent(6)
->DONE


=== hogYesFail===
You make a break for the pig, but your crew members trip over themselves in their own glee and you all tumble into the dirt before crashing into a dead tree. By some stroke of luck, the tree has a cache of fuel in it—just not the food you were so looking forward to…
~causeEvent(5)
->DONE

=== hogNo === 
You keep your eyes on the road and make pace, arriving at your destination earlier than anticipated. A little extra beauty sleep will do wonders for your crew. 
~causeEvent(5)
-> DONE

-> END

