using FlaxEngine;
using Game.Managers;

namespace Game
{
    public class ClickButton : Script
    {
        public Camera Camera; // Assign your camera in the inspector

        public Vector3 movement = new Vector3(0, 0.1f, 0);

        public bool unlocked = false;

        public StaticModel model;

        public Material defaultMat;
        public Material clickMat;

        private Vector3 positionOriginal;


        private bool isAnimating;
        private float animTime;
        private readonly float outDuration = 0.12f; // time to move out
        private readonly float backDuration = 0.12f; // time to move back

        public override void OnStart()
        {
            positionOriginal = Actor.Position;
        }

        public override void OnUpdate()
        {
            var targetmat = defaultMat;

            if (unlocked)
            {
                targetmat = clickMat;
            }

            if (model.GetMaterial(0) != targetmat)
            {
                model.SetMaterial(0, targetmat);
            }

            if (!isAnimating)
            {
                if (Input.GetMouseButtonDown(MouseButton.Left))
                    CheckClick();
            }
            else
            {
                // progress animation
                animTime += Time.DeltaTime;

                if (animTime < outDuration)
                {
                    // moving out
                    float a = animTime / outDuration;
                    float smooth = a * a * (3f - 2f * a); // smoothstep
                    Actor.Position = Vector3.Lerp(positionOriginal, positionOriginal + movement, smooth);
                }
                else if (animTime < outDuration + backDuration)
                {
                    // moving back
                    float t = (animTime - outDuration) / backDuration;
                    float smooth = t * t * (3f - 2f * t); // smoothstep
                    Actor.Position = Vector3.Lerp(positionOriginal + movement, positionOriginal, smooth);
                }
                else
                {
                    // finish
                    Actor.Position = positionOriginal;
                    isAnimating = false;
                }
            }
        }

        private void CheckClick()
        {
            if (Camera == null)
                return;

            Debug.Log("checkclick");
            Ray ray = Camera.ConvertMouseToRay(Input.MousePosition);

            if (Physics.RayCast(ray.Position, ray.Direction, out RayCastHit hitInfo, 1000f))
            {
                var myChildCollider = Actor.GetChild<BoxCollider>();
                bool hitMe = false;

                if (hitInfo.Collider != null )
                {
                    if (myChildCollider != null && hitInfo.Collider == myChildCollider)
                        hitMe = true;
                    else if (hitInfo.Collider == Actor.GetChild<BoxCollider>())
                        hitMe = true;
                }

                if (hitMe & unlocked)
                {
                    OnClicked();
                }
            }
        }

        private void OnClicked()
        {
            Debug.Log("Clicked " + Actor.Name);

            if (!isAnimating)
            {
                unlocked = false;
                isAnimating = true;
                animTime = 0f;
                Actor.Scene.FindScript<ElevatorDoor>().OnOpenClose();
                Actor.Scene.FindScript<ElevatorManager>().GoToNextFloor();
            }
        }
    }
}
