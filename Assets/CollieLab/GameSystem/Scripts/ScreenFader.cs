using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CollieLab.GameSystems
{
    public class ScreenFader : MonoBehaviour
    {
        #region Serialized Field
        [Header("Renderer")]
        [SerializeField] private UniversalRendererData rendererData = null;

        [Header("Fade Settings")]
        [SerializeField] private float fadeDuration = 1.5f;
        [SerializeField] private AnimationCurve fadeCurve = null;
        #endregion

        #region Private Field
        private static readonly int AlphaProperty = Shader.PropertyToID("_Alpha");

        private ScreenFadeFeature fadeFeature = null;
        #endregion

        private void Awake()
        {
            SetupFadeFeature();
        }

        #region Initialize
        public void SetupFadeFeature()
        {
            ScriptableRendererFeature rendererFeature = rendererData.rendererFeatures.Find(i => i is ScreenFadeFeature);
            if (rendererFeature is ScreenFadeFeature fadeFeature)
            {
                this.fadeFeature = fadeFeature;
                this.fadeFeature.SetActive(false);
            }
        }
        #endregion

        #region Fade Controls
        /// <summary>
        /// Screen Fade In.
        /// </summary>
        public IEnumerator FadeIn()
        {
            fadeFeature.SetActive(true);
            Material fadeMat = fadeFeature.RenderPass.RuntimeMat;
            yield return LerpAlphaValue(fadeMat, 1f, fadeDuration, fadeCurve);
        }

        /// <summary>
        /// Screen Fade Out.
        /// </summary>
        public IEnumerator FadeOut()
        {
            Material fadeMat = fadeFeature.RenderPass.RuntimeMat;
            yield return LerpAlphaValue(fadeMat, 0f, fadeDuration, fadeCurve, () =>
            {
                fadeFeature.SetActive(false);
            });
        }

        private IEnumerator LerpAlphaValue(Material material, float targetAlpha, float duration = 1f, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float alphaValue = Mathf.Lerp(material.GetFloat(AlphaProperty), targetAlpha, curve.Evaluate(elapsedTime / duration));
                material.SetFloat(AlphaProperty, alphaValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            material.SetFloat(AlphaProperty, targetAlpha);
            done?.Invoke();
        }
        #endregion
    }
}
