using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.RenderFeatures
{
    public class SobelOutlineFeature : ScriptableRendererFeature
    {
        private class SobelOutlinePass : ScriptableRenderPass
        {
            private readonly ProfilingSampler m_profilingSample = new("SobelOutlineBlit");
            private Material m_material;
            private RTHandle m_cameraColorTarget;

            public SobelOutlinePass(Material material)
            {
                m_material = material;
                renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
            }

            public void SetTarget(RTHandle colorHandle)
            {
                m_cameraColorTarget = colorHandle;
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                ConfigureTarget(m_cameraColorTarget);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (renderingData.cameraData.cameraType != CameraType.Game)
                {
                    return;
                }

                if (m_material == null)
                {
                    return;
                }

                var cmd = CommandBufferPool.Get();

                using (new ProfilingScope(cmd, m_profilingSample))
                {
                    Blitter.BlitCameraTexture(cmd, m_cameraColorTarget, m_cameraColorTarget, m_material, 0);
                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }
        }

        [SerializeField] private Material m_material;

        private SobelOutlinePass m_pass;

        public override void Create()
        {
            m_pass = new SobelOutlinePass(m_material);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType == CameraType.Game)
            {
                m_pass.ConfigureInput(ScriptableRenderPassInput.Color);
                m_pass.ConfigureInput(ScriptableRenderPassInput.Depth);
                m_pass.ConfigureInput(ScriptableRenderPassInput.Normal);
                m_pass.SetTarget(renderer.cameraColorTargetHandle);
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType == CameraType.Game)
            {
                renderer.EnqueuePass(m_pass);
            }
        }
    }
}
