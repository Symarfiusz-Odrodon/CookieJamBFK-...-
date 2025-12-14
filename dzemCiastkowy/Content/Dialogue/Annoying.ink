EXTERNAL add_to_team(id)

VAR annoying_admit_thief = false
VAR speaker = "annoying"

VAR to_export="annoying_admit_thief"

-> intro

=== intro ===
Who are you?
Wait, you have such a beautiful necklace.
* [Um, thanks?]
    -> why_you_here
* [...]
    -> why_you_here
* [What?]
    -> why_you_here

=== why_you_here ===
Okay, nevermind about that, who are you?
You're not supposed to be here.
* [I am here to steal things.] -> admit_to_stealing
* [I'm here to do inspection.]
    I don't buy it.
    Inspection of what?
    You leave us be, we do our jobs.
    Are you a thief?
    * * [Yes] -> admit_to_stealing
    * * [No]
        I'm pretty sure you are a thief.
        So, are you a thief?
        * * * [Yes] -> admit_to_stealing
        * * * [No]
            Are you REALLY sure?
            * * * * [I am a thief] -> admit_to_stealing
            * * * * [I am not a thief] -> do_not_admit_to_stealing

=== admit_to_stealing ===
~ annoying_admit_thief = true
Oh, that makes sense.
You know what, that sounds exciting.
I always wanted to do a heist.
Because I assume that is what we are doing.
And you need my keycard, if you want to go any further...
so...
-> offer

=== do_not_admit_to_stealing ===
Okay, then.
So I'm not really sure why you are here.
Nor what exactly do you want to do.
But I know that you will need a keycard to go any further.
A keycard I have.
So...
-> offer

=== offer ===
Can I join you?
I mean, it's not like you can decline.
Like, what are you gonna do?
Eventually you will run into someone who will try to stop you.
And you will have to have allies by your side.
And I'm like one of the most liked people here.
So, what do you say?
* [Okay] -> end
* [I don't have a choise] -> end

=== end ===
Perfect.
Thank you.
~ add_to_team("annoying")
-> END

