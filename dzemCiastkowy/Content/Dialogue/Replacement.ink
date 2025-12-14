VAR speaker = "replacement"
VAR printert = true
VAR annoying = false
VAR to_export = "printert;annoying"

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
Hello...

{printert:
    -> printert_proposition
}

Hello, who are you.
{annoying:
    Oh, and...
    She...
    ~ speaker = "annoying"
    A similar plesure to meet you.
    ~ speaker = "replacement"
    What are you doing?
    ~ speaker = "annoying"
    I don't know.
    Something we don't want you for.
    ~ speaker = "replacement"
    Listen, I don't want to cause trouble.
    Do whatever you want.
    I unlocked the elevator for you.
    -> END
}

* [I am here to steal things]
* [I am here to cause chaos]
- Sounds fun.
Can I join.
* [Sure] -> accept
* [No] -> refuse
-> END


=== printert_proposition ===
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
{annoying:
    -> printert_annoying
}

* [(Let her join)] -> accept
* [(Don't)] -> refuse

=== printert_annoying ===
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
-> END

=== accept ===
{annoying:
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
}
{not annoying:
    ~speaker = "replacement"
    Awesome.
}
~ add_to_team("replacement")
~ improve_morale()
-> END

=== refuse ===
{printert:
    ~ speaker = "printert"
    ~ hurt_morale()
    :(
}
{annoying:
    ~ speaker = "annoying"
    Oh, you better believe it!
    You ain't getting a single dolar!
}
~ speaker = "replacement"
Well, I...
{annoying:
    ...
    What?
    Whatever.
}
I hope you have fun doing whatever it is you're doing.
{annoying:
    ~ speaker = "annoying"
    Oh, we will.
}
-> END