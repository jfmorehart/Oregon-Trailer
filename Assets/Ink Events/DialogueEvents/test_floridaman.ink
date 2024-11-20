EXTERNAL causeEvent(ID)
INCLUDE ../globals.ink

-> floridaman

== floridaman ==
<b>Man</b>: <b>WE BEEN DENIED OUR PROMISE LAND FOR FAR TOO LONG!</b>

Someone is walking in circles off the side of the road, holding a giant sign, shouting loud enough for you to hear with your windows closed.

<b>Man</b>: <b>THE BIG MEN UPSTAIRS ARE NOT <i>OUR</i> BIG MAN UPSTAIRS!</b>

* [ Approach ] -> approach
* [ Drive Away ] -> leave

== leave ==

-> END

== approach ==
As you drive closer, the man stops and waves his sign in your direction. You see then that the sign reads: "BE BORN AGAIN... IN FLORIDA!"

<b>Man</b>: GOOD DAY! HAVE YOU EVER BEEN TO THE PROMISED LAND?

* [ Ask About It ] -> ask
* [ Answer Yes ] -> yes
* [ Answer No ] -> no

== ask ==
<b>Man</b>: THE SUNSHINE STATE. FLORIDA.

* [ Answer Yes ] -> yes
* [ Answer No ] -> no

== yes ==
He appears overjoyed.

<b>Man</b>: YOU ARE <i>BLESSED!</i> TRULY! I WISH TO GO TOO, TO SEE <i>HIM</i>...

-> exposition

== exposition ==
<b>Man</b>: IT IS ALWAYS SUNNY IN FLORIDA, NOT LIKE THE FALSE SUN OF THE WEST COAST! THE BIG CORPOS DON'T EXIST UNDER THE <i>TRUE</i> SUN!

<b>Man</b>: I SHALL ONE DAY RAISE MONEY TO GO AND SEE THE HOLY LAND. THAT'S WHERE <i>HE</i> LIVES. THE HLE. THE HOLY LAND EXPERIENCE.

<b>Man</b>: THE GREATEST THEME PARK KNOWN TO THESE UNITED STATES- NAY, MANKIND-

<b>Man</b>: - Can I have some money by the way?

* [ Give Money (-7) ] -> give
* [ Refuse ] -> refuse
* [ Rob (+7) ] -> rob

== no ==
He shakes his head.

<b>Man</b>: ME NEITHER. IT IS MY DREAM TO GO THERE. TO SEE... <i>HIM</i>...

-> exposition

== give ==
<b>[ Resource Lost: -7 Currency ]</b>

<b>Man</b>: Thanks. Where was I...

<b>Man</b>: -<b>IT IS TIME FOR US TO GO BACK TO WHERE WE BELONG! TOWARDS SUNBURN BY THE ONE TRUE-</b>

He seems to have resumed his routine, which marks the end of a very strange interaction.

-> END

== refuse ==
<b>Man</b>: Oh. Okay. Darn.

He shifts uneasily.

<b>Man</b>: Well, you're just wasting my time if you're not going to give me anything, so... where was I?...

<b>Man</b>: -<b>PUNISH THE COMPLICIT BY MOVING FAR FAR AWAY TOWARDS THE DIRECTION OF-</b>

He seems to have resumed his routine, which marks the end of a very strange interaction.

-> END

== rob ==
<b>Man</b>: Woah-woah-woah. Okay. Okay. Here you go. //terrified, drops sign

<b>[ Resource Gained: +7 Currency ]</b>
<b>[ Item Gained: Map of Park ]</b>

<b>Man</b>: ... EVIL! GO AWAY! I HOPE YOU GET A FLAT TIRE!

He sits on the ground as you drive away. He's holding the sign up.

-> END



