Shader "Render Features/Sobel Outline"
{
    Properties
    {
        _OutlineColor("Outline color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline thickness", Float) = 1
        _OutlineDepthMultiplier("Outline depth multiplier", Float) = 0
        _OutlineDepthBias("Outline depth bias", Float) = 0
        _OutlineNormalMultiplier("Outline normal bias", Float) = 0
        _OutlineNormalBias("Outline normal bias", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        ZWrite Off Cull Off

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment frag

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineThickness;
                float _OutlineDepthMultiplier;
                float _OutlineDepthBias;
                float _OutlineNormalMultiplier;
                float _OutlineNormalBias;
            CBUFFER_END

            float ApplySobelOperator(TEXTURE2D_PARAM(tex, s), float2 uv)
            {
                float2 delta = _ScreenSize.zw * _OutlineThickness;

                float4 hr = float4(0, 0, 0, 0);
                float4 vt = float4(0, 0, 0, 0);

                hr += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(-delta.x, delta.y)) * 1;
                hr += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(delta.x, delta.y)) * -1;
                hr += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(-delta.x, 0)) * 2;
                hr += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(delta.x, 0)) * -2;
                hr += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(-delta.x, -delta.y)) * 1;
                hr += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(delta.x, -delta.y)) * -1;

                vt += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(-delta.x, delta.y)) * 1;
                vt += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(0, delta.y)) * 2;
                vt += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(delta.x, delta.y)) * 1;
                vt += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(-delta.x, -delta.y)) * -1;
                vt += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(0, -delta.y)) * -2;
                vt += SAMPLE_TEXTURE2D_X(tex, s, uv + float2(delta.x, -delta.y)) * -1;

                return sqrt(dot(hr, hr) + dot(vt, vt));
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                float depthOutline = ApplySobelOperator(
                    TEXTURE2D_ARGS(_CameraDepthTexture, sampler_CameraDepthTexture), input.texcoord
                );
                depthOutline = pow(abs(depthOutline * _OutlineDepthMultiplier), _OutlineDepthBias);

                float normalsOutline = ApplySobelOperator(
                    TEXTURE2D_ARGS(_CameraNormalsTexture, sampler_CameraNormalsTexture), input.texcoord
                );
                normalsOutline = pow(abs(normalsOutline * _OutlineNormalMultiplier), _OutlineNormalBias);
            
                float4 sceneColor = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, input.texcoord);

                return lerp(sceneColor, _OutlineColor, saturate(max(depthOutline, normalsOutline)));
            }

            ENDHLSL
        }
    }
}
