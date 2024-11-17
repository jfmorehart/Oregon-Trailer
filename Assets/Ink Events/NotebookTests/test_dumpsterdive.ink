EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> dump

== dump ==
You see a couple of worn-down dumpsters by the side of the road: one of them open and the other one not. You can tell that they <i>smell</i> even from a distance.

* [ Drive Closer ] -> drive
* [ Leave ] -> leave

== drive ==
Come to think of it, you could throw some trash away while you’re at it. The van hasn’t been smelling the best. 
You can also see the edge of a magazine that peeks out of the closed dumpster.

* [ Throw Trash in Open Dumpster ] -> open
* [ Pull Out Magazine from Closed Dumpster ] -> closed

== open ==
You throw your trash away and return to the van, which already feels a bit better to lounge in (without worrying about the smell).
-> END

== closed ==
You nearly rip the magazine as you try to pull it out. It looks quite old, but you can tell it was a fashion magazine by the model posing on the front. Someone could make use of this.

The closed dumpster remains closed shut, but maybe it could be open. And maybe you can open it.

* [ Throw Trash; Open Dumpster ] -> open
* [ Try to Open Closed Dumpster (Constitution > 2) ] -> attempt

/*{player_constitution > 2: (this is for making the choice appear or disappear)

* [ Try to Open Closed Dumpster (Constitution > 2) ] -> attempt
}*/

== attempt ==
//(skill check, if constitution > 2, open, if constitution =< 2, fail)
The lid falls off with a clunk. You don’t need to dig too much through the dumpster, thankfully, for you to uncover an old bobble-head figurine. It has a baseball cap, but instead of a baseball bat, this one has a gun.

//<b>[ Item Acquired: Gun-toting Bobblehead ]</b>

-> open

== leave ==

-> END