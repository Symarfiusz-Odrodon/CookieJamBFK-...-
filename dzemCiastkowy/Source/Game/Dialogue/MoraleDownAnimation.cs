using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game.Dialogue;

public class MoraleDownAnimation : Script
{
    [Header("Controls")]
    public UIControl leftControl;    
    public UIControl rightControl;    
    public UIControl textControl;

    [Header("Settings")]
    public float startDuration = 1f;
    public float holdDuration = 1.5f;
    public float endDuration = 0.3f;

    Vector3 leftPos;
    Vector3 rightPos;
    Vector3 textPos;

    public override void OnStart()
    {
        leftPos = leftControl.LocalPosition;
        rightPos = rightControl.LocalPosition;
        textPos = textControl.LocalPosition;

        LerpOut(1f);
    }

    bool _animate = false;
    float _time = 0;
    public override void OnUpdate()
    {
        if (!_animate) return;

        if (_time < startDuration)
            Lerp(_time / startDuration);
        else if (_time < startDuration + holdDuration)
            Lerp(1f);
        else if (_time < startDuration + holdDuration + endDuration)
            LerpOut((_time - startDuration - holdDuration) / endDuration);
        else
        {
            LerpOut(1f);
            _animate = false;
        }

        _time += Time.DeltaTime;
    }

    private static float SmoothTo(float t, float start, float end)
    {
        var len = end - start;
        return TweenUtils.SmoothTween(Math.Clamp((t - start) / len, 0f, 1f));
    }

    public void Animate()
    {
        _animate = true;
        _time = 0f;
    }

    private void Lerp(float t)
    {
        var leftLerp1 = SmoothTo(t, 0f, 0.4f);
        var rightLerp1 = SmoothTo(t, 0f, 0.4f);
        var leftLerp2 = SmoothTo(t, 0.4f, 0.8f);
        var rightLerp2 = SmoothTo(t, 0.4f, 0.8f);
        var textLerp = SmoothTo(t, 0.6f, 1f);

        leftControl.LocalPosition = leftPos + Vector3.Lerp(new(0f, 0f, 0f), new(-100f, 20f, 0f), leftLerp2);
        if (leftControl.Control is Image left)
        {
            left.Rotation = float.Lerp(0f, -30f, leftLerp2);
            left.Color = Color.Lerp(Color.Transparent, Color.White, leftLerp1);
            left.MouseOverColor = left.Color;
        }
        
        rightControl.LocalPosition = rightPos + Vector3.Lerp(new(0f, 0f, 0f), new(100f, 20f, 0f), rightLerp2);
        if (rightControl.Control is Image right)
        {
            right.Rotation = float.Lerp(0f, 30f, rightLerp2);
            right.Color = Color.Lerp(Color.Transparent, Color.White, rightLerp1);
            right.MouseOverColor = right.Color;
        }

        textControl.LocalPosition = textPos + Vector3.Lerp(new(0f, 20f, 0f), new(0f, 0f, 0f), textLerp);
        if (textControl.Control is Image text)
        {
            text.Color = Color.Lerp(Color.Transparent, Color.White, textLerp);
            text.MouseOverColor = text.Color;
        }
    }    

    private void LerpOut(float t)
    {
        var color = Color.Lerp(Color.White, Color.Transparent, Math.Clamp(t, 0f, 1f));

        if (leftControl.Control is Image left) { left.Color = color; left.MouseOverColor = color; }
        if (rightControl.Control is Image right) { right.Color = color; right.MouseOverColor = color; }
        if (textControl.Control is Image text) { text.Color = color; text.MouseOverColor = color; }
    }
}
