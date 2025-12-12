using FlaxEngine;
using Game;

public class PlayerController : Script
{
    [Limit(0, 100), Tooltip("Camera movement speed factor")]
    public float MoveSpeed { get; set; } = 4;

    [Tooltip("Camera rotation smoothing factor")]
    public float CameraSmoothing { get; set; } = 20.0f;

    private ElevatorDoor doorController;

    private float pitch;
    private float yaw;

    public override void OnStart()
    {
        doorController = Actor.Scene.FindScript<ElevatorDoor>();
        var initialEulerAngles = Actor.Orientation.EulerAngles;
        pitch = initialEulerAngles.X;
        yaw = initialEulerAngles.Y;
    }

    public override void OnUpdate()
    {
        Screen.CursorVisible = false;
        Screen.CursorLock = CursorLockMode.Locked;

        var mouseDelta = new Float2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        pitch = Mathf.Clamp(pitch + mouseDelta.Y, -88, 88);
        yaw += mouseDelta.X;
    }

    public override void OnFixedUpdate()
    {
        var camTrans = Actor.Transform;
        var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);

        camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(pitch, yaw, 0), camFactor);

        var inputH = Input.GetAxis("Horizontal");
        var inputV = Input.GetAxis("Vertical");
        var move = new Vector3(inputH, 0.0f, inputV);
        move.Normalize();
        move = camTrans.TransformDirection(move);

        camTrans.Translation += move * MoveSpeed;

        Actor.Transform = camTrans;

        if(Input.GetKeyDown(KeyboardKeys.F))
        {
            Debug.Log("DebugDoorOpen");
            doorController.OnOpenClose();
        }


        if (Input.GetKeyDown(KeyboardKeys.G))
        {
            Debug.Log("DebugFloorUpdate");
            Actor.Scene.FindScript<FloorRenderer>().OnFloorUpdate(5);
        }
    }
}
