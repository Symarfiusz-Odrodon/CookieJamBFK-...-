EXTERNAL add_to_team(id)
EXTERNAL remove_from_team(id)
EXTERNAL improve_morale()
EXTERNAL hurt_morale()

VAR replacement_spoke = false
VAR speaker = "replacement"
VAR printert = false
VAR to_export = "replacement_spoke;printert"

{ replacement_spoke:
    -> repeat
}

-> intro

=== intro ===
{printert:
    Hello...
    ~ speaker = "printert"
    WE HAVE TO HAVE HER.
    PLEASE.
    ~ speaker = "replacement"
    Um.
    What is exactly going on here?
    ~ speaker = "replacement"
    I'm not sure myself, something about causing trouble.
    Will you join us?
    ~ speaker = "replacement"
    Oh yeah sure.
    Wait, she is with you?
    ~ speaker = "annoying"
    Yeah Trixie, what you gonna do about it?
    ~ speaker = "replacement"
    I'm sorry, but this won't work.
    Unless you would get rid of her.
    But that would be mean a bit mean.
    Maybe deserved.
    But mean.
    ~ speaker = "annoying"
    Oh, but he can't.
    I'm an essential member.
    Plus, I'm the only one with a keycard to the upper floors.
    ~ speaker = "replacement"
    That's not true.
    I have one of those too.
    A non-stolen one, to be exact.
    ~ speaker = "annoying"
    o
    ~ speaker = "replacement"
    I don't recommend it...
    ~ speaker = "printert"
    BUT I WOULD!
    ~ speaker = "replacement"
    But if you would get rid of her, I could take her place.
    * [I'll let you join and get rid of her] -> accept
    * [I'm keeping her] -> refuse
- else:
    Hello, who are you.
    Oh, and...
    She...
    ~ speaker = "annoying"
    A simular plesure to meet you.
    ~ speaker = "replacement"
    What are you doing?
    ~ speaker = "annoying"
    I don't know.
    Something we don't want you for.
    ~ speaker = "replacement"
    Listen, I don't want to cause trouble.
    Do whatever you want.
    I unlocked the elevator for you.
}

=== accept ===
~ speaker = "annoying"
Really?
I thought we had a connection man!
~ speaker = "replacement"
I'm sorry.
I'll...
~ speaker = "annoying"
Whatever!
Bye!
~ remove_from_team("annoying")
~ speaker = "replacement"
I shouldn't have suggested it.
But I guess I can't back up now.
~ add_to_team("replacement")
~ improve_morale()
-> END

=== refuse ===
~ speaker = "printert"
~ hurt_morale()
:(
~ speaker = "annoying"
Oh, you better believe it!
You ain't getting a single dolar!
~ speaker = "replacement"
Well, I...
...
What?
Whatever.
I hope you have fun doing whatever it is you're doing.
~ speaker = "annoying"
Oh, we will.
~ replacement_spoke = true
-> END

=== repeat ===
~ speaker = "replacement"
I hope you succeed in your dreams!
-> END 