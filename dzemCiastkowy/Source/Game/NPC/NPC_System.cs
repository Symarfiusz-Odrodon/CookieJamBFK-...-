using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game.NPC;

public class NPC_System : Script
{
    public const int NPC_LIMIT = 3;

    public static NPC_System Instance { get; private set; }

    [Header("Database")]
    public JsonAssetReference<NpcDatabase> database;

    [Header("Textures")]
    public Texture backgroundTex, frameTex, HPUnderTex, HPFillTex, HPFrameTex, APUnderTex, APFillTex, APFrameTex;


    public UIControl[] NPC_Background = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_Character = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_Frame = new UIControl[NPC_LIMIT];

    public UIControl[] NPC_HPProgress = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_HPFrame = new UIControl[NPC_LIMIT];

    public UIControl[] NPC_APProgress = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_APFrame = new UIControl[NPC_LIMIT];

    public List<FriendlyNpcInstance> Npcs { get; } = [];

    public override void OnStart()
    {
        if (Instance != null)
            Instance = this;

        InitStaticUi();
    }

    public override void OnUpdate()
    {
        UpdateUi();
    }

    [DebugCommand]
    public static void AddNpcCommand(string id)
    {
        if (Instance == null)
        {
            Debug.LogError("No instance assigned :(");
            return;
        }

        Instance.AddNpc(id);
    }

    public void AddNpc(string id)
    {
        if (Npcs.Count < NPC_LIMIT)
        {
            Debug.LogError("Can't add friendly npc, npc limit already reached!");
            return;
        }

        var data = database.Instance.GetNpcById(id);

        if (data == null)
        {
            Debug.LogError($"Npc of id '{id}' does not exist!");
            return;
        }

        Npcs.Add(new FriendlyNpcInstance(data));
    }

    private void UpdateUi()
    {
        for (int i = 0; i < Npcs.Count; i++)
            UpdateUiForCharacter(i, Npcs[i]);
        
        for (int i = 0; i < NPC_LIMIT; i++)
            UpdateUiForCharacter(i, null);
    

        void UpdateUiForCharacter(int i, FriendlyNpcInstance npc)
        {
            NPC_Character[i].Get<Image>().Brush = npc?.Data.headTexture == null ? null : new TextureBrush(npc?.Data.headTexture);

            var HPProgress = NPC_HPProgress[i].Get<ProgressBar>();
            HPProgress.Value = npc == null || npc.maxHealth == 0 ? HPProgress.Maximum : (float)npc.health / npc.maxHealth;
            HPProgress.Visible = npc != null;

            var APProgress = NPC_APProgress[i].Get<ProgressBar>();
            APProgress.Value = npc?.actionPoints ?? APProgress.Maximum;
            APProgress.Visible = npc != null;
        }
    }

    private void InitStaticUi()
    {
        for (int i = 0; i < NPC_LIMIT; i++)
        {
            NPC_Background[i].Get<Image>().Brush = new TextureBrush(backgroundTex);
            NPC_Frame[i].Get<Image>().Brush = new TextureBrush(frameTex);

            var HPProgress = NPC_HPProgress[i].Get<ProgressBar>();
            HPProgress.BarBrush = new TextureBrush(HPFillTex);
            HPProgress.BackgroundBrush = new TextureBrush(HPUnderTex);
            NPC_HPFrame[i].Get<Image>().Brush = new TextureBrush(HPFrameTex);

            var APProgress = NPC_APProgress[i].Get<ProgressBar>();
            APProgress.BarBrush = new TextureBrush(APFillTex);
            APProgress.BackgroundBrush = new TextureBrush(APUnderTex);
            NPC_APFrame[i].Get<Image>().Brush = new TextureBrush(APFrameTex);
        }
    }
}
