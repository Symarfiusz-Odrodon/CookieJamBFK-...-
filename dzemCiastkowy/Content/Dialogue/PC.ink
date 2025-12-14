VAR to_export = "printert;annoying;replacement;annoying_admit_thief;hat;team_count;flash_drive"
VAR speaker = "pc"
VAR printert = false
VAR annoying = false
VAR replacement = false
VAR hat = false
VAR annoying_admit_thief = false
VAR team_count = 0
VAR flash_drive = false

-> intro

EXTERNAL add_to_team(id)
EXTERNAL remove_from_team(id)
EXTERNAL improve_morale()
EXTERNAL hurt_morale()

=== function add_to_team(id) ===
~ return
=== function remove_from_team(id) ===
~ return
=== function improve_morale() ===
~ return
=== function hurt_morale() ===
~ return

=== intro ===
Oh, hi
hello.
It has been
a while # italic
since I saw someone here.
how is your day
afternoon
night? # bold
HAULT
Necklace.
Mmmm.
* [Yes]
* [Um, thanks]
* [You're welcome]
- What are you doing here.
{
- printert:
    ~speaker = "printert"
    We are here to mess up upper management.
    -> to_cause_trouble
- replacement:
    ~speaker = "replacement"
    Causing trouble?
    -> to_cause_trouble
- annoying and annoying_admit_thief:
    ~speaker = "annoying"
    We are here to steal things.
    -> to_cause_trouble
- annoying:
    ~speaker = "annoying"
    We are here to cause trouble.
    -> to_cause_trouble
}
* [{team_count == 0: I'm} {team_count > 1: We're} here to cause trouble] -> to_cause_trouble
* [{team_count == 0: I'm} {team_count > 1: We're} here to steal things] -> to_cause_trouble


=== to_cause_trouble ===
~ speaker = "pc"
Oh.
Cool.
...
Do you require assistance?

{team_count == 3:
-> capacity_reached
}

{
- annoying:
    ~ speaker = "annoying"
    Hey listen.
    Could we not have him here?
    Like, you see him.
    He's weird.
    ~ speaker = "pc"
    I do not wish to cause trouble
    difficulties
    problems
    inconviniences
    disturbances
    hot water
    grief.
    I can stay if that would be more convinient for you.
}

* [(Let him join)]
    ~ add_to_team("pc")
    I will try to help
    aid you to my best ability.
    
    {annoying:
        ~ speaker = "annoying"
        ~ hurt_morale()
        Really?
        This will be a pain.
        ~ speaker = "pc"
        ~ hurt_morale()
        I exchange simular feelings with this object.
    }
    -> any_more_assistance
    
* [(The opposite of that)]
    As you wish.
    -> any_more_assistance

=== capacity_reached ===
Oh, I see the elevator is already at max
full
compleated
capacity.
Well then, I'm not going to hold you.

{replacement:
    ~ speaker = "replacement"
    Hey, so...
    I kinda already replaced someone by going with you
    so if someone else wants to go now, it would be only fair if you replaced me with them.
    What do you think?
    * [You're staying]
        Oh.
        I'm not sure, why you want me here so much, but...
        thank you.
        -> any_more_assistance
    * [Okay]
        ~ remove_from_team("replacement")
        ~ add_to_team("pc")
        See you, later.
        Maybe.
        -> any_more_assistance
}

-> any_more_assistance

=== any_more_assistance ===
~ speaker = "pc"
Could I be of any other assistance?
* [(Plug in the flash light to a server)]
    ~ flash_drive = true
    As you wish.
* [Nothing]
- -> end

=== end ===
-> END
