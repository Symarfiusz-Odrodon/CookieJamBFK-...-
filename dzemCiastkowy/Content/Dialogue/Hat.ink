VAR speaker = "hat"
VAR has_highlighter = true
VAR horse_hoisted = false
VAR intro_highlighter_shown = false

CONST to_export = "has_highlighter;horse_hoisted;highlighter_num"

// To export
VAR highlighter_num = 0

{ highlighter_num == 0:
    -> intro
}

=== intro ===

Do YOU happen to have a gray highlighter?

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
* [Okay, I'm gonna look for a {intro_highlighter_shown: better} highlighter]
    -> finish_first
* {horse_hoisted} [Worry not, I shal venture forth and locate the right highlighing device!]
    -> finish_first

=== finish_first ===
~ highlighter_num = 1
-> END

=== first_highlighter ===
Mmmm...
No.
This won't do.
It's too green.
Try finding another one.
-> END