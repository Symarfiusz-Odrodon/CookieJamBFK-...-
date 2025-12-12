using System;
using FlaxEngine;

namespace Game
{
    public class FloorRenderer : Script
    {
        private int floor = 0;

        public override void OnStart()
        {
        }

        public void OnFloorUpdate(int floorUp)
        {
            floor += floorUp;

            var text = (TextRender)Actor;

            if (text != null)
            {
                text.Text = floor.ToString();
            }
            else
            {
                Debug.LogWarning("FloorRenderer: no TextRender component found on actor " + Actor.Name);
            }
        }
    }
}
