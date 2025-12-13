using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;



/// <summary>
/// NPC_System Script.
/// </summary>
namespace Game.NPC
{
    public class NPC_System : Script
    {
        public Texture backgroundTex, frameTex, HPUnderTex, HPFillTex, HPFrameTex, APUnderTex, APFillTex, APFrameTex;


        public UIControl[] NPC_Background = new UIControl[3];
        public UIControl[] NPC_Character = new UIControl[3];
        public UIControl[] NPC_Frame = new UIControl[3];

        public UIControl[] NPC_HPProgress = new UIControl[3];
        public UIControl[] NPC_HPFrame = new UIControl[3];

        public UIControl[] NPC_APProgress = new UIControl[3];
        public UIControl[] NPC_APFrame = new UIControl[3];

        public NPC_Class[] NPCs = new NPC_Class[3];

        /// <inheritdoc/>
        public override void OnStart()
        {
            int position = 0;
            for (int i = 0; i < 3; i++)
            {
                if (NPCs[i] == null)
                    continue;
                if (NPC_Character[position] != null && NPCs[i].charTexture != null)
                {
                    if (NPCs[i].charTexture != null)
                        NPC_Character[position].Get<Image>().Brush = new TextureBrush(NPCs[i].charTexture);

                    NPC_Background[position].Get<Image>().Brush = new TextureBrush(backgroundTex);
                    NPC_Frame[position].Get<Image>().Brush = new TextureBrush(frameTex);

                    var HPProgress = NPC_HPProgress[position].Get<ProgressBar>();
                    HPProgress.Value = (float)NPCs[i].healthPoints / NPCs[i].maxHealth;
                    HPProgress.BarBrush = new TextureBrush(HPFillTex);
                    HPProgress.BackgroundBrush = new TextureBrush(HPUnderTex);
                    NPC_HPFrame[position].Get<Image>().Brush = new TextureBrush(HPFrameTex);

                    var APProgress = NPC_APProgress[position].Get<ProgressBar>();
                    APProgress.Value = NPCs[i].actionPoints;
                    APProgress.BarBrush = new TextureBrush(APFillTex);
                    APProgress.BackgroundBrush = new TextureBrush(APUnderTex);
                    NPC_APFrame[position].Get<Image>().Brush = new TextureBrush(APFrameTex);

                    position++;
                }
            }

            for (int i = position; i < 3; i++)
            {
                if (NPC_Background[i] != null)
                {
                    var image = NPC_Background[i].Get<Image>();
                    if (image != null)
                        image.Brush = null;
                }
                if (NPC_Frame[i] != null)
                {
                    var image = NPC_Frame[i].Get<Image>();
                    if (image != null)
                        image.Brush = null;
                }
                if (NPC_HPProgress[i] != null)
                {
                    var progress = NPC_HPProgress[i].Get<ProgressBar>();
                    if (progress.Visible)
                    {
                        progress.Visible = false;
                    }
                }
                if (NPC_HPFrame[i] != null)
                {
                    var image = NPC_HPFrame[i].Get<Image>();
                    if (image != null)
                        image.Brush = null;
                }
                if (NPC_APProgress[i] != null)
                {
                    var progress = NPC_APProgress[i].Get<ProgressBar>();
                    if (progress.Visible)
                    {
                        progress.Visible = false;
                    }
                }
                if (NPC_APFrame[i] != null)
                {
                    var image = NPC_APFrame[i].Get<Image>();
                    if (image != null)
                        image.Brush = null;
                }

            }
        }

        /// <inheritdoc/>
        public override void OnEnable()
        {
            // Here you can add code that needs to be called when script is enabled (eg. register for events)
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
        }

        /// <inheritdoc/>
        public override void OnUpdate()
        {
            int position = 0;
            for (int i = 0; i < 3; i++)
            {
                if (NPCs[i] == null)
                    continue;
                if (NPC_Character[position] != null && NPCs[i].charTexture != null)
                {
                    if (NPCs[i].charTexture != null)
                        NPC_Character[position].Get<Image>().Brush = new TextureBrush(NPCs[i].charTexture);

                    NPC_Background[position].Get<Image>().Brush = new TextureBrush(backgroundTex);
                    NPC_Frame[position].Get<Image>().Brush = new TextureBrush(frameTex);

                    var HPProgress = NPC_HPProgress[position].Get<ProgressBar>();
                    HPProgress.Value = (float)NPCs[i].healthPoints / NPCs[i].maxHealth;
                    HPProgress.BarBrush = new TextureBrush(HPFillTex);
                    HPProgress.BackgroundBrush = new TextureBrush(HPUnderTex);
                    NPC_HPFrame[position].Get<Image>().Brush = new TextureBrush(HPFrameTex);

                    var APProgress = NPC_APProgress[position].Get<ProgressBar>();
                    APProgress.Value = NPCs[i].actionPoints;
                    APProgress.BarBrush = new TextureBrush(APFillTex);
                    APProgress.BackgroundBrush = new TextureBrush(APUnderTex);
                    NPC_APFrame[position].Get<Image>().Brush = new TextureBrush(APFrameTex);

                    position++;
                }
            }

            for (int i = position; i < 3; i++)
            {
                if (NPC_Background[i] != null)
                {
                    var image = NPC_Background[i].Get<Image>();
                    if (image != null)
                        image.Brush = null;
                }
                if (NPC_Frame[i] != null)
                {
                    var image = NPC_Frame[i].Get<Image>();
                    if (image != null)
                        image.Brush = null;
                }
                if (NPC_HPProgress[i] != null)
                {
                    var progress = NPC_HPProgress[i].Get<ProgressBar>();
                    if (progress.Visible)
                    {
                        progress.Visible = false;
                    }
                }
                if (NPC_HPFrame[i] != null)
                {
                    var image = NPC_HPFrame[i].Get<Image>();
                    if (image != null)
                        image.Brush = null;
                }
                if (NPC_APProgress[i] != null)
                {
                    var progress = NPC_APProgress[i].Get<ProgressBar>();
                    if (progress.Visible)
                    {
                        progress.Visible = false;
                    }
                }
                if (NPC_APFrame[i] != null)
                {
                    var image = NPC_APFrame[i].Get<Image>();
                    if (image != null)
                        image.Brush = null;
                }

            }
        }
    }

};
