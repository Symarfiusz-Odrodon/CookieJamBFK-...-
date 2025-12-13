using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

public class PlayGame : Script
{
    public SceneReference gameScene;
    private bool isLoading = false;

    public override void OnUpdate()
    {
        if (isLoading) return;

        var uiControl = Actor as UIControl;
        if (uiControl != null && uiControl.Get<Button>().IsPressed)
        {
            if (gameScene == null) return;

            isLoading = true;

            Scripting.RunOnUpdate(() =>
            {
                Level.UnloadAllScenes();
                Level.LoadScene(gameScene);
            });
        }
    }
}