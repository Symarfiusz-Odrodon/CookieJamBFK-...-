VAR speaker = "annoying"
VAR annoying = false
VAR printert = false

VAR to_export="annoying;printert"

-> intro

=== intro ===
{annoying:
    ~ speaker = "annoying"
    And what are you supposed to be?
    ...
    Oh, I guess they don't talk.
    Ookay guy.
    ...
    Just trying to start a friendly conversation.
    {printert:
        For the love of, just start the fight already!
    }
- else:
    ~ speaker = "replacement"
    Oh, I know these guys.
    ...
    They are bad.
    ...
    Sorry, we will have to fight through them if we want to get anywhere.
    {printert:
        ~ speaker = "printert"
        As you command!
        ~ speaker = "replacement"
        Jesus, are you in love with me or something?
        ~ speaker = "printert"
        What?
        ~ speaker = "replacement"
        I mean, you keep saying things like that, it's just...
        It's weird.
        Like you are obsessed with me or something.
        ~ speaker = "printert"
        I'm not!
        You know, I have a wife!
        ~ speaker = "replacement"
        ...
        You have a wife?
        ~ speaker = "printert"
        No, that was a lie.
        It's just that...
        Well...
        ...
        Okay, I'm starting to realize that I am in fact sounding, like I am in love.
        But just for your knowledge.
        I'm not.
        I think.
        I mean, I'm not.
        ~ speaker = "replacement"
        Just stop speaking.
        ~ speaker = "printert"
        It's just that...
        ...
        I now understand your point of view-
        and the situation in general-
        and all I want to say is...
        Noted.
        I'll... keep that in mind in the future.
        I apologize, if my actions made you uncomfortable.
        It won't happen again.
    }
}
-> END