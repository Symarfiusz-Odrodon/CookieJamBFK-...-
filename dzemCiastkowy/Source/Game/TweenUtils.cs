using System;

namespace Game;

public static partial class TweenUtils
{
    public static float SmoothTween(float t, float pow = 2)
    {
        if (t < 0.5f) return MathF.Pow(2f*t, pow)/2f;
        return 1 - MathF.Pow(2f*(1-t), pow)/2f;
    }
}