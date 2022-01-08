using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CollieLab.GameSystems
{
    public class ScreenFadeFeature : ScriptableRendererFeature
    {
        public FadeSettings settings = new();

        private ScreenFadePass renderPass = null;
        public ScreenFadePass RenderPass
        {
            get => renderPass;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(renderPass);
        }

        public override void Create()
        {
            renderPass = new ScreenFadePass(settings);
        }
    }

    [Serializable]
    public class FadeSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

        public Color fadeColor = Color.black;
    }
}
