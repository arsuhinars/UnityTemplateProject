Shader "Render Features/Hull Outline"
{
    Properties
    {
        _OutlineColor("Outline color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline thickness", Float) = 1
        [KeywordEnum(Vertex, Color)] _Normal ("Normals source", Float) = 0
        [KeywordEnum(Clip, World)] _Render ("Outline rendering space", Float) = 0.0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "ForceNoShadowCasting" = "True"    
        }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            Cull Front

            HLSLPROGRAM

            #pragma multi_compile _NORMAL_VERTEX _NORMAL_COLOR
            #pragma multi_compile _RENDER_CLIP_SPACE _RENDER_WORLD_SPACE

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineThickness;
            CBUFFER_END

            struct appdata {
                float3 position : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f {
                float4 position : SV_POSITION;
            };

            v2f vert(appdata IN) {
                v2f OUT;
                
                #if _NORMAL_VERTEX
                float3 n = IN.normal;
                #elif _NORMAL_COLOR
                float3 n = UnpackNormalRGB(IN.color);
                #endif

                #if _RENDER_CLIP_SPACE
                n = mul((float3x3)UNITY_MATRIX_MVP, n);
                OUT.position = TransformObjectToHClip(IN.position);
                OUT.position.xy += normalize(n.xy) * _OutlineThickness / _ScreenParams.xy * OUT.position.w * 2;

                #elif _RENDER_WORLD_SPACE

                n = TransformObjectToWorldNormal(n);
                OUT.position = TransformWorldToHClip(
                    TransformObjectToWorld(IN.position) + n * _OutlineThickness
                );
                #endif

                return OUT;
            }

            float4 frag(v2f v) : SV_TARGET {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}
