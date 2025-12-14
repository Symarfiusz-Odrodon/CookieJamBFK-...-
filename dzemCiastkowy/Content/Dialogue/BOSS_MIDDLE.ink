VAR to_export = "printert;annoying;replacement;hat;pc;team_count"
VAR speaker = "dl"
VAR printert = false
VAR annoying = false
VAR replacement = false
VAR hat = false
VAR pc = false
VAR team_count = 0

-> intro

=== intro ===
...
{
    - hat:
        ~ speaker = "hat"
    - printert:
        ~ speaker = "printert"
    - annoying:
        ~ speaker = "annoying"
    - replacement:
        ~ speaker = "replacement"
    - else:
        ~ speaker = "pc"
}

{team_count > 0:
    ...
    -> intro2
}

* [...] -> intro2
-> intro2

=== intro2 ===
~ speaker = "dl"
......
{
    - hat:
        ~ speaker = "hat"
    - printert:
        ~ speaker = "printert"
    - annoying:
        ~ speaker = "annoying"
    - replacement:
        ~ speaker = "replacement"
    - else:
        ~ speaker = "pc"
}

{team_count > 0:
    ......
    -> intro3
}

* [......] -> intro3

=== intro3 ===
~ speaker = "dl"
.........................................................
{
    - hat:
        ~ speaker = "hat"
    - printert:
        ~ speaker = "printert"
    - annoying:
        ~ speaker = "annoying"
    - replacement:
        ~ speaker = "replacement"
    - else:
        ~ speaker = "pc"
}

{team_count > 0:
    dot dot dot
    -> END
}

* [dot dot dot] -> END
