EXTERNAL causeEvent(ID)

-> dustStorm

=== dustStorm === 
A pretty big dust storm is kicking up ahead, what's the plan? 

    * [Take Cover]
        -> takeCover
    * [Drive Through] 
        -> driveThrough


=== takeCover ===
~ temp num = RANDOM(1, 100)

{ num > 50: 
    -> takeCoverSuccess
- else: 
    ->takeCoverFail
}

=== driveThrough ===
~ temp num = RANDOM(1, 100)

{ num > 50: 
    -> driveThroughSuccess
- else: 
    ->driveThroughFail
}

=== takeCoverSuccess ===
~causeEvent(6)
You stop the van outside an abandoned joint and everyone shelters inside successfully. You even find an untouched food kit wedged inside a cabinet for the taking. 

-> DONE 
=== takeCoverFail===
~causeEvent(4)
You get blasted with dust so bad that you get bowled over. You have to scramble to drag yourself out, though you at least manage to successfully take cover afterwards. 
-> DONE
=== driveThroughSuccess ===
~causeEvent(5)
Long live the vainglorious! You charge into the dust storm without giving a shit whether your wheels are still making contact with the asphalt below. If you die, you die—and your van comes out the other end relatively unscathed!
-> DONE
===driveThroughFail ===
~causeEvent(2)
Somehow, you aren’t the only one crazy enough to try driving through this dust storm… 
-> DONE


-> END