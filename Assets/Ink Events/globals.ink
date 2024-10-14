VAR has_side_kick = false
//story points can be tracked using these variables
//https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#using-global-variables
VAR has_brick = false
VAR player_name = ""
VAR puppy_kicked = ""
VAR puppy_status = "Kicked"


VAR has_GatorHead = false
VAR has_Sam = false

== function setGatorHeadInParty(value)==
~ has_GatorHead = value

== function setSamInParty(value)==
~ has_Sam = value
