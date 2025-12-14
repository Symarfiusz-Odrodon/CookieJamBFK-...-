using dzemCiastkowy;
using FlaxEngine;

public class ElevatorDoor : Script
{
    public Actor DoorRight;
    public Actor DoorLeft;

    public Vector3 openPosLeft;
    public Vector3 openPosRight;

    public AudioClip elevatorDoorSFX;
    public AudioSource elevatordoorsource;

    private Vector3 closedPosLeft;
    private Vector3 closedPosRight;

    private bool opening;
    private float openTime;
    private readonly float duration = 0.5f;

    private bool openc = false;

    private bool tutPass = false;

    public override void OnStart()
    {
        closedPosLeft = DoorLeft.Position;
        closedPosRight = DoorRight.Position;
        openPosLeft = closedPosLeft + openPosLeft;
        openPosRight = closedPosRight + openPosRight;
    }

    public void OnOpenClose()
    {
        if(!opening)
        {
            if(tutPass)
            {
                Actor.Scene.FindScript<MusicController>().StartStopElevatorMusic();
            }
            else
            {
                tutPass = true;
            }
            elevatordoorsource.Clip = elevatorDoorSFX;
            elevatordoorsource.Play();
            opening = true;
            openTime = 0f;
        }
    }

    public override void OnUpdate()
    {
        if (!opening)
            return;

        openTime += Time.DeltaTime;
        float alpha = Mathf.Clamp(openTime / duration, 0, 1);
        
        if(!openc)
        {
            DoorRight.Position = Vector3.Lerp(closedPosRight, openPosRight, alpha);
            DoorLeft.Position = Vector3.Lerp(closedPosLeft, openPosLeft, alpha);
        }
        else
        {
            DoorRight.Position = Vector3.Lerp(openPosRight, closedPosRight, alpha);
            DoorLeft.Position = Vector3.Lerp(openPosLeft, closedPosLeft, alpha);
        }

        if (alpha >= 1f)
        {
            opening = false;
            openc = !openc;
        }
    }
}
