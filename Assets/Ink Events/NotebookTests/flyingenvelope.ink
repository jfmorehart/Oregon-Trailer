INCLUDE ../globals.ink
EXTERNAL causeEvent(ID)

-> envelope

== envelope ==
You think you see something outside of your window, but before you can stop, something <i>else</i> hits your windshield. It’s caught underneath one of your wipers, and it’s about to fly away.
It looks like a rumpled, yellowed-out piece of paper.

* [ Take It ] -> take
* [ Keep Driving ] -> drive

== drive ==
The wipers instinctively move across the windshield, causing the paper to fly away into the wind.

-> END

== take ==
You slow down. After getting out of the car, you take the paper object from your windshield. It has an openable slit in it - it's an envelope. Opening it, you find some money.

* [ Take Money ] -> money
* [ Leave Envelope ] -> leave

== money ==
<b>[ You have earned $5! ]</b>

When you take the money out of the envelope, a yellow letter falls out. Picking it up, it reads:

"-- church-- ahead-- help"

You pocket the money and the letter.

-> END

== leave ==
You throw away the envelope. You don't need the money, it seems.

-> END
