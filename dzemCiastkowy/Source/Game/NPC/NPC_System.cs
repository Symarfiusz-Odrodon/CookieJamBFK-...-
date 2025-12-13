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

    public UIControl[] NPC_Background = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_Character = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_Frame = new UIControl[NPC_LIMIT];

    public UIControl[] NPC_HPProgress = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_HPFrame = new UIControl[NPC_LIMIT];

    public UIControl[] NPC_APProgress = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_APFrame = new UIControl[NPC_LIMIT];

    public UIControl[] NPC_Action1 = new UIControl[NPC_LIMIT];
    public UIControl[] NPC_Action2 = new UIControl[NPC_LIMIT];

    public SpriteRender[] enemySprites = new SpriteRender[NPC_LIMIT];
    public UIControl[] enemyHPProgress = new UIControl[NPC_LIMIT];
    public UIControl[] enemyAPProgress = new UIControl[NPC_LIMIT];

    public List<FriendlyNpcInstance> Npcs { get; } = [];
    public List<EnemyNpcInstance> Enemies { get; } = [];

    public override void OnStart()
    {
        if (Instance == null)
            Instance = this;
    }

    public override void OnUpdate()
    {
        UpdateUi();
    }

    [DebugCommand]
    public static void AddFriendNpcCommand(string id)
    {
        if (Instance == null)
        {
            Debug.LogError("No instance assigned :(");
            return;
        }

        Instance.AddFriendNpc(id);
    }


    public void AddFriendNpc(string id)
    {
        if (Npcs.Count >= NPC_LIMIT)
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

    [DebugCommand]
    public static void AddEnemyNpcCommand(string id)
    {
        if (Instance == null)
        {
            Debug.LogError("No instance assigned :(");
            return;
        }

        Instance.AddEnemyNpc(id);
    }

    public void AddEnemyNpc(string id)
    {
        if (Enemies.Count >= NPC_LIMIT)
        {
            Debug.LogError("Can't add enemy npc, npc limit already reached!");
            return;
        }

        var data = database.Instance.GetNpcById(id);

        if (data == null)
        {
            Debug.LogError($"Npc of id '{id}' does not exist!");
            return;
        }

        Enemies.Add(new EnemyNpcInstance(data));
    }

    private void UpdateUi()
    {
        for (int i = 0; i < Npcs.Count; i++)
            UpdateUiForCharacter(i, Npcs[i]);
        
        for (int i = Npcs.Count; i < NPC_LIMIT; i++)
            UpdateUiForCharacter(i, null);
    

        void UpdateUiForCharacter(int i, FriendlyNpcInstance npc)
        {
            NPC_Character[i].Get<Image>().Brush = npc?.Data.headTexture == null ? null : new TextureBrush(npc?.Data.headTexture);
            NPC_Background[i].Get<Image>().Visible = npc != null;
            NPC_Frame[i].Get<Image>().Visible = npc != null;


            var HPProgress = NPC_HPProgress[i].Get<ProgressBar>();
            HPProgress.Value = npc == null || npc.maxHealth == 0 ? HPProgress.Maximum : (float)npc.health / npc.maxHealth;
            HPProgress.Visible = npc != null;

            var APProgress = NPC_APProgress[i].Get<ProgressBar>();
            APProgress.Value = npc?.actionPoints ?? APProgress.Maximum;
            APProgress.Visible = npc != null;

            var action1 = NPC_Action1[i].Get<Button>();
            action1.Visible = npc != null;
            action1.Enabled = npc?.actionPoints >= 1;
            action1.Text = npc?.Data.friendAction1Name;

            var action2 = NPC_Action2[i].Get<Button>();
            action2.Visible = npc != null;
            action2.Enabled = npc?.actionPoints >= 1;
            action2.Text = npc?.Data.friendAction2Name;
        }

        //Update enemy sprites
        for (int i = 0; i < 3; i++)
        {
            if (i < Enemies.Count && Enemies[i] != null && Enemies[i].Data != null)
            {
                enemySprites[i].Image = Enemies[i]?.Data.worldTexture;
                var HPProgress = enemyHPProgress[i].Get<ProgressBar>();
                HPProgress.Visible = true;
                HPProgress.Value = Enemies[i].maxHealth == 0 ? HPProgress.Maximum : (float)Enemies[i].health / Enemies[i].maxHealth;

                var APProgress = enemyAPProgress[i].Get<ProgressBar>();
                APProgress.Visible = true;
                APProgress.Value = Enemies[i].actionPoints;
            }
            else
            {
                enemySprites[i].Image = null;
                enemyHPProgress[i].Get<ProgressBar>().Visible = false;
                enemyAPProgress[i].Get<ProgressBar>().Visible = false;
            }
        }
    }
}
