using FlaxEngine;

public class ElevatorDoor : Script
{
    public Actor DoorRight;
    public Actor DoorLeft;

    public Vector3 OpenPosLeft;
    public Vector3 OpenPosRight;

    private Vector3 closedPosLeft;
    private Vector3 closedPosRight;

    private bool opening;
    private float openTime;
    private readonly float duration = 1.5f;

    public override void OnStart()
    {
        closedPosLeft = DoorLeft.Position;
        closedPosRight = DoorRight.Position;
        OpenPosLeft = closedPosLeft + OpenPosLeft;
        OpenPosRight = closedPosRight + OpenPosRight;
    }

    public void OnOpen()
    {
        opening = true;
        openTime = 0f;
    }

    public override void OnUpdate()
    {
        if (!opening)
            return;

        openTime += Time.DeltaTime;
        float alpha = Mathf.Clamp(openTime / duration, 0, 1);

        DoorRight.Position = Vector3.Lerp(closedPosRight, OpenPosRight, alpha);
        DoorLeft.Position = Vector3.Lerp(closedPosLeft, OpenPosLeft, alpha);

        if (alpha >= 1f)
            opening = false;
    }
}
