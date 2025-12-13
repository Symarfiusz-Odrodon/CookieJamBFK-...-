VAR speaker = "tut"

-> intro

=== intro ===
Wait, Gary?
I haven't seen you in so long.
How's it been?
~ speaker = "gunman"
Oh, well, you know...
...
Who is that?
~ speaker = "tut"
Oh yeah, I forgot.
We are trying to sneak into the building.
~ speaker = "gunman"
Um...
...
Could you not?
* [No]
    Well, I can't let you do that.
    -> END
* [Ok]
    ~ speaker = "tut"
    Oh no, we ARE breaking in.
    I'm comitted.
    ~ speaker = "gunman"
    Well, I can't let you do that.
    -> END