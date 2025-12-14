VAR speaker = "box"
VAR to_export = "team_count;annoying;printert;replacement;lore_dropped"
VAR lore_dropped = false

VAR team_count = 0
VAR annoying = false
VAR printert = false
VAR replacement = false

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
(Looks at you adorably)

{lore_dropped:
    -> sad
}

-> happy

=== sad ===
{annoying:
    ~speaker = "annoying"
    ...
}
{printert:
    ~speaker = "printert"
    ...
}
{replacement:
    ~speaker = "replacement"
    ...
}

-> adopt

=== happy ===
{annoying:
    ~speaker = "annoying"
    Beautiful specimen.
}
{printert:
    ~speaker = "printert"
    Pettable.
}
{replacement:
    ~speaker = "replacement"
    :)
}

-> adopt

=== adopt ===

* {team_count < 3} [Adopt him]
    ~ speaker = "box"
    ~ add_to_team("box")
    :)
    -> END
* [Leave him]
    ~ speaker = "box"
    :(
    -> END