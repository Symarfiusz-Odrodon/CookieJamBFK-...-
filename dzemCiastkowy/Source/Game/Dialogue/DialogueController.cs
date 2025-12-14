using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxInk;
using FlaxEngine.GUI;
using Game.NPC;
using System.Linq;

namespace Game.Dialogue;

public class DialogueController : Script
{
    public const string VAR_NAME_SPEAKER = "speaker";
    public const string VAR_NAME_CHARACTER_LEFT = "char_left";
    public const string VAR_NAME_CHARACTER_RIGHT = "char_right";
    public const string VAR_NAME_TO_EXPORT = "to_export";
    public const string FUNC_NAME_ADD_TO_TEAM = "add_to_team";
    public const string FUNC_NAME_REMOVE_FROM_TEAM = "remove_from_team";
    public const string FUNC_NAME_IMPROVE_MORALE = "improve_morale";
    public const string FUNC_NAME_HURT_MORALE = "hurt_morale";

    [Header("Database")]
    public JsonAssetReference<NpcDatabase> npcDatabase;

    [Header("Input")]
    public InputEvent progressDialogueInput = new();

    [Header("References")]
    public BetterDialogueRunner runner;
    public NPC_System npcSystem;
    public MoraleDownAnimation moraleDownAnimation;
    public MoraleUpAnimation moraleUpAnimation;

    [Space(16)]
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
    public UIControl characterLeftImageControl;
    public UIControl characterRightImageControl;

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
            runner.GetVariable(VAR_NAME_SPEAKER, out string speakerId);
            HideOptions();
            if (textControl?.Control is RichTextBox textBox)
                textBox.Text = line.Text;
            else if (textControl?.Control is Label label)
                label.Text = line.Text;
            
            if (speakerLabelControl?.Control is Label speakerLabel)
                speakerLabel.Text = npcDatabase.Instance.GetNpcById(speakerId)?.name ?? speakerId;
            
            if (characterLeftImageControl?.Control is Image charLeft)
            {
                runner.GetVariable(VAR_NAME_CHARACTER_LEFT, out string charLeftId);
                charLeft.Brush = new TextureBrush(npcDatabase.Instance.GetNpcById(charLeftId)?.dialogueTexture);
            }

            if (characterRightImageControl?.Control is Image charRight)
            {
                runner.GetVariable(VAR_NAME_CHARACTER_RIGHT, out string charRightId);
                charRight.Brush = new TextureBrush(npcDatabase.Instance.GetNpcById(charRightId)?.dialogueTexture);
            }
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

        if (npcSystem != null)
        {
            npcSystem.Npcs.CollectionChanged += (_, _) =>
            {
                foreach (var item in npcDatabase.Instance.npcs)
                    SetValue(item.Instance.id, npcSystem.Npcs.Any(x => x.Data.id == item.Instance.id));
            };
        }

        HideAllElements();

        if (startStory.Instance != null)
            StartStory(startStory);
    }

    public void SelectOption(int index)
    {
        if (!runner.IsStoryActive) return;
        runner.ChooseChoice(index);
        HideOptions();
        ContinueDialogue();
    }

    public void StartStory(JsonAssetReference<InkStory> story)
    {
        rootControl.IsActive = true;
        HideOptions();
        StoryActive = true;
        runner.StartDialogue(story);

        runner.BindExternalFunction<string>(FUNC_NAME_ADD_TO_TEAM, id => npcSystem?.AddEnemyNpc(id));
        runner.BindExternalFunction<string>(FUNC_NAME_REMOVE_FROM_TEAM, id => 
        {
            if (npcSystem == null) return;

            var items = npcSystem.Npcs.Where(x => x.Data.id == id)
                .ToList();

            foreach (var item in items)
                npcSystem.Npcs.Remove(item);
        });
        runner.BindExternalFunction(FUNC_NAME_HURT_MORALE, () => 
        {
            moraleDownAnimation?.Animate();
            npcSystem?.HurtMorale();
        });
        runner.BindExternalFunction(FUNC_NAME_IMPROVE_MORALE, () => 
        {
            moraleUpAnimation?.Animate();
            npcSystem?.ImproveMorale();
        });

        CopyVarsToStory();

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
            if (optionControls[i].Control is Button btn)
                btn.Text = runner.CurrentChoices[i].Text;
        }

        for (int i = len; i < optionControls.Length; i++)
            optionControls[i].IsActive = false;
    }

    private void FinishStory()
    {
        CopyVarsFromStory();
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

    #region Variables
    private Dictionary<string, object> _variables = [];

    public T GetValue<T>(string name)
    {
        if (_variables.TryGetValue(name, out var obj) && obj is T t)
            return t;
        
        return default;
    }

    public object GetValueObj(string name)
    {
        if (_variables.TryGetValue(name, out var obj))
            return obj;

        return null;
    }

    public void SetValue(string name, object value)
    {
        if (!_variables.ContainsKey(name))
        {
            _variables.Add(name, value);
            return;
        }

        _variables[name] = value;
    }

    private string[] GetVarsToExport()
    {
        if (runner.GetVariable(VAR_NAME_TO_EXPORT, out string toExport))
            return toExport.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

        return [];
    }

    private void CopyVarsToStory()
    {
        var vars = GetVarsToExport();

        foreach (var item in vars)
            if (_variables.TryGetValue(item, out var val))
                runner.SetVariable(item, val);
    }

    private void CopyVarsFromStory()
    {
        var vars = GetVarsToExport();

        foreach (var item in vars)
            if (runner.GetVariable(item, out object val))
                SetValue(item, val);
    }
    #endregion
}
