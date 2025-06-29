Shader "Custom/Circle"
{
    Properties {
        _Progress ("Progress", Range(0,1)) = 1
        _Color ("Color", Color) = (1,1,1,1)
        _Smoothness ("Edge Smoothness", Range(0,0.1)) = 0.02
    }
    SubShader {
        Tags { "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "IgnoreProjector" = "True"  }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attribute {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varying {
                float2 uv : TEXCOORD0;
                float4 posCW : SV_POSITION;
            };
            
            float _Progress;
            float4 _Color;
            float _Smoothness;

            Varying vert (Attribute IN) {
                Varying OUT;
                OUT.posCW = TransformObjectToHClip(IN.vertex);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varying IN) : SV_Target {
                float2 uv = IN.uv - float2(0.5, 0.5);
                
                float angle = (atan2(uv.y, uv.x) / 6.2831853) + 0.5;
                
                float cutoff = step(angle,_Progress);

                float4 col = _Color;
                col.a *= (1.0 - cutoff);
                return col;
            }
            ENDHLSL
        }
    }
}