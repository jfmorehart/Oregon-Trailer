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
You attempt to drive through the storm
pre
~ temp num = RANDOM(1, 100)
post
{ num > 99: 
    -> driveThroughSuccess
- else: 
    ->driveThroughFail
}

=== takeCoverSuccess ===
~causeEvent(6)
You stop the van outside an abandoned joint and everyone shelters inside successfully. You even find an untouched food kit wedged inside a cabinet for the taking. 

->END
=== takeCoverFail===
~causeEvent(4)
You get blasted with dust so bad that you get bowled over. You have to scramble to drag yourself out, though you at least manage to successfully take cover afterwards. 
->END
=== driveThroughSuccess ===
~causeEvent(5)
Long live the vainglorious! You charge into the dust storm without giving a shit whether your wheels are still making contact with the asphalt below. If you die, you die—and your van comes out the other end relatively unscathed!
->END
===driveThroughFail ===
Somehow, you aren’t the only one crazy enough to try driving through this dust storm…
~causeEvent(2)
->END


-> END