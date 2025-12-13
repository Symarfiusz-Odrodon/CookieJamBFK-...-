using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxInk;
using FlaxEngine.GUI;

namespace Game.Dialogue;

public class DialogueController : Script
{
    [Header("Input")]
    public InputEvent progressDialogueInput = new();

    [Header("References")]
    public DialogueRunner runner;
    public JsonAssetReference<InkStory> startStory;

    [Header("UI Controls")]
    [Tooltip("The root of the dialogue box ui.")]
    public Actor rootControl;
    [Tooltip("Label or RichTextBox that displays the dialogue lines.")]
    public UIControl textControl;
    [Tooltip("A ui control containing option buttons.")]
    public UIControl optionsRootControl;
    [Tooltip("List of buttons with labels as their children representing options.")]
    public UIControl[] optionControls;

    public bool StoryActive { get; private set; }

    private bool _storyAlmostEnded = false;
    private bool _lineRead;
    private Queue<string> _lines = [];

    public override void OnStart()
    {
        // Register click events to option buttons
        for (int i = 0; i < optionControls.Length; i++)
        {
            if (optionControls[i].Control is Button btn)
            {
                var index = i;
                btn.Clicked += () => SelectOption(index);
            }
        }

        runner.NewDialogueLine += line =>
        {
            _lineRead = true;
            _lines.Enqueue(line.Text);
        };

        runner.DialogueEnded += () =>
        {
            _storyAlmostEnded = true;
        };

        progressDialogueInput.Pressed += () =>
        {
            if (StoryActive)
                ContinueDialogue();
        };

        HideAllElements();

        if (startStory.Instance != null)
            StartDialogue(startStory);
    }

    public void SelectOption(int index)
    {
        if (!runner.IsStoryActive) return;
        runner.ChooseChoice(index);
        HideOptions();
        UpdateLines();
        ContinueDialogue();
    }

    public void StartDialogue(JsonAssetReference<InkStory> story)
    {
        _storyAlmostEnded = false;
        rootControl.IsActive = true;
        HideOptions();
        StoryActive = true;
        runner.StartDialogue(story);
        UpdateLines();
        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (optionsRootControl.IsActive) return;

        if (_lines.TryDequeue(out var line))
        {
            HideOptions();
            if (textControl.Control is RichTextBox textBox)
                textBox.Text = line;
            else if (textControl.Control is Label label)
                label.Text = line;
            
            if (_lines.Count > 0) return;
        }

        if (_storyAlmostEnded)
            FinishPlay();

        ShowOptions();
    }

    private void ShowOptions()
    {
        optionsRootControl.IsActive = true;
        var len = Math.Min(runner.CurrentChoices.Count, optionControls.Length);
        for (int i = 0; i < len; i++)
        {
            optionControls[i].IsActive = true;

            if ((optionControls[i].GetChild(0) as UIControl)?.Control is Label optionLabel)
            {
                optionLabel.Text.Value = runner.CurrentChoices[i].Text;
            }
        }

        for (int i = len; i < optionControls.Length; i++)
        {
            optionControls[i].IsActive = false;
        }
    }

    private void FinishPlay()
    {
        HideAllElements();
        StoryActive = false;
    }

    void HideOptions()
    {
        optionsRootControl.IsActive = false;
    }

    private void HideAllElements()
    {
        rootControl.IsActive = false;
        HideOptions();
    }

    void UpdateLines()
    {
        do
        {
            _lineRead = false;
            runner.ContinueDialogue();
        }
        while (_lineRead);
    }
}
