VAR speaker = "bio"
VAR to_export = "flash_drive;team_count;annoying;printert;replacement;hat;pc;lore_dropped"
VAR flash_drive = false
VAR lore_dropped = false
VAR team_count = 0

VAR annoying = false
VAR printert = false
VAR replacement = false
VAR hat = false
VAR pc = false

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
Oh my.
You're not a...
How did you...
...
Wow.
I have never seen an actual human in my entire life.
What are you all doing here?

* [Trying to steal something] -> steal
* [Trying to cause havoc] -> havoc

-> END

=== steal ===
Okay, but what exactly?
There isn't anything of value in these buildings.
They are simply used for housing us and making us do...
-> necklace

=== havoc ===
Okay, sure.
I get it.
Tensions are high.
You don't have any work to do.
But, do you have a bigger plan, or...
-> necklace

=== necklace ===

-> END
Wait.
What is this necklace?

{flash_drive:
    It seems really familiar.
    -> forgof
}

Is that the...
...
Oh no.
You don't know what you are doing.
* [I know exactly what I am doing]
* [You can't stop me]
- Do you even know what she is capable of?
* [I am fully aware]
* [I don't need to know]
- ...
{team_count == 0 or hat:
-> alone
}

{
    - printert:
        ~ speaker = "printert"
        Wait, what's going on?
    - replacement:
        ~ speaker = "replacement"
        Wait, what is happening?
    - annoying:
        ~ speaker = "annoying"
        Wait, what the hell is going on?
    - pc:
        ~ speaker = "pc"
        What seems to be the reason of distress
        ache
        affection. #italic
}

-> lore_drop

=== forgof ===
No matter.
I see you are {team_count > 0: all} exploring the building.
That sounds very interesting.
...
Could I join you?
{team_count == 3:
    -> too_many_people
}

* [Sure]
    Awesome!
    I can't wait to sit there and observe you as you move your every mussle while standing still in an enclosed metal cube.
    This is exciting!
    ~ add_to_team("bio")
    -> END
* [Sorry, no]
    Oh, yeah, I understand.
    Well then, good luck on whatever it is you are doing!
    -> END

=== too_many_people ===

Oh wait, it seems like you are already reaching the maximum elevator capacity.
...
:(
I was gonna observe how your body's mussles work when standing stile in an enclosed metal cube :(((((
...
{replacement:
    ~ speaker = "replacement"
    You are weird.
}
{pc:
    -> replace_pc
}

= replace_pc
~ speaker = "pc"
Hold on.
If it would be of assistance, I could leave the elevator here and let her take my place.
* [Sure]
    My pleasure.
    ~ speaker = "bio"
    Oh, thank you!
    ~ remove_from_team("pc")
    ~ add_to_team("bio")
    -> END
* [No]
    If that's what you want.
    -> END

-> END

=== lore_drop ===
~ lore_dropped = true
~ speaker = "bio"
Do you know what's on the top floor?
That's where the life machine is located.
The thing that created all of us in this building.

{hat:
    ~speaker = "hat"
    Even me?
    ~speaker = "bio"
    No, you're just...
    You're just incredibly dedicated for some reason.
}

That necklace on his...
well...
neck.
It was also given life.
But only temporarily.
It turned out that the stone that's embeded in it has very peculiar properties.
Properties, which Hanna - that being the person that was created from the amulet - could utilize.
This included the ability to manipulate matter.
Which she utilized to destroy an entire building.
And then she escaped.
And it would appear, like she found you.
Listen.
Whatever you are doing, is incredibly dangerous.
You might not come out of it alive.
This is beyond just causing havoc.
This is incredibly dangerous.

{annoying:
    ~ speaker = "annoying"
    Have you been just lying to us this whole time?
    You don't even care if we make it out alive, do you?
    ~ hurt_morale()
}
{printert:
    ~ speaker = "printert"
    I...
    ...
    ~ hurt_morale()
}
{replacement:
    ~ speaker = "replacement"
    Okay, this is... getting a bit out of hand.
    ~ hurt_morale()
}
{hat:
    ~ speaker = "hat"
    ...
    I'm just here for a gray highlighter
}
{pc and hat:
    ~ speaker = "pc"
    I do not have a problem with this as I'm here to assist.
    {
    - team_count == 3:
        However.
        Since we are applying negative status effects.
        I would like to use this moment to emphasize how much I dislike this hat person that is in this team.
        ~ hurt_morale()
    - else:
        But I do have a problem with the hat person being here, which i would just like to point out again.
        ~ hurt_morale()
    }
}

~ speaker = "bio"

I am not going to help you.
But I know I can't stop you.
The elevator should now be unlocked.

-> END

=== alone ===
{hat:
    ~ speaker = "hat"
    So when will we find those gray markers?
    ~ speaker = "bio"
    ...
}

* [Just give me the keycard]
    ...
    I see that there is no way to stop what's to come.
    But when things go bad, I want you to remember that I warned you.
    -> END
