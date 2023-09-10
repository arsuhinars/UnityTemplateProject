using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.RenderFeatures
{
    public class HullOutlineFeature : ScriptableRendererFeature
    {
        [Serializable]
        public class RenderSettings
        {
            public Material m_outlineMaterial;
            public LayerMask m_layerMask;
        }

        private class HullOutlinePass : ScriptableRenderPass
        {
            private Material m_material;
            private FilteringSettings m_filteringSettings;
            private readonly List<ShaderTagId> m_shaderPassId = new() { new("UniversalForward"), new("SRPDefaultUnlit") };

            public HullOutlinePass(Material material, int layerMask)
            {
                m_material = material;
                renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
                m_filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var drawingSettings = CreateDrawingSettings(
                    m_shaderPassId,
                    ref renderingData,
                    renderingData.cameraData.defaultOpaqueSortFlags
                );
                drawingSettings.overrideMaterial = m_material;

                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_filteringSettings);
            }
        }

        [SerializeField] private RenderSettings m_settings;

        private HullOutlinePass m_pass;

        public override void Create()
        {
            m_pass = new HullOutlinePass(m_settings.m_outlineMaterial, m_settings.m_layerMask);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_pass);
        }
    }
}
