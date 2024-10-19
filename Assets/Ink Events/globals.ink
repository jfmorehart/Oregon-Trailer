VAR has_side_kick = false
//story points can be tracked using these variables
//https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#using-global-variables
VAR has_brick = false
VAR player_name = ""
VAR puppy_kicked = ""
VAR puppy_status = "Kicked"


VAR has_GatorHead = false
VAR has_Sam = false

VAR player_constitution = 0
VAR player_wisdom = 0
VAR player_charisma = 0
VAR player_moxie = 0
VAR player_gumption = 0

VAR testValue = 0

== function setTestValue(value)==
~ testValue = value


== function setGatorHeadInParty(value)==
~ has_GatorHead = value

== function setSamInParty(value)==
~ has_Sam = value

== function setCharisma(value)
~ player_constitution = value

== function setWisdom(value)
~ player_wisdom = value

== function setConstitution(value)
~ player_constitution = value

== function setMoxie(value)
~ player_moxie = value

== function setGumption(value)
~ player_gumption = value
