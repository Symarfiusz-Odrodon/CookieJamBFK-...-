EXTERNAL add_to_team(id)
EXTERNAL hurt_morale()

VAR speaker = "printert"
VAR annoying_admit_thief = false
VAR printert_spoke = false

CONST to_export = "annoying_admit_thief;printert_spoke"

{printert_spoke:
    -> reconsider
}

-> intro

=== intro ===
Oh wait, are you a...
Oh. You have her here with you.
~ speaker = "annoying"
Oh hi.
How have you been.
~ speaker = "printert"
Save it.
What is going on here?
{annoying_admit_thief:
    ~ speaker = "annoying"
    Oh, I'm just helping this guy over here rob this place, that's all.
    ~ speaker = "printert"
    Of course you are.
    Have you even thought for a second what you are doing?
    ~ speaker = "annoying"
    Something cool?
    ~ speaker = "printert"
    This is exactly what I mean.
    You don't think.
    You just do things.
    That's why nobody likes you.
    ~ speaker = "annoying"
    What?
    Everyone likes me!
    ~ speaker = "printert"
    But with that in mind.
    I'd like to join.
    ~ speaker = "annoying"
    Huh?
    Then what was that speach for?
    ~ speaker = "printert"
    It doesn't matter what is your conclusion, what matters is how you get to it.
    I have nothing to lose.
    And I know I'll get a kick out of annoying upper management.
    -> proposition
}
{not annoying_admit_thief:
    ~ speaker = "annoying"
    Oh, I'm just helping this guy over here do inspections.
    ~ speaker = "printert"
    Inspections?
    God, you are dumb.
    What kind of inspections, would he be doing here?
    Nobody cares about what happens here!
    ~ speaker = "annoying"
    You lied to me?
    Listen, I am a fan of crimes, but you have hurt me on a personnal level.
    ~ hurt_morale()
    ~ speaker = "printert"
    Hey, you guy, are you gonna do something bad?
    Do you want to steal something?
    No matter, I'm in.
    Even if it's with her.
    -> proposition
}

-> END

=== proposition ===
~ speaker = "printert"
Also, just a heads up.
This person is a mennace.
~ speaker = "annoying"
Hey! I'm not!
~ speaker = "printert"
Everyone hates you!
Literally everyone!
Trust me, you will get only into trouble with that thing with you.
But, anyway.
Can I join you?
* [Yes] -> accept
* [No] -> decline

=== accept ===
Thank you.
I'll unlock the elevator for you.
~ printert_spoke = true
~ add_to_team("printert")
-> END

=== decline ===
...
Well...
Okay then.
Not sure why you would prefer having her with you instead of me, but whatever.
As long as you cause trouble.
I'm gonna unlock the elevator for you.
~ printert_spoke = true
-> END

=== reconsider ===
So, did you reconsider my proposition?
* [Let him join] -> accept2
* [Decline again] -> decline2

=== accept2 ===
Oh wait really?
Awesome.
~ add_to_team("printert")
-> END

=== decline2 ===
Oh.
Yeah, okay.
-> END
