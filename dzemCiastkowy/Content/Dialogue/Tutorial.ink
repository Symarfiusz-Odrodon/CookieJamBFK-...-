VAR new_to_team = ""
VAR speaker = "tut"

-> intro

=== intro ===
Good morning, how can I help you?
    * [I work here.]
        ...
        No you don't
        Wait, are you trying to sneak in?
            * * [Yes?] -> intro_sneak_question_anwser
            * * [No?] -> intro_sneak_question_anwser
    * [I want to rob this place.]
        Oh...
        ...
        You know what sure.
        If someone REAL is trying to get into this building, then there's probably a good reason.
        -> help

=== intro_sneak_question_anwser ===
No matter, if someone REAL is trying to get into this building, then there's probably a good reason.
-> help

=== help ===
I can help you get to the elevator, but I only have access to the first few floors.
To be honest, I'm not sure what you will find in there.
I haven't been there for so long.
I'm not sure if anything even is there anymore.
Well, one way to find out.
~ new_to_team = "tut"
-> END