VAR to_export = "team_count;has_highlighter"
VAR team_count = 1
VAR speaker = "tut"
VAR has_highlighter = false

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
{team_count == 0:
    Well this is akward.
    It shouldn't be possible to get to this state.
    Somehow, the tutorial killed your only companion.
    Womp womp.
    Here's a bit of wisdom:
    The bigger the spoon, the bigger the money.
    I shall now leave you be.
    -> END
}
Well that was something.
But I'm afraid we'll have to go our separate ways.
I'm gonna leave on this floor and bleed out or something.
I mean, I'm not bleading, but I'm sure I'll figure something out.

I can only give you this gray highlighter:
* [(Accept)]
    ~ has_highlighter = true
* [(Decline)]
    ...
    Okay?
    Refusing a gift from a dying person.
    Sure.
- Farewell.
~ remove_from_team("tut")
-> END