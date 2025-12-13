using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxInk;
using FlaxEngine.GUI;
using Game.NPC;

namespace Game.Dialogue;

public class DialogueController : Script
{
    private const string _SPEAKER_VAR_NAME = "speaker";

    [Header("Database")]
    public JsonAssetReference<NpcDatabase> npcDatabase;

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
    [Space(16)]
    [Tooltip("A ui control containing option buttons.")]
    public UIControl optionsRootControl;
    [Tooltip("List of buttons with labels as their children representing options.")]
    public UIControl[] optionControls;
    [Space(16)]
    [Tooltip("Label that displays the current speaking character.")]
    public UIControl speakerLabelControl;

    public bool StoryActive { get; private set; }

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

        runner.NewDialogueChoices += _ =>
        {
            ShowOptions();
        };

        runner.NewDialogueLine += line =>
        {
            runner.GetVariable(_SPEAKER_VAR_NAME, out string speakerId);
            HideOptions();
            if (textControl.Control is RichTextBox textBox)
                textBox.Text = line.Text;
            else if (textControl.Control is Label label)
                label.Text = line.Text;
            
            if (speakerLabelControl.Control is Label speakerLabel)
                speakerLabel.Text = npcDatabase.Instance.GetNpcById(speakerId)?.charName ?? speakerId;
        };

        runner.DialogueEnded += () =>
        {
            FinishStory();
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
        ContinueDialogue();
    }

    public void StartDialogue(JsonAssetReference<InkStory> story)
    {
        rootControl.IsActive = true;
        HideOptions();
        StoryActive = true;
        runner.StartDialogue(story);
        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (optionsRootControl.IsActive) return;

        runner.ContinueDialogue();
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

    private void FinishStory()
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
}
