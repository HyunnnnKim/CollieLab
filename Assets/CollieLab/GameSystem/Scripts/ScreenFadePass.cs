using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CollieLab.GameSystems
{
    public class ScreenFadePass : ScriptableRenderPass
    {
        #region Private Field
        private const string ProfilerTag = "Screen Fade Pass";
        private static readonly int FadeColorProperty = Shader.PropertyToID("_FadeColor");

        private FadeSettings fadeSettings = null;
        private Material runtimeMat = null;
        public Material RuntimeMat
        {
            get => runtimeMat;
        }
        #endregion

        public ScreenFadePass(FadeSettings settings)
        {
            this.fadeSettings = settings;
            renderPassEvent = settings.renderPassEvent;
            if (runtimeMat == null)
                runtimeMat = CoreUtils.CreateEngineMaterial("Custom/Fader");
            runtimeMat.SetColor(FadeColorProperty, fadeSettings.fadeColor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer command = CommandBufferPool.Get(ProfilerTag);

            RenderTargetIdentifier source = BuiltinRenderTextureType.CameraTarget;
            RenderTargetIdentifier destination = BuiltinRenderTextureType.CurrentActive;

            command.Blit(source, destination, runtimeMat);
            context.ExecuteCommandBuffer(command);

            CommandBufferPool.Release(command);
        }
    }
}
