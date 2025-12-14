VAR speaker = "hat"
VAR has_highlighter = false
VAR horse_hoisted = false

VAR to_export = "horse_hoisted;has_highlighter"

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

Do YOU happen to have a gray highlighter?
You could use one to fix that green necklace.

* { has_highlighter } [(give highlighter)] -> you_in_fact_have_a_highlighter
* [No]
    Hmm.
    That is less than ideal.
    -> after_intro

=== you_in_fact_have_a_highlighter ===
~ intro_highlighter_shown = true
Hm...
...
...
This...
This highlighter...
it's just...
it's just wrong.
Too blue.
You can't shade with it.
It's too sharp.
* [Do you sharpen your highlighters?]
    Well obviously.
    Why should highlighters not be sharpened, when pencils and pens are?
    -> after_intro
* [It's just a highlighter]
    Just a highlighter?
    Just a highlighter!?
    Sir.
    Mister.
    How would you expect to hoist a horse with a tool of improper caliber?
    ~ horse_hoisted = true
    -> after_intro

=== after_intro ===
* [Could you give me your keycard?]
    ~ horse_hoisted = false
    -> ask_for_keycard
* {horse_hoisted} [Sire, I am in need of your access to the higher levels of this facility.]
    -> ask_for_keycard

=== ask_for_keycard ==
{ intro_highlighter_shown and not horse_hoisted:
    You want to go further?
    With that thing?
}
{ intro_highlighter_shown and horse_hoisted:
    You want to venture forth!
    With such a disgrace of a emphasization tool such as this?
    You call this a highlighter, I wouldn't even call it a minor point out-er.
}
{ not intro_highlighter_shown:
    Without a highlighter?
    What would you even do with it?
}
* [Let's look for a {intro_highlighter_shown: better} highlighter together.]
    ~ horse_hoisted = false
    -> accept
* {horse_hoisted} [Worry not, we shall venture forth and locate the right highlighing device!]
    Splendid.
    -> accept
* [I'm just gonna... leave...]
    -> END

=== accept ===
-> add_to_team("hat")
-> END