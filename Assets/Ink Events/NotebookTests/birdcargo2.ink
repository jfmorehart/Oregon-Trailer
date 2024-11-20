INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

-> birdcargo2

== birdcargo2 ==

The road here goes right up next to a great big warehouse, and what looks like one of those old electric substations. Weirdly enough, this one seems to be maintained, although theres nobody around right now. 

* [ Stop the car! ] ->left
* [ Keep driving] ->END

== left ==

You drive up to the warehouse's delivery door, which looks unlocked. There's also a padlocked crate outside.

* [ Grab the crate and hit the gas] ->END
* [ Knock on the door] ->open
* [ Sneak inside] -> open


==knock==
The door creaks and rattles loudly, but even after knocking repeatedly, nobody answers. You can hear noises inside. Is that... chirping?
    
    * [Screw it, grab the crate instead] ->END
    * [Open the door] -> open
-> open

== open == 
The door is heavy, and groans as Gatorhead opens it. Just as it opens, a rush of air and feathers whooshes past you, and you jump away. 
A bird, apparently startled by the sound, is now circling above you, chirping angrily. Soon after, another bird joins it, followed by another. Curious. 
    
    * [Continue inside] ->inside


== inside==
Your eyes take a moment to adjust to the darkness. 

You see the room is filled with stacks of crates.

One crate sits closer to the door than the rest, its lid damaged and slightly ajar. From inside, you hear a whirring. As you approach, it whirs louder. 
    * [Pry open the box] ->pry
    * [Shoot the box] -> shoot
    
== shoot == 
You all fire at the box, and it splinters into pieces. The gunshots are almost deafening in the small room. When your ears finally stop ringing, you notice that while the whirring has stopped, their is a new noise, coming from the door.

You all instinctively run out to check on the van, and are shocked to find a dark cloud of birds have formed above it. The sound is now cacophonous. 
There must be two hundred birds in the air above the warehouse, with more dotting the powerlines of the substation. All of them are watching you. 
    * [Run to the van] ->END
    * [Fire into the cloud] -> fight
    


== fight ==

You fight valiantly, but go down in a heroic blaze of bullets and feathers. 

-> END

== pry ==
You crouch down, and pry at the box lid with your fingers. This close, you can see some text printed on its lid- "REPAIRS" After a moment, it gives way, and inside is... is that...? Birds?

The box appears to be filled with birds, most of them stiff and unmoving. 
One of them moves weakly, watching you.
Gatorhead instinctively reaches for the injured bird.

    * [Help the bird] ->help

== help ==
Gatorhead takes the bird in his hands, and then frowns, and hands it to you. 
The bird is far heavier than you expected a bird to weigh. Its feathers feel dusty, and very cold.

It looks at you with beady eyes, moves its wings slightly, and opens its beak to chirp.

Instead of a chirp, it produces a deafening metallic whir. You drop the bird reflexively, and it lands with a clang. 

Gatorhead turns and runs for the van. You all follow him. Outside, there are now dozens of birds circling the warehouse.
You drive off without looking back.
-> END



