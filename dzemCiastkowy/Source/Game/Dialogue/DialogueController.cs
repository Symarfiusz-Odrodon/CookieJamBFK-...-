using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxInk;
using FlaxEngine.GUI;
using Game.NPC;
using System.Linq;
using dzemCiastkowy;

namespace Game.Dialogue;

public class DialogueController : Script
{
    // --- CONSTANTS ---
    public const string VAR_NAME_SPEAKER = "speaker";
    public const string VAR_NAME_CHARACTER_LEFT = "char_left";
    public const string VAR_NAME_CHARACTER_RIGHT = "char_right";
    public const string VAR_NAME_TO_EXPORT = "to_export";
    public const string FUNC_NAME_ADD_TO_TEAM = "add_to_team";
    public const string FUNC_NAME_REMOVE_FROM_TEAM = "remove_from_team";
    public const string FUNC_NAME_IMPROVE_MORALE = "improve_morale";
    public const string FUNC_NAME_HURT_MORALE = "hurt_morale";

    // --- CONFIGURATION FIELDS ---
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

    // --- TEXT REVEAL FIELDS ---
    [Header("Text Reveal")]
    [Tooltip("The time (in seconds) it takes to reveal one character.")]
    public float TypeSpeed = 0.05f;

    private string _currentLineText;     // Stores the full text of the current line
    private float _revealTimer;          // Tracks time for revelation
    private int _charactersRevealed;     // How many characters are currently visible

    private MusicController musicController;
    
    // --- STATE & EVENTS ---
    public bool StoryActive { get; private set; }

    public event Action OnStoryFinished;
    public event Action OnStoryStarted;

