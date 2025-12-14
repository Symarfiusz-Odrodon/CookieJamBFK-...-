EXTERNAL add_to_team(id)
EXTERNAL remove_from_team(id)
EXTERNAL improve_morale()
EXTERNAL hurt_morale()

VAR to_export = "printert;annoying;replacement;annoying_admit_thief;hat;team_count"
VAR speaker = "pc"
VAR printert = false
VAR annoying = false
VAR replacement = false
VAR hat = false
VAR annoying_admit_thief = false
VAR team_count = 0

-> intro

=== intro ===
Oh, hi hello.
It has been
a while # italic
since I saw someone here.
how is your day afternoon night?
HAULT
Necklace.
Mmmm.
* [Yes]
* [Um, thanks]
* [You're welcome]
- What are you doing here.
{
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
- printert:
    We are here to mess up upper management.
    -> to_cause_trouble
}
* [To cause trouble] -> to_cause_trouble
* [To steal things] -> to_cause_trouble


=== to_cause_trouble ===
~ speaker = "pc"
Oh.
Cool.
...
Do you require assistance?
{
- annoying:
    ~ speaker = "annoying"
    Hey listen.
    Could we not have him here?
}
-> END