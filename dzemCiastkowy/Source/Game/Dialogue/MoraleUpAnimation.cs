using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game.Dialogue;

public class MoraleUpAnimation : Script
{
    [Header("Controls")]
    public UIControl backgroundControl;    
    public UIControl arrowControl;    
    public UIControl textControl;

    [Header("Settings")]
    public float startDuration = 0.3f;
    public float holdDuration = 1.5f;
    public float endDuration = 0.3f;

    Vector3 backgroundPos;
    Vector3 arrowPos;
    Vector3 textPos;

    public override void OnStart()
    {
        backgroundPos = backgroundControl.LocalPosition;
        arrowPos = arrowControl.LocalPosition;
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
        var backgroundLerp = SmoothTo(t, 0f, 0.7f);
        var arrowLerp = SmoothTo(t, 0.2f, 0.8f);
        var textLerp = SmoothTo(t, 0.4f, 1f);

        backgroundControl.LocalPosition = backgroundPos + Vector3.Lerp(new(100f, 20f, 0f), new(0f, 0f, 0f), backgroundLerp);
        if (backgroundControl.Control is Image background)
        {
            background.Color = Color.Lerp(Color.Transparent, Color.White, backgroundLerp);
            background.MouseOverColor = background.Color;
        }
        
        arrowControl.LocalPosition = arrowPos + Vector3.Lerp(new(-80f, 80f, 0f), new(0f, 0f, 0f), arrowLerp);
        if (arrowControl.Control is Image arrow)
        {
            arrow.Color = Color.Lerp(Color.Transparent, Color.White, arrowLerp);
            arrow.MouseOverColor = arrow.Color;
        }

        textControl.LocalPosition = textPos + Vector3.Lerp(new(5f, 20f, 0f), new(0f, 0f, 0f), textLerp);
        if (textControl.Control is Image text)
        {
            text.Color = Color.Lerp(Color.Transparent, Color.White, textLerp);
            text.MouseOverColor = text.Color;
        }
    }    

    private void LerpOut(float t)
    {
        var color = Color.Lerp(Color.White, Color.Transparent, Math.Clamp(t, 0f, 1f));

        if (backgroundControl.Control is Image background) { background.Color = color; background.MouseOverColor = color; }
        if (arrowControl.Control is Image arrow) { arrow.Color = color; arrow.MouseOverColor = color; }
        if (textControl.Control is Image text) { text.Color = color; text.MouseOverColor = color; }
    }
}