    // --- ENGINE OVERRIDES ---
    public override void OnStart()
    {
        musicController = Actor.Scene.FindScript<MusicController>();

        // Register click events to option buttons
        for (int i = 0; i < optionControls.Length; i++)
        {
            if (optionControls[i] != null && optionControls[i].Control is Button btn)
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
            // 1. Dialogue Line Setup
            runner.GetVariable(VAR_NAME_SPEAKER, out string speakerId);
            HideOptions();

            // --- TEXT REVEAL SETUP ---
            _currentLineText = line.Text;
            _revealTimer = 0f;
            _charactersRevealed = 0;

            // Set the text control to EMPTY initially, OnUpdate will fill it
            if (textControl?.Control is RichTextBox textBox)
                textBox.Text = string.Empty;
            else if (textControl?.Control is Label label)
                label.Text = string.Empty;
            // --- END TEXT REVEAL SETUP ---

            // 2. Speaker Label Update
            if (speakerLabelControl?.Control is Label speakerLabel)
                speakerLabel.Text = npcDatabase.Instance.GetNpcById(speakerId)?.name ?? speakerId;

            // 3. Character Image Update (Left)
            if (characterLeftImageControl?.Control is Image charLeft)
            {
                runner.GetVariable(VAR_NAME_CHARACTER_LEFT, out string charLeftId);
                charLeft.Brush = new TextureBrush(npcDatabase.Instance.GetNpcById(charLeftId)?.dialogueTexture);
            }

            // 4. Character Image Update (Right)
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

        // Input handler registration
        progressDialogueInput.Pressed += () =>
        {
            if (StoryActive)
                ContinueDialogue();
        };

        // NPC System event handler
        if (npcSystem != null && npcDatabase.Instance != null)
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

    // --- TEXT REVEAL PROGRESSION ---
    public override void OnUpdate()
    {
        // Only run if a story is active and we haven't revealed the whole line yet
        if (StoryActive && _currentLineText != null && _charactersRevealed < _currentLineText.Length)
        {
            _revealTimer += Time.DeltaTime;

            // Calculate how many characters should be revealed based on time and speed
            int charactersToReveal = (int)(_revealTimer / TypeSpeed);

            // Check if we have new characters to show
            if (charactersToReveal > _charactersRevealed)
            {
                if(!musicController.jibber)
                {
                    musicController?.StartStopJibberSpeech();
                }
                _charactersRevealed = Math.Min(charactersToReveal, _currentLineText.Length);

                // Update the UI text to show the substring
                string revealedText = _currentLineText.Substring(0, _charactersRevealed);

                if (textControl?.Control is RichTextBox textBox)
                    textBox.Text = revealedText;
                else if (textControl?.Control is Label label)
                    label.Text = revealedText;
            }
            if(_currentLineText.Length <= _charactersRevealed)
            {
                musicController?.StartStopJibberSpeech();
            }
        }
    }
    // --- END TEXT REVEAL PROGRESSION ---


    // --- DIALOGUE FLOW METHODS ---
    public void SelectOption(int index)
    {
        if (runner == null || !runner.IsStoryActive) return;

        runner.ChooseChoice(index);
        HideOptions();
        ContinueDialogue();
    }

    public void StartStory(JsonAssetReference<InkStory> story)
    {
        if (rootControl == null || runner == null) return;

        rootControl.IsActive = true;
        HideOptions();
        StoryActive = true;
        runner.StartDialogue(story);

        // Bind External Functions
        runner.BindExternalFunction<string>(FUNC_NAME_ADD_TO_TEAM, id => npcSystem?.AddFriendNpc(id));
        runner.BindExternalFunction<string>(FUNC_NAME_REMOVE_FROM_TEAM, id =>
        {
            if (npcSystem == null) return;

            var items = npcSystem.Npcs.Where(x => x.Data.id == id).ToList();

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

        OnStoryStarted?.Invoke();
    }

    public void ContinueDialogue()
    {
        // 1. Handle Null reference (Fixes CS0120)
        if (optionsRootControl != null && optionsRootControl.IsActive) return;

        // 2. Text Skip Logic (If text is typing, skip it)
        if (_currentLineText != null && _charactersRevealed < _currentLineText.Length)
        {
            // Skip animation: reveal the full text immediately
            _charactersRevealed = _currentLineText.Length;
            musicController?.StartStopJibberSpeech();

            if (textControl?.Control is RichTextBox textBox)
                textBox.Text = _currentLineText;
            else if (textControl?.Control is Label label)
                label.Text = _currentLineText;

            // Do NOT proceed to the next line yet
            return;
        }

        // 3. Continue to the next line (only if text is fully revealed)
        if (runner != null)
        {
            runner.ContinueDialogue();
        }
    }

    private void ShowOptions()
    {
        if (optionsRootControl == null) return;

        optionsRootControl.IsActive = true;
        var len = Math.Min(runner.CurrentChoices.Count, optionControls.Length);
        for (int i = 0; i < len; i++)
        {
            if (optionControls[i] == null) continue;

            optionControls[i].IsActive = true;
            if (optionControls[i].Control is Button btn)
                btn.Text = runner.CurrentChoices[i].Text;
        }

        for (int i = len; i < optionControls.Length; i++)
        {
            if (optionControls[i] == null) continue;
            optionControls[i].IsActive = false;
        }
    }

    private void FinishStory()
    {
        CopyVarsFromStory();
        HideAllElements();
        StoryActive = false;

        runner.StopDialogue();

        // Clear the internal line state
        _currentLineText = null;

        OnStoryFinished?.Invoke();
    }

    void HideOptions()
    {
        if (optionsRootControl == null) return;
        optionsRootControl.IsActive = false;
    }

    private void HideAllElements()
    {
        if (rootControl != null)
            rootControl.IsActive = false;

        HideOptions();
    }

    #region Variables
    private Dictionary<string, object> _variables = new Dictionary<string, object>();

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
        if (runner != null && runner.GetVariable(VAR_NAME_TO_EXPORT, out string toExport))
            return toExport.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

        return Array.Empty<string>();
    }

    private void CopyVarsToStory()
    {
        if (runner == null) return;

        var vars = GetVarsToExport();

        foreach (var item in vars)
            if (_variables.TryGetValue(item, out var val))
                runner.SetVariable(item, val);
    }

    private void CopyVarsFromStory()
    {
        if (runner == null) return;

        var vars = GetVarsToExport();

        foreach (var item in vars)
            if (runner.GetVariable(item, out object val))
                SetValue(item, val);
    }
    #endregion
}