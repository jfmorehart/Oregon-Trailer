EXTERNAL causeEvent(ID)

-> motel

=== motel===
A motel decked out in neon lights comes into view. Could be a good place to rest.

* [Rest]
-> rest
* [Leave]
-> leave

=== rest=== 
~temp num = RANDOM(0, 100)

{ num > 50: 
-> rest_success
- else: 
-> rest_fail 
}

=rest_success
~causeEvent(5)
You check in and have a decent time undisturbed by pests or other guests. In the morning, you load up your RV with some complimentary gas and get going. 

-> DONE

=rest_fail
~causeEvent(6)
You sleep okay, but in the morning it seems someone siphoned fuel from the RV while you were busy with the complimentary breakfast. Oh well.
-> DONE

=== leave ===
These types of places are just a little too shady for your tastes. You keep on moving. 
-> END